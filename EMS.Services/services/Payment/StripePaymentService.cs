using EMS.Core.DTOs.Events;
using EMS.Core.DTOs.Payment;
using EMS.Core.Repository.Interfaces;
using EMS.Core.Services.Interfaces;
using EMS.Repository.Data.Contexts;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.services.Payment
{
    public abstract class StripePaymentService<T> : IPaymentService<T> where T : class
    {
        protected readonly IConfiguration _config;
        protected readonly AppDbContext _context;

        public StripePaymentService(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }
        public async Task<PaymentResponse> CreateOrUpdateIntentAsync(int entityId)
        {
            var entity = await GetEntityAsync(entityId);
            if (entity == null)
                return null;

            var amount = await GetAmountAsync(entityId);

     
            if (amount <= 0)
                return null;

            var paymentIntentId = GetPaymentIntentId(entity);
            var service = new PaymentIntentService();
            PaymentIntent intent;

            if (string.IsNullOrEmpty(paymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(amount * 100), 
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await service.CreateAsync(options);

                await SavePaymentIntentIdAsync(
                    entityId,
                    intent.Id,
                    intent.ClientSecret
                );
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(amount * 100)
                };

                intent = await service.UpdateAsync(paymentIntentId, options);
            }

            await _context.SaveChangesAsync();

            return new PaymentResponse
            {
                PaymentIntentId = intent.Id,
                ClientSecret = intent.ClientSecret,
                Amount = amount,
                Currency = "usd"
            };
        }

        public async Task HandlePaymentSucceeded(string paymentIntentId)
        {
            var entity = await FindByIntent(paymentIntentId);
            if (entity == null) return;

            await UpdateStatusAsync(entity, true);
            await _context.SaveChangesAsync();
        }
        public async Task HandlePaymentFailed(string paymentIntentId)
        {
            var entity = await FindByIntent(paymentIntentId);
            if (entity == null) return;

            await UpdateStatusAsync(entity, false);
            await _context.SaveChangesAsync();
        }
        protected abstract Task<T> GetEntityAsync(int id);
        protected abstract string GetPaymentIntentId(T entity);
        protected abstract Task SavePaymentIntentIdAsync(int id, string paymentIntentId, string clientSecret);
        protected abstract Task<decimal> GetAmountAsync(int id);
        protected abstract Task<T> FindByIntent(string paymentIntentId);
        protected abstract Task UpdateStatusAsync(T entity, bool succeeded);
    }



}

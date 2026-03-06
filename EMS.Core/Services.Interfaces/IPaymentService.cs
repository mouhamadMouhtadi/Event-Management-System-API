using EMS.Core.DTOs.Events;
using EMS.Core.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Services.Interfaces
{
    public interface IPaymentService<T>
    {
        Task<PaymentResponse> CreateOrUpdateIntentAsync(int entityId);
        Task HandlePaymentSucceeded(string paymentIntentId);
        Task HandlePaymentFailed(string paymentIntentId);
    }


}

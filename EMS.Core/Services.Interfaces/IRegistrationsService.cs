using EMS.Core.DTOs.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Services.Interfaces
{
    public interface IRegistrationsService
    {
        Task<bool> RegisterAsync (string UserId,int EventId);
        Task<bool> UnRegisterAsync (string UserId,int EventId);

        Task<List<RegistrationResponse>> GetUserRegistrationAsync (string UserId);
    }
}

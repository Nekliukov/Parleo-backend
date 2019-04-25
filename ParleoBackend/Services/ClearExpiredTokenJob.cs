using Parleo.BLL.Interfaces;
using FluentScheduler;
using System.Threading.Tasks;

namespace ParleoBackend.Services
{
    public class ClearExpiredTokenJob: IJob
    {
        private readonly IAccountService _userService;


        public ClearExpiredTokenJob(IAccountService userService)
        {
            _userService = userService;
        }

        public void Execute()
        {
            ClearExpiredTokensAsync().Wait();
        }

        private async Task ClearExpiredTokensAsync()
        {
            await _userService.ClearExpiredTokensAsync();
        }
    }
}

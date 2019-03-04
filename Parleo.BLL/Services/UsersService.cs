using Parleo.BLL.Interfaces;
using Parleo.DAL.Interfaces;

namespace Parleo.BLL.Services
{
    public class UsersService : IUsersService
    {
        private IUsersRepository _repository;

        public UsersService(IUsersRepository repository)
        {
            _repository = repository;
        }
    }
}

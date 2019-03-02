using Parleo.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parleo.DAL.Contexts;
using Parleo.DAL.Entities;
using Parleo.DAL.Interfaces;

namespace Parleo.BLL.Services
{
    public class UserService
    {
        private IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
    }
}

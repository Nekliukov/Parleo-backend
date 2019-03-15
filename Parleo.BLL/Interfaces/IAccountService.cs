﻿using Parleo.BLL.Models;
using Parleo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<Credentials> AuthenticateAsync(AuthorizationModel authorizationModel);

        Task<IEnumerable<UserModel>> GetUsersPageAsync(int number);

        Task<UserModel> GetUserByIdAsync(Guid id);

        Task<UserModel> CreateUserAsync(AuthorizationModel authorizationModel);

        Task<bool> UpdateUserAsync(UserModel user);

        Task<bool> DisableUserAsync(Guid id);

    }
}
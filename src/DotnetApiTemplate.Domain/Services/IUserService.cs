﻿using DotnetApiTemplate.Domain.DTO;
using DotnetApiTemplate.Domain.Models;

namespace DotnetApiTemplate.Domain.Services;

/// <summary>
/// Interface for <see cref="IUserService"/>.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Logs in the User
    /// </summary>
    /// <param name="userLoginDto"> the User login DTO</param>
    /// <returns>The User</returns>
    Task<User?> LoginAsync(UserLoginDTO userLoginDto);


    /// <summary>
    /// Register a User
    /// </summary>
    /// <param name="user"> the User Registration DTO</param>
    /// <returns> The User></returns>
    Task<User?> RegisterAsync(UserRegistrationDTO user);
}

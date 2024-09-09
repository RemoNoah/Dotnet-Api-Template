﻿using DotnetApiTemplate.Domain.DTO;
using DotnetApiTemplate.Domain.Models;
using DotnetApiTemplate.Domain.Services;
using DotnetApiTemplate.Services.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly JwtTokenGenerator _jwtGenerator;
    private readonly IUserService _userService;
    private readonly int _expirationMinutes;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _key;

    public UserController(IConfiguration configuration, IUserService userService, JwtTokenGenerator jwtGenerator)
    {
        _config = configuration;
        _userService = userService;
        _jwtGenerator = jwtGenerator;
        _key = _config.GetValue<string>("Jwt:Key")!;
        _expirationMinutes = _config.GetValue<int>("Jwt:ExpireMinutes");
        _issuer = _config.GetValue<string>("Jwt:Issuer")!;
        _audience = _config.GetValue<string>("Jwt:Audience")!;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult> Register(UserRegistrationDTO user)
    {
        if (user == null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName))
        {
            return BadRequest("Enter all User infos");
        }
        User? registeredUser = await _userService.RegisterAsync(user);

        if (registeredUser != null)
        {
            return Ok(_jwtGenerator.GenerateToken(registeredUser, _key, _expirationMinutes, _issuer, _audience));
        }
        return BadRequest("Email already Exists");
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult> Login(UserLoginDTO user)
    {
        if (user == null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
        {
            return BadRequest("Enter all User infos");
        }

        User? logedInUser = await _userService.LoginAsync(user);

        if (logedInUser != null)
        {
            return Ok(_jwtGenerator.GenerateToken(logedInUser, _key, _expirationMinutes, _issuer, _audience));
        }
        return BadRequest("Username or Password are wrong");
    }
}
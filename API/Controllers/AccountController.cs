using API.Dispatchers.Interfaces;
using API.DTO.Accounting;
using API.Entities.Accounting;
using API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

/// <remarks>
/// Represents a controller for managing user accounts.
/// </remarks>
public class AccountController(
    IDispatcher dispatcher,
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    IMapper mapper) : BaseApiController(dispatcher)
{
    #region private fields
    
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IMapper _mapper = mapper;
    private static readonly string[] _errorIsTaken = ["Username is taken"];

    #endregion private fields

    #region public

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerDto">The registration information.</param>
    /// <returns>The registered user.</returns>
    [HttpPost(nameof(Register))]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExistAsync(registerDto.UserName))
            return BadRequest(_errorIsTaken);

        AppUser user = _mapper.Map<AppUser>(registerDto);
        
        user.UserName = registerDto.UserName.ToLower();

        IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));

        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, "Member");

        if (!roleResult.Succeeded)
            return BadRequest(roleResult.Errors.Select(e => e.Description));

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Token = await _tokenService.CreateToken(user),
        };
    }

    #endregion public

    #region private

    /// <summary>
    /// Checks if a user exists.
    /// </summary>
    /// <param name="userName">The username to check.</param>
    private async Task<bool> UserExistAsync(string userName)
    {
        return await _userManager.Users.AnyAsync(u => u.UserName == userName.ToLower());
    }

    #endregion private
    
}
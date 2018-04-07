using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
  [Route("api/[controller]")]
  public class AuthController : Controller
  {
    private readonly IAuthRepository _repo;
    public AuthController(IAuthRepository repo)
    {
      _repo = repo;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBodyAttribute] UserForRegisterDto userForRegisteredDto)
    {
      userForRegisteredDto.Username = userForRegisteredDto.Username.ToLower();

      if (await _repo.UserExists(userForRegisteredDto.Username))
        ModelState.AddModelError("Username", "Username already exists");

      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var userToCreate = new User
      {
        Username = userForRegisteredDto.Username
      };

      var createUser = await _repo.Register(userToCreate, userForRegisteredDto.password);

      return StatusCode(201);
    }
  }
}
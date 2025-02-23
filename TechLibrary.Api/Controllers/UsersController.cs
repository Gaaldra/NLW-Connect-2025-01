using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Users;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult Create([FromBody] RequestUserJson request)
    {
        try
        {
            RegisterUserUseCase user = new();

            ResponseUserJson response = user.Execute(request);

            return Created(string.Empty, response);
        }
        catch (TechLibraryException ex)
        {
            return BadRequest(new ResponseErrorMessagesJson()
            {
                Errors = ex.GetErrorMessages()
            });
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ResponseErrorMessagesJson()
                {
                    Errors = ["Erro desconhecido"]
                });
        }
    }
}


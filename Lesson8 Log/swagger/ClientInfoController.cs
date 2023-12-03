/*
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Example
{
    public class ResponseDto
    {
        public int Test { get; init; }
    }
    
    /// <summary>
    /// информация о авторизации и регистрации
    /// </summary>
    [Route("public/client/info")]
    [ApiController]
    public class ClientRegistrationInfoController : ControllerBase
    {
        /// <summary>
        /// Запрос информации о авторизации в кабинете
        /// </summary>
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [HttpGet("auth")]
        public async Task<IActionResult> CabinetAuthInfoAsync([FromQuery] string alias)
        {
            return Ok();
        }
    }
}
*/

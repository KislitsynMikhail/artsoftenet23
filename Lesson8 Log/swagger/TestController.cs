using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    [Route("public/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>
        /// Какой-то запрос
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [HttpGet("configuration")]
        public async Task<IActionResult> CabinetAuthInfoAsync(string id)
        {
            var response = _configuration.GetSection("test");
            return Ok(response);
        }
    }
}

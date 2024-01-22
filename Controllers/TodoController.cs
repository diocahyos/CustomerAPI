using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CustomerAPI.Services;
using CustomerAPI.Dtos.Todo;

namespace CustomerAPI.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly JsonTypicodeServices _jsonTipecodeApi;
        public TodoController(JsonTypicodeServices jsonTipecodeApi)
        {
            _jsonTipecodeApi = jsonTipecodeApi;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTodos()
        {
            var result = await _jsonTipecodeApi.GetTodosAsync();
            var response = new TodoListResponse()
            {
                status = "success",
                message = "",
                data = result
            };
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetTodoById(int id)
        {
            try
            {
                var result = _jsonTipecodeApi.GetTodoById(id);
                return Ok(result);
            } catch(HttpRequestException ex) when (ex.StatusCode.HasValue) {
                return StatusCode((int)ex.StatusCode.Value, ex.Message);
            } catch(Exception ex) { 
                return StatusCode(500, ex.Message);
            }
        }
    }
}

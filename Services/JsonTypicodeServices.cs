using CustomerAPI.Dtos.Todo;
using System.Text.Json;
using System.Xml;

namespace CustomerAPI.Services
{
    public class JsonTypicodeServices
    {
        private readonly HttpClient _httpClient;
        //private readonly ILogger _logger;

        public JsonTypicodeServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
          //  _logger = logger;
        }

        public async Task<List<TodoDto>> GetTodosAsync()
        {
            var response = await _httpClient.GetAsync("/todos");
            response.EnsureSuccessStatusCode();
            using (var responseStream = await response.Content.ReadAsStreamAsync()) {
                return await JsonSerializer.DeserializeAsync<List<TodoDto>>(responseStream);
            }
        }

        public TodoResponse GetTodoById(int id) {
            var response = _httpClient.GetAsync($"/todos/{id}").Result;
            try
            {
                response.EnsureSuccessStatusCode();
                using (var responseStream = response.Content.ReadAsStreamAsync().Result) {
                    return JsonSerializer.Deserialize<TodoResponse>(responseStream);
                }
            } catch (Exception ex) {
                throw;
            }
        }
    }
}

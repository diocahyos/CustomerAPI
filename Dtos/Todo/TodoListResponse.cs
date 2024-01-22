namespace CustomerAPI.Dtos.Todo
{
    public class TodoListResponse : BaseResponse
    {
        public List<TodoDto> data { get; set; }
    }
}

using testing.Application.Common.Mappings;
using testing.Domain.Entities;

namespace testing.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem> {
    public string? Title { get; set; }

    public bool Done { get; set; }
}
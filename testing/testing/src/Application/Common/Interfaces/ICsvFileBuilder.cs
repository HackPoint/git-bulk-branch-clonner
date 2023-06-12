using testing.Application.TodoLists.Queries.ExportTodos;

namespace testing.Application.Common.Interfaces;

public interface ICsvFileBuilder {
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
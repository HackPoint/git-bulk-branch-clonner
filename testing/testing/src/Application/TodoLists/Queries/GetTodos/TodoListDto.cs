﻿using testing.Application.Common.Mappings;
using testing.Domain.Entities;

namespace testing.Application.TodoLists.Queries.GetTodos;

public class TodoListDto : IMapFrom<TodoList> {
    public TodoListDto() {
        Items = new List<TodoItemDto>();
    }

    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Colour { get; set; }

    public IList<TodoItemDto> Items { get; set; }
}
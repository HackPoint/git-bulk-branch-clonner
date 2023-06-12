using testing.Application.Common.Exceptions;
using testing.Application.TodoLists.Commands.CreateTodoList;
using testing.Application.TodoLists.Commands.DeleteTodoList;
using testing.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace testing.Application.IntegrationTests.TodoLists.Commands;

using static Testing;

public class DeleteTodoListTests : BaseTestFixture {
    [Test]
    public async Task ShouldRequireValidTodoListId() {
        var command = new DeleteTodoListCommand(99);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTodoList() {
        var listId = await SendAsync(new CreateTodoListCommand {
            Title = "New List"
        });

        await SendAsync(new DeleteTodoListCommand(listId));

        var list = await FindAsync<TodoList>(listId);

        list.Should().BeNull();
    }
}
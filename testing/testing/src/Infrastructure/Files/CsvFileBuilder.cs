using System.Globalization;
using testing.Application.Common.Interfaces;
using testing.Application.TodoLists.Queries.ExportTodos;
using testing.Infrastructure.Files.Maps;
using CsvHelper;

namespace testing.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder {
    public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records) {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream)) {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Context.RegisterClassMap<TodoItemRecordMap>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }
}
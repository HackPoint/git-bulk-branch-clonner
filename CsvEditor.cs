using System.Text;
using System.Text.RegularExpressions;

namespace git_manager;

public class CsvEditor {
    public static IList<string> ReadCsv(string path, bool toLinq = false) {
        if (toLinq) {
            ReadAndWriteCsvWithLinq(path);
        }
        var lines = new List<string>();
        using (var reader = new StreamReader(path)) {
            var line = reader.ReadLine();

            var values = new List<string>();
            while (line != null) {
                values.Clear();
                Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                var cols = CSVParser.Split(line);
                for (int i = 0; i < cols.Length; i++) {
                    if (cols[i] == "properties") {
                        StringBuilder sb = new StringBuilder();
                        for (int j = i; j < cols.Length - i; j++) {
                            sb.Append(cols[j]);
                        }
                    }
                    values.Add(cols[i]);
                }
                var newLine = string.Join(",", values);
                lines.Add(newLine);
                line = reader.ReadLine();
            }
        }

        return lines;
    }

    private static void ReadAndWriteCsvWithLinq(string path) {
        string[] csvLines = File.ReadAllLines(path);
        string header = csvLines.FirstOrDefault(l => !String.IsNullOrWhiteSpace(l));
        if (header != null) {
            int balanceIndex = Array.FindIndex<string>(
                header.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                , col => col.Equals("content", StringComparison.OrdinalIgnoreCase));
            if (balanceIndex >= 0) {
                var allCols = csvLines
                    .Select(l => new { Columns = l.Split(new[] { ',' }, StringSplitOptions.None) })
                    .Where(x => x.Columns.Length > balanceIndex)
                    .Select(x => string.Join(",", x.Columns
                        // .Where((col, index) => index != balanceIndex)
                        .Select(col => col.Trim())));
                // rewrite the file with all columns but balance:
                File.WriteAllLines(path, allCols);
            }
        }
    }
}
using System.Diagnostics;
using System.Text;
using Octokit;

class Program {
    public static void Main(string[] args) {
        if (!args.Any(string.IsNullOrEmpty)) {
            Console.WriteLine(
                "No arguments being sent to service");
            Console.ReadLine();
            return;
        }
        Console.WriteLine("Token {0}", args[0]);
        Console.WriteLine("Folder {0}", args[1]);
       
        try {
            var client = new GitHubClient(new ProductHeaderValue("OctokitTests")) {
                Credentials = new Credentials(args[0], AuthenticationType.Bearer)
            };
            var org = client.Organization.Get("Arborknot").Result;
            var listOfRepositories = client.Repository.GetAllForOrg(org.Name).Result;

            foreach (var repository in listOfRepositories) {
                Console.WriteLine(repository.Name);
                Console.WriteLine(repository.CloneUrl);

                var clone = RunClone(repository.CloneUrl, args[1]);
                clone.Wait();

                if (clone.IsCompleted) {
                    Console.WriteLine(repository.Name + "clone completed!");
                }
            }
        }
        catch (AggregateException aex) {
            aex.InnerExceptions.AsParallel().ForAll(ex => Console.WriteLine(ex));
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
        }

        Console.ReadKey();
    }

    private static Task<int> RunClone(string url, string cloneToLocation) {
        var tcs = new TaskCompletionSource<int>();

        var cloneProcess = new Process {
            StartInfo = { FileName = "git", Arguments = $"clone {url}", WorkingDirectory = cloneToLocation },
            EnableRaisingEvents = true
        };

        cloneProcess.Exited += (sender, args) =>
        {
            tcs.SetResult(cloneProcess.ExitCode);
            cloneProcess.Dispose();
        };

        cloneProcess.Start();
        return tcs.Task;
    }

    public static string RemoveColumnDelimitersInsideValues(string input) {
        const char valueDelimiter = '"';
        const char columnDelimiter = ',';

        StringBuilder output = new StringBuilder();

        bool isInsideValue = false;
        for (var i = 0; i < input.Length; i++) {
            var currentChar = input[i];

            if (currentChar == valueDelimiter) {
                isInsideValue = !isInsideValue;
                output.Append(currentChar);
                continue;
            }

            if (currentChar != columnDelimiter || !isInsideValue) {
                output.Append(currentChar);
            }
            // else ignore columnDelimiter inside value
        }

        return output.ToString();
    }
}
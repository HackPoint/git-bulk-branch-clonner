using System.Diagnostics;
using Octokit;

class Program {
    private static string _token = "";

    public static void Main(string[] args) {
        try {
            var client = new GitHubClient(new ProductHeaderValue("OctokitTests"));
            client.Credentials = new Credentials(_token, AuthenticationType.Bearer);
            var org = client.Organization.Get("Arborknot").Result;
            var listOfRepositories = client.Repository.GetAllForOrg(org.Name).Result;

            foreach (var repository in listOfRepositories) {
                Console.WriteLine(repository.Name);
                Console.WriteLine(repository.CloneUrl);

                var clone = RunClone(repository.CloneUrl, "/Users/hackpoint/dev/ar-workspace");
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
            StartInfo = {FileName = "git", Arguments = $"clone `${url}` .", WorkingDirectory = cloneToLocation},
            EnableRaisingEvents = true
        };

        cloneProcess.Exited += (sender, args) => {
            tcs.SetResult(cloneProcess.ExitCode);
            cloneProcess.Dispose();
        };

        cloneProcess.Start();
        return tcs.Task;
    }
}
using System.Net;
using Octokit;
using Credentials = Octokit.Credentials;
using Repository = Octokit.Repository;

class Program {
    private static string _token = "";

    private static async Task<string> GetWebPage(string uri) {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", " 2022-11-28");
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer ${_token}");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
        httpClient.DefaultRequestHeaders.Add("User-Agent",
            @"Mozilla/5.0 (Windows NT 10; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0");

        var response =
            await httpClient.GetAsync(new Uri(uri, UriKind.Absolute), HttpCompletionOption.ResponseContentRead);

        return await response.Content.ReadAsStringAsync();
    }

    private static void Download(string token, string url) {
        var request = (HttpWebRequest) WebRequest.Create(url);
        request.Headers.Add(HttpRequestHeader.Authorization, string.Concat("token ", token));
        request.Accept = "application/vnd.github.v3.raw";
        request.UserAgent = "test app";
        using (var response = request.GetResponse()) {
            var encoding = System.Text.Encoding.UTF8;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding)) {
                var fileContent = reader.ReadToEnd();
            }
        }
    }

    public static void Main(string[] args) {
        try {
            var client = new GitHubClient(new ProductHeaderValue("OctokitTests"));
            client.Credentials = new Credentials(_token, AuthenticationType.Bearer);
            var org = client.Organization.Get("Arborknot").Result;
            var listOfRepositories = client.Repository.GetAllForOrg(org.Name).Result;

            foreach (var repository in listOfRepositories) {
                Download(_token, repository.CloneUrl);
            }
            /*// This works:
            Task<string> getPageTask = GetWebPage(org.ReposUrl);
            getPageTask.Wait();
            if (getPageTask.IsCompleted)
                Console.WriteLine(getPageTask.Result);*/
        }
        catch (AggregateException aex) {
            aex.InnerExceptions.AsParallel().ForAll(ex => Console.WriteLine(ex));
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
        }

        Console.ReadKey();
    }
}
using System.Reflection;

namespace git_manager;

public static class Utils {
    public static string CurrentFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
}
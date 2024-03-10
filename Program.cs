using System.Text.RegularExpressions;

class Program {
    /// <summary>
    /// Rename all files in the directory matching the given pattern. Can use match groups in the replacement name
    /// </summary>
    /// <param name="pattern">RegEx matching pattern</param>
    /// <param name="name">Replacement name pattern</param>
    /// <param name="recursive">Indicate if the search only applies to the current directory (false) or to subdirectories too (true).</param>
    /// <param name="dry">Mark this as a dry run or not</param>
    public static void Main(string pattern, string name, bool dry = false, bool recursive = false) {
        var regex = new Regex(pattern);
        var variable = new Regex(@"\%(?<index>\d+)|(?<!{){\s*(?<name>\w+)\s*}");
        var Current = new DirectoryInfo(Environment.CurrentDirectory);
        foreach (var file in Current.GetFiles("*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {
            var before = file.Name;
            var match = regex.Match(file.Name);
            if (!match.Success)
                continue;
            var after = variable.Replace(name, (replacement) => {
                if (!replacement.Success)
                    return string.Empty;
                if (replacement.Groups["index"].Success) {
                    var index = int.Parse(replacement.Groups["index"].Value);
                    return match.Groups[index].Value;
                } else if (replacement.Groups["name"].Success) {
                    return match.Groups[replacement.Groups["name"].Value].Value;
                } else {
                    return string.Empty;
                }
            });
            var parent = file.Directory;
            if (parent is null)
                continue;

            var root = Path.GetRelativePath(Current.FullName, parent.FullName);
            var root_b4 = Path.Combine(root, before);
            var root_afr = Path.Combine(root, after);
            Console.WriteLine($"RENAME {root_b4} -> {root_afr}");
            if (dry) {
                continue;
            }
            file.MoveTo(Path.Join(parent.FullName, after));
        }
    }

}
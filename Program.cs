using System.Text.RegularExpressions;

class Program {

    /*/// <summary>
    /// Rename all files by adding a prefix to the filename
    /// </summary>
    /// <param name="prefix">prefix to add</param>
    /// <param name="dry">mark this as a dry run or not</param>
    public static void Main(string prefix, bool dry = false) {
        var Current = new DirectoryInfo(Environment.CurrentDirectory);
        foreach (var file in Current.GetFiles("*", SearchOption.AllDirectories)) {
            var before = file.Name;
            var after = prefix + file.Name;
            var parent = file.Directory;
            if (parent is null)
                continue;
            
            if (dry) {
                var root = Path.GetRelativePath(Current.FullName, parent.FullName);
                var root_b4 = Path.Combine(root, before);
                var root_afr = Path.Combine(root, after);
                Console.WriteLine($"{root_b4} -> {root_afr}");
                continue;
            }
            file.MoveTo(Path.Join(parent.FullName, after));
        }
    }*/

    /// <summary>
    /// Rename all files in the directory matching the given pattern. Can use match groups in the replacement name
    /// </summary>
    /// <param name="pattern">RegEx matching pattern</param>
    /// <param name="name">Replacement name pattern</param>
    /// <param name="dry">mark this as a dry run or not</param>
    public static void Main(string pattern, string name, bool dry = false) {
        var regex = new Regex(pattern);
        var variable = new Regex(@"\%(?<index>\d+)|(?<!{){\s*(?<name>\w+)\s*}");
        var Current = new DirectoryInfo(Environment.CurrentDirectory);
        foreach (var file in Current.GetFiles("*", SearchOption.AllDirectories)) {
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
            Console.WriteLine($"{root_b4} -> {root_afr}");
            if (dry) {
                continue;
            }
            file.MoveTo(Path.Join(parent.FullName, after));
        }
    }

}
using System;
using System.IO;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main()
    {
        var path = @"path_to_your_project"; // Replace with your project directory

        foreach (var file in Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories))
        {
            var content = File.ReadAllText(file);

            var summaryRegex = new Regex(@"/// <summary>\s*/// (.+?)\s*/// </summary>");
            var swaggerRegex = new Regex(@"\[SwaggerOperation\(([^)]+?)\)\]");

            var matches = summaryRegex.Matches(content);

            foreach (Match match in matches)
            {
                var summary = match.Groups[1].Value.Trim();

                var swaggerMatch = swaggerRegex.Match(content, match.Index + match.Length);

                // Check if SwaggerOperation is found after the summary
                if (swaggerMatch.Success)
                {
                    var currentSwaggerContent = swaggerMatch.Groups[1].Value.Trim();

                    var newSwaggerContent = currentSwaggerContent + $", Summary = \"{summary}\"";

                    content = content.Replace(currentSwaggerContent, newSwaggerContent);
                }
            }

            File.WriteAllText(file, content);
        }
    }
}
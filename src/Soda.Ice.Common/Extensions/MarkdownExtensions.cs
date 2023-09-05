using Markdig;

namespace Soda.Ice.Common;

public static class MarkdownExtensions
{
    public static string GetContent(this string markdown)
    {
        var doc = Markdown.Parse(markdown);
        foreach (var block in doc.AsEnumerable())
        {
            if (block is Markdig.Syntax.ParagraphBlock)
            {
                Console.WriteLine(block.ToString());
            }
        }

        return string.Join("", doc.AsEnumerable().Where(x=>x is Markdig.Syntax.ParagraphBlock).Select(x=>x.ToString()));
    }
}

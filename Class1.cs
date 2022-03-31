using System.Text.RegularExpressions;
using Bbob.Plugin;

namespace bbob_plugin_mathjax;
public class Class1 : IPlugin
{
    public void GenerateCommand(string filePath, GenerationStage stage)
    {
        if (stage != GenerationStage.FinalProcess) return;

        string source = "<script>MathJax.typeset()</script>";
        PluginHelper.modifyRegisteredObject<dynamic>("article", (ref dynamic? article)=>{
            if (article != null)
            {
                article.contentParsed += source;
            }
        });
    }
    public Action? CommandComplete(Commands cmd)
    {
        if (cmd != Commands.GenerateCommand) return null;
        string indexFile = Path.Combine(PluginHelper.DistributionDirectory, "index.html");
        string indexPlain = File.ReadAllText(indexFile);
        string mathJaxOption = "<script>MathJax={options:{processHtmlClass:'math',ignoreHtmlClass:'.*'}};</script>";
        string src = "<script type=\"text/javascript\" id=\"MathJax-script\" async src=\"https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-mml-chtml.js\"></script>";
        string patternHead = @"<head>(.*)</head>";
        string replacement1 = $"<head>{src}{mathJaxOption}$1</head>";
        indexPlain = Regex.Replace(indexPlain, patternHead, replacement1, RegexOptions.Singleline);
        File.WriteAllText(indexFile, indexPlain);
        return null;
    }
}

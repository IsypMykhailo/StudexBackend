using System.Text;
using PdfSharp.Pdf.IO;

namespace Studex.Services;

public class PdfExtractor
{
    public static string ExtractText(string filePath)
    {
        using var pdf = PdfReader.Open(filePath, PdfDocumentOpenMode.ReadOnly);
        var text = new StringBuilder();
        foreach (var page in pdf.Pages)
        {
            text.Append(page.Contents.ToString());
        }
        return text.ToString();
    }
}
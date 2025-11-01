using less2;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;

internal class HtmlSerializer
{
    public async Task<string> Load(string url)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        return html;
    }

    public List<HtmlElement> ParseHtml(string html)
    {
        var cleanHtml = new Regex("\\s").Replace(html, "");
        var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);

        // המרת המחרוזות לאובייקטים מסוג HtmlElement
        var elements = new List<HtmlElement>();
        foreach (var line in htmlLines)
        {
            // כאן תצטרך לכתוב לוגיקה כדי להמיר את המחרוזות לאובייקטים של HtmlElement
            // לדוגמה:
            var element = new HtmlElement
            {
                // הגדר את התכונות של HtmlElement בהתאם למידע במחרוזת
                Name = line // דוגמה, תצטרך להתאים זאת
            };

            elements.Add(element);
        }

        return elements;
    }

    //public string Serialize(object obj)
    //{

    //    var htmlElement = "<div id=\"my-id\" class=\"my-class-1 my-class-2\" width=\"100%\">text</div>";

    //    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElement);
    //    return "<serialized-object></serialized-object>";
    //}
}
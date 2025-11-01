using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace less2
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public string[] AllHtmlTags { get; private set; }
        public string[] SelfClosingTags { get; private set; }
        public static HtmlHelper Instance => _instance;
        private HtmlHelper()
        {
            var allHtmlTagsJson = File.ReadAllText("ListHtmlTags/HtmlTags.json");
            AllHtmlTags = JsonSerializer.Deserialize<string[]>(allHtmlTagsJson);

            var selfClosingTagsJson = File.ReadAllText("ListHtmlTags/HtmlVoidTags.json");
            SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingTagsJson);
        }
    }
}
using less2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace less2
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector(string tagName, string id = null, List<string> classes = null)
        {
            TagName = tagName;
            Id = id;
            Classes = classes ?? new List<string>();
        }

        public static Selector FromQueryString(string queryString)
        {
            var levels = queryString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Selector root = null;
            Selector current = null;

            foreach (var level in levels)
            {
                var parts = level.Split(new[] { '#', '.' }, StringSplitOptions.RemoveEmptyEntries);
                string tagName = null;
                string id = null;
                var classes = new List<string>();

                for (int i = 0; i < parts.Length; i++)
                {
                    if (i == 0 && !level.StartsWith("#") && !level.StartsWith("."))
                    {
                        // בדוק אם השם הוא שם תקין של תגית HTML
                        if (IsValidHtmlTag(parts[i]))
                        {
                            tagName = parts[i];
                        }
                    }
                    else if (level.Contains("#") && parts[i].Equals(parts[i], StringComparison.OrdinalIgnoreCase))
                    {
                        id = parts[i];
                    }
                    else if (level.Contains("."))
                    {
                        classes.Add(parts[i]);
                    }
                }

                Selector newSelector = new Selector(tagName, id, classes);

                if (root == null)
                {
                    root = newSelector; // זהו השורש
                }
                else
                {
                    current.Child = newSelector; // הוספת הבן
                    newSelector.Parent = current; // עדכון האב
                }

                current = newSelector; // עדכון הסלקטור הנוכחי
            }

            return root;
        }
        private static bool IsValidHtmlTag(string tagName)
        {
            // השתמש ב- HtmlHelper כדי לבדוק אם התגית היא תקינה
            var htmlHelper = HtmlHelper.Instance;
            return Array.Exists(htmlHelper.AllHtmlTags, tag => tag.Equals(tagName, StringComparison.OrdinalIgnoreCase));
        }
    }
}

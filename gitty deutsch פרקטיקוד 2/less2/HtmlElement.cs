using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace less2
{

    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HtmlElement current = queue.Dequeue();
                yield return current;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this.Parent;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public IEnumerable<HtmlElement> FindElementsBySelector(Selector selector)
        {
            HashSet<HtmlElement> results = new HashSet<HtmlElement>(); // שימוש ב-HashSet
            FindElementsBySelectorRecursive(this, selector, results);
            return results;
        }

        private void FindElementsBySelectorRecursive(HtmlElement currentElement, Selector currentSelector, HashSet<HtmlElement> results)
        {
            // קבלת כל הצאצאים של האלמנט הנוכחי
            var descendants = currentElement.Descendants().ToList();

            // חיפוש ברשימת הצאצאים לפי קריטריונים של הסלקטור הנוכחי
            var filtered = descendants.Where(e => MatchesSelector(e, currentSelector)).ToList();

            // אם הגעת לסלקטור האחרון, הוספת את הרשימה המסוננת לתוצאות
            if (currentSelector.Child == null) // תנאי עצירה
            {
                foreach (var element in filtered)
                {
                    results.Add(element); // HashSet יטפל במניעת כפילויות
                }
            }
            else
            {
                // ריצה על הרשימה המסוננת עם הסלקטור הבא
                foreach (var element in filtered)
                {
                    FindElementsBySelectorRecursive(element, currentSelector.Child, results);
                }
            }
        }

        private bool MatchesSelector(HtmlElement element, Selector selector)
        {
            bool matches = true;

            if (!string.IsNullOrEmpty(selector.TagName) && element.Name != selector.TagName)
                matches = false;

            if (!string.IsNullOrEmpty(selector.Id) && element.Id != selector.Id)
                matches = false;

            if (selector.Classes.Any() && !selector.Classes.All(c => element.Classes.Contains(c)))
                matches = false;

            return matches;
        }
    }
}
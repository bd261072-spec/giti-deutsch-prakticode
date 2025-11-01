using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace less2
{
    internal class HtmlQueryService
    {
        public List<HtmlElement> Query(HtmlElement root, string selectorString)
        {
            // יצירת Selector מתוך המחרוזת
            var selector = Selector.FromQueryString(selectorString);
            var results = new List<HtmlElement> { root };

            // חיפוש לפי הסלקטור
            results = results.SelectMany(e => FindElements(e, selector)).ToList();

            return results.Distinct().ToList(); // הסרת כפילויות
        }

        private IEnumerable<HtmlElement> FindElements(HtmlElement element, Selector selector)
        {
            var matchingElements = new List<HtmlElement>();

            // חיפוש לפי סלקטור
            if (MatchesSelector(element, selector))
            {
                matchingElements.Add(element);
            }

            // חיפוש בילדים
            if (element.Children != null)
            {
                foreach (var child in element.Children)
                {
                    matchingElements.AddRange(FindElements(child, selector));
                }
            }

            return matchingElements;
        }

        private bool MatchesSelector(HtmlElement element, Selector selector)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            bool matches = true;

            if (!string.IsNullOrEmpty(selector.TagName) && !element.Name.Equals(selector.TagName, StringComparison.OrdinalIgnoreCase))
                matches = false;

            if (!string.IsNullOrEmpty(selector.Id))
            {
                if (element.Id == null) // בדוק אם Id הוא null
                    return false;
                if (!element.Id.Equals(selector.Id, StringComparison.OrdinalIgnoreCase))
                    matches = false;
            }

            // בדוק אם Classes לא הוא null לפני הבדיקה
            if (selector.Classes != null && selector.Classes.Any())
            {
                if (element.Classes == null || !selector.Classes.All(c => element.Classes.Contains(c)))
                    matches = false;
            }

            return matches;
        }



    }

}

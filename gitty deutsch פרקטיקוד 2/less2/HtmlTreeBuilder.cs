using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace less2
{
    internal class HtmlTreeBuilder
    {
        public HtmlElement BuildTree(string[] htmlLines)
        {
            HtmlElement root = new HtmlElement { Name = "root" }; // אובייקט שורש
            HtmlElement currentElement = root; // אלמנט נוכחי
            Stack<HtmlElement> stack = new Stack<HtmlElement>(); // מחסנית לשמירת הורים

            foreach (var line in htmlLines)
            {
                var trimmedLine = line.Trim();
                if (trimmedLine.Equals("html/")) // סיום ה-html
                {
                    break;
                }

                var tagName = trimmedLine.Split(' ')[0].Trim('<', '>'); // קבל את שם התגית

                if (trimmedLine.StartsWith("/")) // תגית סוגרת
                {
                    if (stack.Count > 0)
                    {
                        currentElement = stack.Pop(); // חזור לאבא
                    }
                }
                else
                {
                    var newElement = new HtmlElement { Name = tagName, Parent = currentElement }; // צור אלמנט חדש
                    currentElement.Children.Add(newElement); // הוסף את האלמנט החדש לרשימת הילדים

                    // פרוק את המחרוזת להמשך
                    var attributesString = trimmedLine.Substring(tagName.Length).Trim();
                    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(attributesString);

                    foreach (Match attribute in attributes)
                    {
                        var attrName = attribute.Groups[1].Value;
                        var attrValue = attribute.Groups[2].Value;

                        if (attrName.Equals("id", StringComparison.OrdinalIgnoreCase))
                        {
                            newElement.Id = attrValue;
                        }
                        else if (attrName.Equals("class", StringComparison.OrdinalIgnoreCase))
                        {
                            newElement.Classes.AddRange(attrValue.Split(' ')); // פרוק את ה-Class לחלקים
                        }

                        newElement.Attributes.Add(attrName + "=\"" + attrValue + "\"");
                    }

                    // בדוק אם התגית סוגרת את עצמה
                    bool isSelfClosing = attributesString.EndsWith("/") ||
                        HtmlHelper.Instance.SelfClosingTags.Contains(tagName);

                    if (!isSelfClosing)
                    {
                        stack.Push(currentElement); // שמור את האלמנט הנוכחי במחסנית
                        currentElement = newElement; // עדכן את האלמנט הנוכחי
                    }
                    else
                    {
                        // אם התגית סוגרת את עצמה, לא מעדכנים את currentElement
                    }
                }
            }

            return root; // החזר את עץ ה-html
        }
    }
}

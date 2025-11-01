//BSD

using less2;
using System.Threading.Channels;

//static async Task Main(string[] args)
//{
//HtmlSerializer serializer = new HtmlSerializer();
//string url = "https://malkabruk.co.il/";
//string html = await serializer.Load(url);//מטעין את כל הHTML
//List<string> tags = serializer.ParseHtml(html);

//}



using less2;
using System.Threading.Tasks; // ודא שאתה כולל את המרחב הנדרש

// יצירת מופע של HtmlQueryService
HtmlQueryService queryService = new HtmlQueryService();

HtmlSerializer serializer = new HtmlSerializer();
var html = await serializer.Load("https://malkabruk.co.il/");
var domElements = serializer.ParseHtml(html); // הנח שזה מחזיר List<HtmlElement>

foreach (var root in domElements)
{
    var result = queryService.Query(root, "div.site-button"); // השתמש במופע של queryService
    result.ForEach(e => Console.WriteLine(e.ToString()));
}

Console.ReadLine();





//var root = new HtmlElement { Name = "div", Id = "root" };
//var child1 = new HtmlElement { Name = "div", Id = "mydiv", Classes = new List<string> { "class-name" } };
//var child2 = new HtmlElement { Name = "span", Classes = new List<string> { "class-name" } };
//root.Children.Add(child1);
//child1.Children.Add(child2);
//var queryService = new HtmlQueryService();
//var results = queryService.Query(root, "div#mydiv");



//foreach (var result in results)
//{
//    Console.WriteLine(result.Name);
//}


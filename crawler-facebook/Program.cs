using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Text;

namespace SeleniumDocs.GettingStarted
{
    public class Program
    {
        static ConsoleManager _consoleManager = ConsoleManager.GetInstance();
        static void Main()
        {
            _consoleManager.ReadCommand();
            Console.ReadKey();
        }
    }

    public class Person
    {
        private string _login = "root";
        private string _password = "root";
        
        public string Login { get => _login; set => _login = value; }
        public string Password
        {
            get { return _password; } set { if (value != "") _password = value; }
        }
    }

    public sealed class ConsoleManager
    {
        public List<String> subscrition = new List<String>();
        public Engine crawler = new Engine();

        private Person _Person = new Person();
        private ConsoleManager() { }
        private static ConsoleManager _instance;

        public static ConsoleManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ConsoleManager();
            }
            return _instance;
        }
        public void ReadCommand()
        {
            Console.WriteLine("Insert Command");
            string answer = Console.ReadLine() ?? "";
            switch (answer)
            {
                case "autho":
                    Authorization();
                    break;
                case "sub":
                    Subscribe();
                    break;
                case "show":
                    ShowSubs();
                    break;
                case "crawl":
                    crawler.Crawl(subscrition);
                    break;
                default: Console.WriteLine("Not command"); break;
            }
            ReadCommand();
        }
        private void Authorization() // for future 
        {
            Console.WriteLine("<Login>");
            _Person.Login = Console.ReadLine() ?? "";
            Console.WriteLine("<Password>");
            _Person.Password = Console.ReadLine() ?? ""; 
        }
        private void Subscribe()
        {
            Console.WriteLine("Write id group");
            subscrition.Add("https://www.facebook.com/groups/" + Console.ReadLine());
        }
        private void ShowSubs()
        {
            foreach (var item in subscrition)
            {
                Console.WriteLine(item);
            }
        }
    }

    public class Engine : ICrawalbe
    { // 439630546077439 <= public group id, which works
        public void Crawl(List<String> subscrition)
        {
            EdgeSession(subscrition);
        }
        private void EdgeSession(List<String> subscrition)
        {
            var options = new EdgeOptions();
            options.AddArgument("--lang=en");
            var driver = new EdgeDriver(options);

            if(subscrition.Count < 0) { Console.WriteLine("NO SUBS. Please subscribe to crawl");return; }
            while (true)
            { 
                for (int i = 0; i < subscrition.Count; i++) 
                {
                driver.Navigate().GoToUrl(subscrition[i]);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(3000);
                var fbOffer = driver.FindElement(By.XPath("//div[@class='x1yztbdb x1n2onr6 xh8yej3 x1ja2u2z']")); // find latest offer
                var name = fbOffer.FindElement(By.XPath(".//div[contains(@class, 'x1ok221b')]"));
                var text = fbOffer.FindElement(By.XPath(".//div[@dir='auto']")); 
                var price_location = fbOffer.FindElement(By.XPath(".//span[contains(@class, 'xtvhhri')]"));
                var title = fbOffer.FindElement(By.XPath(".//span[@class='x1lliihq x6ikm8r x10wlt62 x1n2onr6 x1j85h84']"));
                List<String> images = GetImg(fbOffer.FindElements(By.XPath(".//img[contains(@class, 'x1ey2m1c')]")));

                Console.WriteLine(ToUTF8(name.Text));
                Console.WriteLine(ToUTF8(text.Text));
                Console.WriteLine(ToUTF8(price_location.Text));
                Console.WriteLine(ToUTF8(title.Text));
                for (i = 0; i < images.Count; i++)
                { 
                    Console.Write(images[i] + " \n" ); }
                }
                if (Console.ReadKey().Key != ConsoleKey.Enter) return;
                Thread.Sleep(100000);
            }

        }
        private string ToUTF8( string text)
        {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));
        }
        private List<String> GetImg(IReadOnlyList<IWebElement> sources) 
        {
            List<String> img = new List<String>();
            for(int i = 0;i< sources.Count; i++) 
            {
                img.Add(sources[i].GetAttribute("src").ToString()); 
            }
            return img;
        }
    }
    interface ICrawalbe 
    {
         void Crawl(List<String> subscrition);
    }

    public class Offer  // for future 
    {
        public string NameOfferer { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        private int Price { get; set; }
        public string Location { get; set; }
        private List<String> _linkimages = new List<String>();
        public Offer(string name, string title,string text,int price,string location) { }
    }
}
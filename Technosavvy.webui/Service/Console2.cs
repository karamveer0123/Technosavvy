using TechnoApp.Ext.Web.UI.Service;

namespace TechnoApp.Ext.Web.UI
{
    public static class Console2
    {
        public static void WriteLine_RED(string line)
        {
            APIHub.PublishLog($"ERROR:{line}");
            var fc1 = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(line);
            Console.ForegroundColor = fc1;
        }
        public static void WriteLine_White(string line)
        {
            APIHub.PublishLog(line);
            var fc1 = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(line);
            Console.ForegroundColor = fc1;
        }
        public static void WriteLine_DarkYellow(string line)
        {
            APIHub.PublishLog(line);
            var fc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(line);
            Console.ForegroundColor = fc;
        }
        public static void WriteLine_Green(string line)
        {
            APIHub.PublishLog(line);
            var fc2 = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(line);
            Console.ForegroundColor = fc2;
        }
    }
}

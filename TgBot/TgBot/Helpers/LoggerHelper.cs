namespace TgBot.Helpers
{
    public static class LoggerHelper
    {
        public static void WriteLine(string text)
        {
            string readText = File.ReadAllText("log.txt");
            using (StreamWriter writer = new StreamWriter("log.txt"))
            {
                writer.Write(readText);
                writer.WriteLine(text);
            }
        }
    }
}

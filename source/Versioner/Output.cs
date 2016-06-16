using System;

namespace Versioner
{
    public interface IOutput
    {
        void Write(string text);
    }

    public class ConsoleOutput : IOutput
    {
        public void Write(string text)
        {
            Console.Write(text);
        }
    }
}

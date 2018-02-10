using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCore.Tools
{
    class Logger
    {

        public enum LogType
        {
            INFO = 1,
            WARNING,
            ERROR,
            DEBUG
        };

        public static void Info(String Message, params object[] args)
        {
            WriteMessage(LogType.INFO, ConsoleColor.White, Message, args);
        }

        public static void Warning(String Message, params object[] args)
        {
            WriteMessage(LogType.WARNING, ConsoleColor.Yellow, Message, args);
        }

        public static void Error(String Message, params object[] args)
        {
            WriteMessage(LogType.ERROR, ConsoleColor.Red, Message, args);
        }

        public static void Debug(String Message, params object[] args)
        {
            WriteMessage(LogType.DEBUG, ConsoleColor.Cyan, Message, args);
        }

        private static void WriteMessage(LogType Tag, ConsoleColor color, String Message, params object[] args)
        {
            switch(Tag)
            {
                case LogType.INFO:
                    color = ConsoleColor.White;
                    break;
                case LogType.WARNING:
                    color = ConsoleColor.Yellow;
                    break;
                case LogType.ERROR:
                    color = ConsoleColor.Red;
                    break;
                case LogType.DEBUG:
                    color = ConsoleColor.Cyan;
                    break;
            }
            ConsoleColor def = Console.ForegroundColor;
            Console.ForegroundColor = color;
            StringBuilder builder = new StringBuilder();
            builder.Append("[").Append(Tag.ToString()).Append("]\t").Append(Message);
            if (args.Length > 0)
                Console.WriteLine(builder.ToString(), args);
            else
                Console.WriteLine(builder.ToString());
           Console.ForegroundColor = def;
        }

    }
}

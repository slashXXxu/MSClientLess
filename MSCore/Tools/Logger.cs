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
            WriteMessage(LogType.INFO, Message, args);
        }

        public static void Warning(String Message, params object[] args)
        {
            WriteMessage(LogType.WARNING, Message, args);
        }

        public static void Error(String Message, params object[] args)
        {
            WriteMessage(LogType.ERROR, Message, args);
        }

        public static void Debug(String Message, params object[] args)
        {
            WriteMessage(LogType.DEBUG, Message, args);
        }

        private static void WriteMessage(LogType Tag, String Message, params object[] args)
        {
            ConsoleColor textColor = ConsoleColor.White;
            switch(Tag)
            {
                case LogType.INFO:
                    textColor = ConsoleColor.White;
                    break;
                case LogType.WARNING:
                    textColor = ConsoleColor.Yellow;
                    break;
                case LogType.ERROR:
                    textColor = ConsoleColor.Red;
                    break;
                case LogType.DEBUG:
                    textColor = ConsoleColor.Cyan;
                    break;
            }
            ConsoleColor def = Console.ForegroundColor;
            Console.ForegroundColor = textColor;
            StringBuilder builder = new StringBuilder();
            builder.Append("[").Append(Tag.ToString()).Append("]\t").Append(Message);
            Console.WriteLine(builder.ToString(), args);
            Console.ForegroundColor = def;
        }
    }
}

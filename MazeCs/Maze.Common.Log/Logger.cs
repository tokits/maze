using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Log
{
    public enum LogLevel
    {
        None = 0,
        Debug,
        Info,
        Warning,
        Fatal
    }

    public static class Logger
    {
        #region Fields
        /// <summary>
        /// 多重起動チェックMutex
        /// </summary>
        static Mutex lockMutex;

        const string mutextString = "Common.Log.Logger";

        static readonly Encoding defaultEncoding;

        static readonly string logFilePath;
        #endregion

        static Logger()
        {
            lockMutex = new Mutex(false, mutextString);

            defaultEncoding = Encoding.GetEncoding("shift_jis");

            logFilePath = string.Format(".\\Maze_{0}.log",  DateTime.Now.ToString("yyyyMMdd"));
        }

        public static void LogOutput(LogLevel logLevel, string message)
        {
#if false
            try
            {
                lockMutex.WaitOne();

                // 追記・SJISでファイルを開く
                using (StreamWriter writer = new StreamWriter(logFilePath, true, defaultEncoding))
                    writer.WriteLine(CreateLogMesssage(logLevel, message));
            }
            catch
            {
                // ログ出力出来ないときは、例外をスローしない
            }
            finally
            {
                lockMutex.ReleaseMutex();
            }
#endif
        }

        public static void LogOutput(Exception ex, LogLevel logLevel = LogLevel.Fatal)
        {
            try
            {
                lockMutex.WaitOne();

                LogOutputWithExceptionInternal("例外発生！", ex, logLevel);
            }
            finally
            {
                lockMutex.ReleaseMutex();
            }
        }

        static void LogOutputWithExceptionInternal(string message, Exception ex, LogLevel logLevel)
        {
            try
            {
                // 追記・SJISでファイルを開く
                using (StreamWriter writer = new StreamWriter(logFilePath, true, defaultEncoding))
                {
                    writer.WriteLine(CreateLogMesssage(logLevel, message));
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.Source);
                    writer.WriteLine(ex.StackTrace);
                    writer.WriteLine(ex.TargetSite);
                }
            }
            catch
            {
                // ログ出力出来ないときは、例外をスローしない
            }
        }

        static string CreateLogMesssage(LogLevel logLevel, string message)
        {
            DateTime now = DateTime.Now;

            StackTrace stackTrace = new StackTrace();
            MethodBase methodBase = null;
            foreach (StackFrame frame in stackTrace.GetFrames())
            {
                if (frame.GetMethod().ReflectedType.Namespace == "Common.Log")
                    continue;
                else
                {
                    methodBase = frame.GetMethod();
                    break;
                }
            }

            string methodName = string.Format("{0}.{1}", methodBase.ReflectedType.Name, methodBase.Name);


            // 例
            // [Debug] '2013/08/20,18:28:19.894, xxxxxxxxxxxxxxxx
            return string.Format(
                "[{0}]\t\'{1}\t{2}\t{3}",
                logLevel.ToString(),
                now.ToString("yyyy/MM/dd HH:mm:ss.fff"),
                methodName,
                message);
        }
    }
}

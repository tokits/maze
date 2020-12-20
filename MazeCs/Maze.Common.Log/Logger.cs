using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

/// <summary>
/// ログ出力のnamespace
/// 全体的に未完成
/// </summary>
namespace Common.Log
{
    /// <summary>
    /// ログレベル
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 出力なし
        /// </summary>
        None = 0,
        /// <summary>
        /// デバッグ出力
        /// </summary>
        Debug,
        /// <summary>
        /// 詳細情報
        /// </summary>
        Info,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        /// <summary>
        /// 警告
        /// </summary>
        Fatal
    }

    /// <summary>
    /// ログ出力クラス
    /// </summary>
    public static class Logger
    {
        #region Fields
        /// <summary>
        /// 多重起動チェックMutex
        /// </summary>
        static Mutex lockMutex;

        /// <summary>
        /// Mutexにつける名前(識別子？)
        /// </summary>
        const string mutextString = "Common.Log.Logger";

        /// <summary>
        /// ログ文字列のエンコーディング
        /// </summary>
        static readonly Encoding defaultEncoding;

        /// <summary>
        /// ログファイルパス
        /// </summary>
        static readonly string logFilePath;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static Logger()
        {
            lockMutex = new Mutex(false, mutextString);

            defaultEncoding = Encoding.GetEncoding("shift_jis");

            logFilePath = string.Format(".\\Maze_{0}.log",  DateTime.Now.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <param name="logLevel">ログレベル</param>
        /// <param name="message">ログメッセージ</param>
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

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <param name="ex">例外オブジェクト</param>
        /// <param name="logLevel">ログレベル</param>
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

        /// <summary>
        /// ログ出力（内部メソッド）
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="ex">例外オブジェクト</param>
        /// <param name="logLevel">ログレベル</param>
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

        /// <summary>
        /// ログメッセージ生成（内部メソッド）
        /// </summary>
        /// <param name="logLevel">ログレベル</param>
        /// <param name="message">ログメッセージ</param>
        /// <returns></returns>
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

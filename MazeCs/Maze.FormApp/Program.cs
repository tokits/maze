using System;
using System.Windows.Forms;

/// <summary>
/// 迷路プログラムのnamespace
/// </summary>
namespace Maze.FormApp
{
    /// <summary>
    /// アプリケーションのメインクラス
    /// </summary>
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MazeForm());
        }
    }
}

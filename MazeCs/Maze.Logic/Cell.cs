/// <summary>
/// 迷路プログラムのnamespace
/// </summary>
namespace Maze.Logic
{
    /// <summary>
    /// セルの状態
    /// </summary>
    public enum CellStatus
    {
        /// <summary>
        /// 迷路の周りの壁
        /// </summary>
        Wall = 0,
        /// <summary>
        /// ブロック（通過不可）
        /// </summary>
        Block,
        /// <summary>
        /// 開始のセル
        /// </summary>
        Start,
        /// <summary>
        /// ゴールのセル
        /// </summary>
        End,
        /// <summary>
        /// 通過可能セル
        /// </summary>
        Path,
        /// <summary>
        /// 迷路の回答のセル
        /// </summary>
        Answer,
        /// <summary>
        /// 迷路の回答とならないセル
        /// </summary>
        NotAnswer
    }

    /// <summary>
    /// セル（迷路のブックの意味）
    /// </summary>
    public struct Cell
    {
        /// <summary>
        /// CellStatusプロパティ
        /// </summary>
        public CellStatus CellStatus { get; set; }
    }
}

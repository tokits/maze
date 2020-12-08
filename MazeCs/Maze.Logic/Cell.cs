using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze.Logic
{
    public enum CellStatus
    {
        Wall = 0,
        Block,
        Start,
        End,
        Path,
        Answer,
        NotAnswer
    }

    public struct Cell
    {
        public CellStatus CellStatus { get; set; }
    }
}

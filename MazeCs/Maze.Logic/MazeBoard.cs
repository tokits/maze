//using Maze.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze.Logic
{
    public class MazeBoardException : Exception
    {
        MazeBoardException(string message)
            : base(message)
        {
        }

        MazeBoardException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }

    public enum Direction
    {
        North = 0,
        South,
        East,
        West,
        DirectionCount
    }

    public struct Coordinate
    {
        public int X;
        public int Y;

        static Coordinate()
        {
            Origin = new Coordinate(0, 0);
        }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Coordinate Origin;
    }

    public class MazeBoard
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Cell[][] Cells { get; private set; }

        public bool IsCompleted { get; private set; }
        public bool IsAnswerCompleted { get; private set; }

        Random Random { get; set; }

        public Stack<Coordinate> CoordinateStack { get; private set; }

        Queue<Coordinate> CoordinateQueue = new Queue<Coordinate>();

        // ロジックで全部出せない？
        Direction[][] RandomDirections = {
                                       new [] { (Direction)0, (Direction)1, (Direction)2, (Direction)3 },
                                       new [] { (Direction)0, (Direction)1, (Direction)3, (Direction)2 },
                                       new [] { (Direction)0, (Direction)2, (Direction)1, (Direction)3 },
                                       new [] { (Direction)0, (Direction)2, (Direction)3, (Direction)1 },
                                       new [] { (Direction)0, (Direction)3, (Direction)1, (Direction)2 },
                                       new [] { (Direction)0, (Direction)3, (Direction)2, (Direction)1 },

                                       new [] { (Direction)1, (Direction)0, (Direction)2, (Direction)3 },
                                       new [] { (Direction)1, (Direction)0, (Direction)3, (Direction)2 },
                                       new [] { (Direction)1, (Direction)2, (Direction)0, (Direction)3 },
                                       new [] { (Direction)1, (Direction)2, (Direction)3, (Direction)0 },
                                       new [] { (Direction)1, (Direction)3, (Direction)0, (Direction)2 },
                                       new [] { (Direction)1, (Direction)3, (Direction)2, (Direction)0 },

                                       new [] { (Direction)2, (Direction)0, (Direction)1, (Direction)3 },
                                       new [] { (Direction)2, (Direction)0, (Direction)3, (Direction)1 },
                                       new [] { (Direction)2, (Direction)1, (Direction)0, (Direction)3 },
                                       new [] { (Direction)2, (Direction)1, (Direction)3, (Direction)0 },
                                       new [] { (Direction)2, (Direction)3, (Direction)0, (Direction)1 },
                                       new [] { (Direction)2, (Direction)3, (Direction)1, (Direction)0 },

                                       new [] { (Direction)3, (Direction)0, (Direction)1, (Direction)2 },
                                       new [] { (Direction)3, (Direction)0, (Direction)2, (Direction)1 },
                                       new [] { (Direction)3, (Direction)1, (Direction)0, (Direction)2 },
                                       new [] { (Direction)3, (Direction)1, (Direction)2, (Direction)0 },
                                       new [] { (Direction)3, (Direction)2, (Direction)0, (Direction)1 },
                                       new [] { (Direction)3, (Direction)2, (Direction)1, (Direction)0 },
                                   };

        public MazeBoard(int width, int height)
        {
            Width = width;
            Height = height;
            IsCompleted = false;
            IsAnswerCompleted = false;

            CoordinateStack = new Stack<Coordinate>();

            Cells = new Cell[Width][];
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = new Cell[Height];
                for (int j = 0; j < Cells[i].Length; j++)
                {
                    if (i == 0 || i == Width - 1 || j == 0 || j == Height - 1)
                        Cells[i][j].CellStatus = CellStatus.Wall;
                    else
                        Cells[i][j].CellStatus = CellStatus.Block;
                }
            }

            Cells[1][0].CellStatus = CellStatus.Start;
            //Cells[1][1].CellStatus = CellStatus.Path;
            //Cells[1][2].CellStatus = CellStatus.Block;
            Cells[Width - 2][Height - 1].CellStatus = CellStatus.End;

            Random = new Random((int)DateTime.Now.Ticks);
        }

        Direction[] GetDrectionNextSet()
        {
#if false
            Direction[] dir = new Direction[(int)Direction.DirectionCount];

            do
            {
                for (int i = 0; i < (int)Direction.DirectionCount; i++)
                {
                    dir[i] = (Direction)(Random.Next() % (int)Direction.DirectionCount);
                }
            } while (dir[0] == dir[1] || dir[1] == dir[2] || dir[2] == dir[3] || dir[3] == dir[0]
                 || dir[0] == dir[2] || dir[1] == dir[3]);

            return dir;
#else
            return RandomDirections[Random.Next() % 24];
#endif
        }

        bool IsValidCell(Coordinate coordinate, Direction dir)
        {
            int x = coordinate.X;
            int y = coordinate.Y;
            if (x < 0 || x > Width - 1)
                return false;
            if (y < 0 || y > Height - 1)
                return false;

            //if (Cells[x][y].CellStatus != CellStatus.Block)
            //    return false;

            // 元の座標を除く、回りのCellが既にPathだったら、False
            //switch (dir)
            //{
            //    case Direction.North:
            //        if ((Cells[x - 1][y].CellStatus != CellStatus.Block
            //            && Cells[x - 1][y].CellStatus != CellStatus.Wall)
            //            || (Cells[x + 1][y].CellStatus != CellStatus.Block
            //            && Cells[x + 1][y].CellStatus != CellStatus.Wall)
            //            || (Cells[x][y - 1].CellStatus != CellStatus.Block
            //            && Cells[x][y - 1].CellStatus != CellStatus.Wall))
            //            return false;
            //        break;
            //    case Direction.South:
            //        if ((Cells[x - 1][y].CellStatus != CellStatus.Block
            //            && Cells[x - 1][y].CellStatus != CellStatus.Wall)
            //            || (Cells[x + 1][y].CellStatus != CellStatus.Block
            //            && Cells[x + 1][y].CellStatus != CellStatus.Wall)
            //            || (Cells[x][y + 1].CellStatus != CellStatus.Block
            //            && Cells[x][y + 1].CellStatus != CellStatus.Wall))
            //            return false;
            //        break;
            //    case Direction.East:
            //        if ((Cells[x][y - 1].CellStatus != CellStatus.Block
            //            && Cells[x][y - 1].CellStatus != CellStatus.Wall)
            //            || (Cells[x][y + 1].CellStatus != CellStatus.Block
            //            && Cells[x][y + 1].CellStatus != CellStatus.Wall)
            //            || (Cells[x + 1][y].CellStatus != CellStatus.Block
            //            && Cells[x + 1][y].CellStatus != CellStatus.Wall))
            //            return false;
            //        break;
            //    case Direction.West:
            //        if ((Cells[x][y - 1].CellStatus != CellStatus.Block
            //            && Cells[x][y - 1].CellStatus != CellStatus.Wall)
            //            || (Cells[x][y + 1].CellStatus != CellStatus.Block
            //            && Cells[x][y + 1].CellStatus != CellStatus.Wall)
            //            || (Cells[x - 1][y].CellStatus != CellStatus.Block
            //            && Cells[x - 1][y].CellStatus != CellStatus.Wall))
            //            return false;
            //        break;
            //}
            switch (dir)
            {
                case Direction.North:
                    if ((x > 0 && Cells[x - 1][y].CellStatus == CellStatus.Path)
                     || (x < Width - 1 && Cells[x + 1][y].CellStatus == CellStatus.Path)
                     || (y > 0 && Cells[x][y - 1].CellStatus == CellStatus.Path))
                        return false;
                    break;
                case Direction.South:
                    if ((x > 0 && Cells[x - 1][y].CellStatus == CellStatus.Path)
                        || (x < Width - 1 && Cells[x + 1][y].CellStatus == CellStatus.Path)
                        || (y < Height - 1 && Cells[x][y + 1].CellStatus == CellStatus.Path))
                        return false;
                    break;
                case Direction.East:
                    if ((y > 0 && Cells[x][y - 1].CellStatus == CellStatus.Path)
                        || (y < Height - 1 && Cells[x][y + 1].CellStatus == CellStatus.Path)
                        || (x < Width -1 && Cells[x + 1][y].CellStatus == CellStatus.Path))
                        return false;
                    break;
                case Direction.West:
                    if ((y > 0 && Cells[x][y - 1].CellStatus == CellStatus.Path)
                        || (y < Height - 1 && Cells[x][y + 1].CellStatus == CellStatus.Path)
                        || (x > 0 && Cells[x - 1][y].CellStatus == CellStatus.Path))
                        return false;
                    break;
            }

#if false
            if (
                   Cells[x - 1][y].CellStatus == CellStatus.Path
                || Cells[x + 1][y].CellStatus == CellStatus.Path
                || Cells[x][y - 1].CellStatus == CellStatus.Path
                || Cells[x][y + 1].CellStatus == CellStatus.Path)
                //|| Cells[x - 1][y].CellStatus == CellStatus.End
                //|| Cells[x + 1][y].CellStatus == CellStatus.End
                //|| Cells[x][y - 1].CellStatus == CellStatus.End
                //|| Cells[x][y + 1].CellStatus == CellStatus.End)
                return false;
            //if ((Cells[x - 1][y].CellStatus != CellStatus.Block
            //    && Cells[x - 1][y].CellStatus != CellStatus.Wall)
            //    || (Cells[x + 1][y].CellStatus != CellStatus.Block
            //    && Cells[x + 1][y].CellStatus != CellStatus.Wall)
            //    || (Cells[x][y - 1].CellStatus != CellStatus.Block
            //    && Cells[x][y - 1].CellStatus != CellStatus.Wall)
            //    || (Cells[x][y + 1].CellStatus != CellStatus.Block
            //    && Cells[x][y + 1].CellStatus != CellStatus.Wall))
            //    return false;
#endif
            return (Cells[x][y].CellStatus == CellStatus.Block || Cells[x][y].CellStatus == CellStatus.End);
        }

        bool IsAnswerValidCell(Coordinate coordinate, Direction dir)
        {
            int x = coordinate.X;
            int y = coordinate.Y;
            if (x < 0 || x > Width - 1)
                return false;
            if (y < 0 || y > Height - 1)
                return false;

            return Cells[x][y].CellStatus == CellStatus.Path || Cells[x][y].CellStatus == CellStatus.End;
        }

        bool IsEndCell(Coordinate coordinate)
        {
            int x = coordinate.X;
            int y = coordinate.Y;

            if (x < 0 || x >= Width)
                return false;
            if (y < 0 || y >= Height)
                return false;

            return (Cells[x][y].CellStatus == CellStatus.End);
        }

        void InnerCreate(Coordinate coordinate)
        {
            try
            {
                //Logger.LogOutput(LogLevel.Debug, "Enter");

                int x = coordinate.X;
                int y = coordinate.Y;

                if (IsEndCell(coordinate))
                {
                    IsCompleted = true;
                    return;
                }

                Coordinate newCord = Coordinate.Origin;
                Direction[] dirs = GetDrectionNextSet();
                Coordinate firstCord = Coordinate.Origin;
                //Queue<Coordinate> queue = new Queue<Coordinate>();
                for (int i = 0; i < (int)Direction.DirectionCount; i++)
                {
                    switch (dirs[i])
                    {
                        case Direction.North:
                            newCord = new Coordinate(x, y - 1);
                            break;
                        case Direction.South:
                            newCord = new Coordinate(x, y + 1);
                            break;
                        case Direction.East:
                            newCord = new Coordinate(x + 1, y);
                            break;
                        case Direction.West:
                            newCord = new Coordinate(x - 1, y);
                            break;
                    }
                    if (IsValidCell(newCord, dirs[i]))
                    {
                        CoordinateQueue.Enqueue(newCord);
                        //Logger.LogOutput(LogLevel.Debug, string.Format("({0}, {1})", newCord.X, newCord.Y));
                        //CoordinateStack.Push(newCord);
                    }
                }
                if (CoordinateQueue.Count <= 0) return;
                //if (CoordinateStack.Count <= 0) return;
                while (CoordinateQueue.Count > 0)
                {
                    Coordinate temp = CoordinateQueue.Dequeue();
                    //Logger.LogOutput(LogLevel.Debug, string.Format("({0}, {1})", temp.X, temp.Y));
                    CoordinateStack.Push(temp);
                }
                if (Cells[x][y].CellStatus == CellStatus.Block)
                {
                    Cells[x][y].CellStatus = CellStatus.Path;
                }
            }
            finally
            {
                //Logger.LogOutput(LogLevel.Debug, "Leave");
            }
        }

        void InnerAnswer(Coordinate coordinate)
        {
            try
            {
                //Logger.LogOutput(LogLevel.Debug, "Enter");

                int x = coordinate.X;
                int y = coordinate.Y;

                if (IsEndCell(coordinate))
                {
                    IsAnswerCompleted = true;
                    return;
                }

                if (Cells[x][y].CellStatus == CellStatus.Path)
                {
                    Cells[x][y].CellStatus = CellStatus.Answer;
                }
                else if (Cells[x][y].CellStatus == CellStatus.Answer)
                {
                    Cells[x][y].CellStatus = CellStatus.NotAnswer;
                    CoordinateStack.Pop();
                    return;
                }
                else if (Cells[x][y].CellStatus == CellStatus.NotAnswer)
                {
                    CoordinateStack.Pop(); 
                    return;
                }

                Coordinate newCord = Coordinate.Origin;
                Direction[] dirs = GetDrectionNextSet();
                Coordinate firstCord = Coordinate.Origin;
                //Queue<Coordinate> queue = new Queue<Coordinate>();
                for (int i = 0; i < (int)Direction.DirectionCount; i++)
                {
                    switch (dirs[i])
                    {
                        case Direction.North:
                            newCord = new Coordinate(x, y - 1);
                            break;
                        case Direction.South:
                            newCord = new Coordinate(x, y + 1);
                            break;
                        case Direction.East:
                            newCord = new Coordinate(x + 1, y);
                            break;
                        case Direction.West:
                            newCord = new Coordinate(x - 1, y);
                            break;
                    }
                    if (IsAnswerValidCell(newCord, dirs[i]))
                    {
                        CoordinateQueue.Enqueue(newCord);
                        //Logger.LogOutput(LogLevel.Debug, string.Format("({0}, {1})", newCord.X, newCord.Y));
                        //CoordinateStack.Push(newCord);
                    }
                }
                //if (CoordinateQueue.Count <= 0)
                if (CoordinateQueue.Count <= 0)
                {
                    Cells[x][y].CellStatus = CellStatus.NotAnswer;
                    CoordinateStack.Pop();
                    return;
                }
                while (CoordinateQueue.Count > 0)
                {
                    Coordinate temp = CoordinateQueue.Dequeue();
                    //Logger.LogOutput(LogLevel.Debug, string.Format("({0}, {1})", temp.X, temp.Y));
                    CoordinateStack.Push(temp);
                }

            }
            finally
            {
                //Logger.LogOutput(LogLevel.Debug, "Leave");
            }
        }

        public void CreationStart()
        {
            IsCompleted = false;
            CoordinateStack.Clear();
            CoordinateQueue.Clear();
            Coordinate cord = new Coordinate(1, 0);
            CoordinateStack.Push(cord);

            //while (CoordinateStack.Count > 0)
            //{
            //    cord = CoordinateStack.Pop();
            //    InnerCreateEx(cord);
            //}
        }

        public bool CreationNext()
        {
            if (CoordinateStack.Count > 0)
            {
                InnerCreate(CoordinateStack.Pop());
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AnswerStart()
        {
            IsAnswerCompleted = false;
            CoordinateStack.Clear();
            CoordinateQueue.Clear();
            Coordinate cord = new Coordinate(1, 0);
            CoordinateStack.Push(cord);
        }

        public bool AnswerNext()
        {
            if (IsAnswerCompleted == false && CoordinateStack.Count > 0)
            {
                InnerAnswer(CoordinateStack.Peek());
                return true;
            }
            else
            {
                return !IsAnswerCompleted;
            }
        }

        public void AnswerClear()
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                for (int j = 0; j < Cells[i].Length; j++)
                {
                    if (Cells[i][j].CellStatus == CellStatus.Answer
                        || Cells[i][j].CellStatus == CellStatus.NotAnswer)
                        Cells[i][j].CellStatus = CellStatus.Path;
                }
            }
        }
    }
}

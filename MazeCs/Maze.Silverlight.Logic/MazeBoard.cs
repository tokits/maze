using Maze.Silverlight.Logic;
//using Maze.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze.SilverLight.Logic
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
        DirectionCount,
        None
    }

    public class Coordinate
    {
        public int X;
        public int Y;
        public Direction Direction;

        static Coordinate()
        {
            Origin = new Coordinate(0, 0, Direction.None);
        }

        public Coordinate(int x, int y, Direction dir)
        {
            X = x;
            Y = y;
            Direction = dir;
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

        //Queue<Coordinate> CoordinateQueue = new Queue<Coordinate>();

        static readonly Direction[][] Directions;

        public Coordinate CurrentCoordinate { get; private set; }

        bool IsCompletedWithoutGoal = false;

        static MazeBoard()
        {
            Permutaions perm = new Permutaions((int)Direction.DirectionCount);
            Directions = new Direction[perm.Count][];
            foreach (var item in perm.Results.Select((v, i) => new { Value = v, Index = i }))
            {
                Directions[item.Index] = new Direction[item.Value.Length];
                for (int i = 0; i < item.Value.Length; i++)
                    Directions[item.Index][i] = (Direction)item.Value[i];
            }
        }

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
            Cells[Width - 2][Height - 1].CellStatus = CellStatus.End;

            Random = new Random((int)DateTime.Now.Ticks);
        }

        Direction[] GetDrectionNextSet()
        {
            return Directions[Random.Next() % 24];
        }

        bool IsNeighborhoodCell(Coordinate coordinate, Direction dir, CellStatus cellStatus)
        {
            int x = coordinate.X;
            int y = coordinate.Y;

            // 元の座標を除く、回りのCellが既にPathだったら、False
            switch (dir)
            {
                case Direction.North:
                    if ((x > 0 && (Cells[x - 1][y].CellStatus == cellStatus))
                     || (x < Width - 1 && (Cells[x + 1][y].CellStatus == cellStatus))
                     || (y > 0 && (Cells[x][y - 1].CellStatus == cellStatus)))
                        return true;
                    break;
                case Direction.South:
                    if ((x > 0 && (Cells[x - 1][y].CellStatus == cellStatus))
                        || (x < Width - 1 && (Cells[x + 1][y].CellStatus == cellStatus))
                        || (y < Height - 1 && (Cells[x][y + 1].CellStatus == cellStatus)))
                        return true;
                    break;
                case Direction.East:
                    if ((y > 0 && (Cells[x][y - 1].CellStatus == cellStatus))
                        || (y < Height - 1 && (Cells[x][y + 1].CellStatus == cellStatus))
                        || (x < Width - 1 && (Cells[x + 1][y].CellStatus == cellStatus)))
                        return true;
                    break;
                case Direction.West:
                    if ((y > 0 && (Cells[x][y - 1].CellStatus == cellStatus))
                        || (y < Height - 1 && (Cells[x][y + 1].CellStatus == cellStatus))
                        || (x > 0 && (Cells[x - 1][y].CellStatus == cellStatus)))
                        return true;
                    break;
            }
            return false;
        }

        void InnerCreate(Coordinate coordinate)
        {
            try
            {
                //Logger.LogOutput(LogLevel.Debug, "Enter");

                int x = coordinate.X;
                int y = coordinate.Y;

                CurrentCoordinate = null;

                if (x < 0 || x >= Width)
                    return;
                if (y < 0 || y >= Height)
                    return;

                switch (Cells[x][y].CellStatus)
                {
                    case CellStatus.End:
                        if (!IsCompletedWithoutGoal)
                        {
                            IsCompleted = true;
                            return;
                        }
                        break;
                    case CellStatus.Start:
                        // 継続
                        break;
                    case CellStatus.Wall:
                        CurrentCoordinate = null;
                        return;
                    case CellStatus.Path:
                        if (IsCompletedWithoutGoal)
                            IsCompleted = true;
                        return;
                    case CellStatus.Block:
                        if (!IsCompletedWithoutGoal)
                        {
                            if (IsNeighborhoodCell(coordinate, coordinate.Direction, CellStatus.Path) == false)
                            {
                                Cells[x][y].CellStatus = CellStatus.Path;
                                CurrentCoordinate = coordinate;
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            CurrentCoordinate = coordinate;
                            Cells[x][y].CellStatus = CellStatus.Path;
                        }
                        break;
                }

                Direction[] dirs = GetDrectionNextSet();
                for (int i = 0; i < (int)Direction.DirectionCount; i++)
                {
                    Coordinate newCord = null;
                    switch (dirs[i])
                    {
                        case Direction.North:
                            if (coordinate.Direction != Direction.South)
                                newCord = new Coordinate(x, y - 1, dirs[i]);
                            break;
                        case Direction.South:
                            if (coordinate.Direction != Direction.North)
                                newCord = new Coordinate(x, y + 1, dirs[i]);
                            break;
                        case Direction.East:
                            if (coordinate.Direction != Direction.West)
                                newCord = new Coordinate(x + 1, y, dirs[i]);
                            break;
                        case Direction.West:
                            if (coordinate.Direction != Direction.East)
                                newCord = new Coordinate(x - 1, y, dirs[i]);
                            break;
                    }
                    if (newCord != null)
                    {
                        //CoordinateQueue.Enqueue(newCord);
                        CoordinateStack.Push(newCord);
                    }
                }
                //while (CoordinateQueue.Count > 0)
                //{
                //    Coordinate temp = CoordinateQueue.Dequeue();
                //    CoordinateStack.Push(temp);
                //}
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

                CurrentCoordinate = null;

                int x = coordinate.X;
                int y = coordinate.Y;

                if (x < 0 || x > Width)
                {
                    CoordinateStack.Pop();
                    return;
                }
                if (y < 0 || y > Height)
                {
                    CoordinateStack.Pop();
                    return;
                }

                switch (Cells[x][y].CellStatus)
                {
                    case CellStatus.End:
                        IsAnswerCompleted = true;
                        CoordinateStack.Pop();
                        return;
                    case CellStatus.Start:
                        // 継続
                        break;
                    case CellStatus.Wall:
                        CoordinateStack.Pop();
                        return;
                    case CellStatus.Block:
                        CoordinateStack.Pop();
                        return;
                    case CellStatus.Path:
                        Cells[x][y].CellStatus = CellStatus.Answer;
                        CurrentCoordinate = coordinate;
                        break;
                    case CellStatus.Answer:
                    case CellStatus.NotAnswer:
                        Cells[x][y].CellStatus = CellStatus.NotAnswer;
                        CoordinateStack.Pop();
                        CurrentCoordinate = coordinate;
                        return;
                }

                Direction[] dirs = GetDrectionNextSet();
                for (int i = 0; i < (int)Direction.DirectionCount; i++)
                {
                    Coordinate newCord = null;
                    switch (dirs[i])
                    {
                        case Direction.North:
                            if (coordinate.Direction != Direction.South) 
                                newCord = new Coordinate(x, y - 1, dirs[i]);
                            break;
                        case Direction.South:
                            if (coordinate.Direction != Direction.North)
                                newCord = new Coordinate(x, y + 1, dirs[i]);
                            break;
                        case Direction.East:
                            if (coordinate.Direction != Direction.West)
                                newCord = new Coordinate(x + 1, y, dirs[i]);
                            break;
                        case Direction.West:
                            if (coordinate.Direction != Direction.East)
                                newCord = new Coordinate(x - 1, y, dirs[i]);
                            break;
                    }
                    if (newCord != null)
                    {
                        //CoordinateQueue.Enqueue(newCord);
                        CoordinateStack.Push(newCord);
                    }
                }
                //while (CoordinateQueue.Count > 0)
                //{
                //    Coordinate temp = CoordinateQueue.Dequeue();
                //    CoordinateStack.Push(temp);
                //}

            }
            finally
            {
                //Logger.LogOutput(LogLevel.Debug, "Leave");
            }
        }

        public void CreationStart()
        {
            IsCompleted = false;
            IsCompletedWithoutGoal = false;
            CurrentCoordinate = null;
            CoordinateStack.Clear();
            //CoordinateQueue.Clear();
            Coordinate cord = new Coordinate(1, 0, Direction.None);
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
                if (IsCompleted)
                    return false;
                else
                {
                    IsCompletedWithoutGoal = true;
                    Coordinate cord = new Coordinate(Width - 2, Height - 1, Direction.None);
                    CoordinateStack.Push(cord);
                    InnerCreate(CoordinateStack.Pop());
                    return !IsCompleted;
                }
            }
        }

        public void AnswerStart()
        {
            CurrentCoordinate = null;
            IsAnswerCompleted = false;
            CoordinateStack.Clear();
            //CoordinateQueue.Clear();
            Coordinate cord = new Coordinate(1, 0, Direction.None);
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
            CurrentCoordinate = null;
            IsAnswerCompleted = false;
        }
    }
}

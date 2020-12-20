using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 順列を求めるプログラムのnamespace
/// </summary>
namespace Permutaion.ConsoleApp
{
    /// <summary>
    /// アプリケーションのメインクラス
    /// </summary>
    static class Program
    {
        /// <summary>
        /// 順列生成（パターン①）
        /// </summary>
        class Permutaions1
        {
            class Pair
            {
                public List<int> Left;
                public List<int> Right;

                public Pair(List<int> left, List<int> right)
                {
                    Left = left;
                    Right = right;
                }
            }

            Stack<Pair> processingStack = new Stack<Pair>();
            List<List<int>> results = new List<List<int>>();

            public int Count { get { return results.Count; } }

            int size;

            public Permutaions1(int size)
            {
                this.size = size;

                List<int> left = new List<int>();
                List<int> right = new List<int>();

                for (int i = 0; i < size; i++)
                    right.Add(i + 1);

                processingStack.Push(new Pair(left, right));
                while (processingStack.Count > 0)
                {
                    var pair = processingStack.Pop();
                    Create(pair);
                }
                results.Reverse();
            }

            void Create(Pair pair)
            {
                if (pair.Right.Count <= 0)
                {
                    results.Add(pair.Left);
                    return;
                }
                for (int i = 0; i < pair.Right.Count; i++)
                {
                    List<int> left = new List<int>();
                    List<int> right = new List<int>();
                    left.AddRange(pair.Left);
                    left.Add(pair.Right[i]);
                    right.AddRange(pair.Right);
                    right.Remove(pair.Right[i]);
                    processingStack.Push(new Pair(left, right));
                }
            }

            public IEnumerator<IList<int>> GetEnumerator()
            {
                foreach (var item in results) yield return item;
            }
        }

        /// <summary>
        /// 順列生成（パターン②）
        /// </summary>
        class Permutaions2
        {
            int size;
            int[] indexArray;
            List<int[]> results = new List<int[]>();

            public int Count { get { return results.Count; } }

            public Permutaions2(int size)
            {
                this.size = size;
                indexArray = new int[this.size];

                for (int i = 0; i < this.size; i++)
                    indexArray[i] = i + 1;

                Permut(0);
            }

            void Permut(int currentIndex)
            {
                if (currentIndex < size - 1)
                {
                    for (int i = currentIndex; i < size; i++)
                    {
                        // シフト
                        int temp = indexArray[i];
                        for (int j = i; j > currentIndex; j--)
                            indexArray[j] = indexArray[j - 1];
                        indexArray[currentIndex] = temp;

                        Permut(currentIndex + 1);

                        for (int j = currentIndex; j < i; j++)
                            indexArray[j] = indexArray[j + 1];

                        indexArray[i] = temp;
                    }
                }
                else
                {
                    //foreach (var value in indexArray)
                    //    Console.Write("{0} ", value);
                    //Console.WriteLine();
                    results.Add((int [])indexArray.Clone());
                }
            }
            public IEnumerator<IList<int>> GetEnumerator()
            {
                foreach (var item in results) yield return item;
            }
        }

        /// <summary>
        /// 順列生成（パターン③）
        /// </summary>
        class Permutaions3
        {
            int size;
            int[] indexArray;

            //Stack<Pair> processingStack = new Stack<Pair>();
            public int Count { get { return results.Count; } }

            struct Position
            {
                public int Start;
                public int End;
            }

            Stack<Position> processingStack = new Stack<Position>();
            List<int[]> results = new List<int[]>();

            public Permutaions3(int size)
            {
                this.size = size;
                indexArray = new int[this.size];

                for (int i = 0; i < this.size; i++)
                    indexArray[i] = i + 1;

                processingStack.Push(new Position() { Start=0, End=0 });
                while (processingStack.Count > 0)
                {
                    Position pos = processingStack.Pop();
                    if (pos.Start >= size - 1)
                    {
                        results.Add(indexArray.ToArray());
                        Exchange(pos.Start, pos.End, true);
                    }
                    else
                    {
                        Exchange(pos.Start, pos.End);
                        //int start = 0;
                        //int end = 0;
                        //if (pos.End >= size - 1)
                        //{

                        //}
                        //else
                        //{
                        //}

                        processingStack.Push(new Position() { Start = pos.Start++, End = pos.Start });
                    }
                }
            }

            public IEnumerator<IList<int>> GetEnumerator()
            {
                foreach (var item in results) yield return item;
            }

            void Exchange(int pos1, int pos2, bool isReverse=false)
            {
                if (!isReverse)
                {
                    int temp = indexArray[pos1];
                    for (int i = pos1; i < pos2; i++)
                        indexArray[i] = indexArray[i + 1];
                    indexArray[pos2] = temp;
                }
                else
                {
                    int temp = indexArray[pos2];
                    for (int i = pos2; i > pos1; i--)
                        indexArray[i] = indexArray[i - 1];
                    indexArray[pos1] = temp;
                }
            }
        }

        /// <summary>
        /// 順列生成（パターン④）
        /// </summary>
        class Permutaions4
        {
            class Pair
            {
                public List<int> Left;
                public List<int> Right;

                public Pair(List<int> left, List<int> right)
                {
                    Left = left;
                    Right = right;
                }
            }

            Queue<Pair> processingQueue = new Queue<Pair>();
            List<List<int>> results = new List<List<int>>();

            public int Count { get { return results.Count; } }

            int size;

            public Permutaions4(int size)
            {
                this.size = size;

                List<int> left = new List<int>();
                List<int> right = new List<int>();

                for (int i = 0; i < size; i++)
                    right.Add(i + 1);

                processingQueue.Enqueue(new Pair(left, right));
                while (processingQueue.Count > 0)
                {
                    var pair = processingQueue.Dequeue();
                    Create(pair);
                }
                //results.Reverse();
            }

            void Create(Pair pair)
            {
                if (pair.Right.Count <= 0)
                {
                    results.Add(pair.Left);
                    return;
                }
                for (int i = 0; i < pair.Right.Count; i++)
                {
                    List<int> left = new List<int>();
                    List<int> right = new List<int>();
                    left.AddRange(pair.Left);
                    left.Add(pair.Right[i]);
                    right.AddRange(pair.Right);
                    right.Remove(pair.Right[i]);
                    processingQueue.Enqueue(new Pair(left, right));
                }
            }

            public IEnumerator<IList<int>> GetEnumerator()
            {
                foreach (var item in results) yield return item;
            }
        }

        /// <summary>
        /// Mainメソッド
        /// </summary>
        /// <param name="args">コマンドライン引数</param>
        static void Main(string[] args)
        {
#if false
            Permutaions1 perm = new Permutaions1(6);

            foreach (var item in perm)
            {
                foreach (var value in item)
                    Console.Write("{0} ", value);
                Console.WriteLine();
            }
#endif
            //var perm = new Permutaions3(4);
            var perm = new Permutaions4(4);
            Console.WriteLine(perm.Count);
            foreach (var item in perm)
            {
                foreach (var value in item)
                    Console.Write("{0} ", value);
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}

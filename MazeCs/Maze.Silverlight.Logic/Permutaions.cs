using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Maze.Silverlight.Logic
{
    public class Permutaions
    {
        int size;
        int[] indexArray;
        List<int[]> results = new List<int[]>();

        public List<int[]> Results { get { return results; } }
        public int Count { get { return results.Count; } }

        public Permutaions(int size)
        {
            this.size = size;
            indexArray = new int[this.size];

            for (int i = 0; i < this.size; i++)
                indexArray[i] = i;

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
                results.Add((int[])indexArray.Clone());
            }
        }

        public IEnumerator<IList<int>> GetEnumerator()
        {
            foreach (var item in results) yield return item;
        }
    }
}

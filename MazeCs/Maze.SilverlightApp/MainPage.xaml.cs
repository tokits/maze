using Maze.SilverLight.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Maze.SilverlightApp
{
    public partial class MainPage : UserControl
    {
        //const int MazeWidth = 40;
        //const int MazeHieght = 30;
        const int MazeWidth = 60;
        const int MazeHieght = 40;
        //const int MazeWidth = 100;
        //const int MazeHieght = 80;

        MazeBoard MazeBoard = new MazeBoard(MazeWidth, MazeHieght);

        //enum CellStatus
        //{
        //    Wall = 0,
        //    Block,
        //    Start,
        //    End,
        //    Path
        //    Answer,
        //    NotAnswer
        //}
        Color[] CellBrushes = {
                                  
            Colors.DarkGray,
            Colors.Brown,
            Colors.Red,
            Colors.Blue,
            Colors.White,
            Colors.Purple,
            Colors.White
                              };


        DispatcherTimer createMazeTimer = new DispatcherTimer();
        DispatcherTimer answerMazeTimer = new DispatcherTimer();

        Rectangle[][] mazeRects;

        public MainPage()
        {
            InitializeComponent();

            mazeCreateButton.Click += (s, e) =>
            {
                createMazeTimer.Stop();

                MazeBoard = new MazeBoard(MazeWidth, MazeHieght);
                UpdateMaze();

                MazeBoard.CreationStart();
                createMazeTimer.Start();
            };
            mazeClearButton.Click += (s, e) =>
            {
                createMazeTimer.Stop();
                answerMazeTimer.Stop();

                MazeBoard = new MazeBoard(MazeWidth, MazeHieght);
                MazeBoard.CreationStart();
                while (MazeBoard.CreationNext()) ;
                UpdateMaze();
            };
            answerStartButton.Click += (s, e) =>
            {
                if (MazeBoard.IsCompleted)
                {
                    createMazeTimer.Stop();
                    answerMazeTimer.Stop();
                    MazeBoard.AnswerClear();
                    UpdateMaze();

                    MazeBoard.AnswerStart();
                    answerMazeTimer.Start();
                }
            };
            answerClearButton.Click += (s, e) =>
            {
                answerMazeTimer.Stop();
                
                if (MazeBoard.IsCompleted)
                {
                    MazeBoard.AnswerClear();
                    UpdateMaze();
                }
            };
            answerCreateButton.Click += (s, e) =>
            {
                createMazeTimer.Stop();
                answerMazeTimer.Stop();

                if (MazeBoard.IsCompleted)
                {
                    MazeBoard.AnswerClear();
                    MazeBoard.AnswerStart();
                    while (MazeBoard.AnswerNext()) ;
                    UpdateMaze();
                }
            };

            createMazeTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            createMazeTimer.Tick += (s, e) =>
            {
                if (MazeBoard.CreationNext())
                {
                    if (MazeBoard.CurrentCoordinate != null)
                        UpdateCell(MazeBoard.CurrentCoordinate.X, MazeBoard.CurrentCoordinate.Y);
                }
                else
                {
                    createMazeTimer.Stop();
                    MessageBox.Show("Completed!");
                }
            };
            answerMazeTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            answerMazeTimer.Tick += (s, e) =>
            {
                if (MazeBoard.AnswerNext())
                {
                    if (MazeBoard.CurrentCoordinate != null)
                        UpdateCell(MazeBoard.CurrentCoordinate.X, MazeBoard.CurrentCoordinate.Y);
                }
                else
                {
                    answerMazeTimer.Stop();
                    MessageBox.Show("Answer Completed!");
                }
            };


            {

                double width = MazeCanvas.Width / MazeBoard.Width;
                double height = MazeCanvas.Height / MazeBoard.Height;

                mazeRects = new Rectangle[MazeWidth][];
                for (int i = 0; i < MazeWidth; i++)
                {
                    mazeRects[i] = new Rectangle[MazeHieght];
                    for (int j = 0; j < MazeHieght; j++)
                    {
                        var rect = new Rectangle();
                        rect.Width = width + 1;
                        rect.Height = height + 1;
                        rect.Fill = new SolidColorBrush(Colors.Orange);
                        //rect.StrokeThickness = 1;
                        //rect.Stroke = new SolidColorBrush(Colors.Blue);
                        //rect.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        //rect.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        Canvas.SetLeft(rect, i * width);
                        Canvas.SetTop(rect, j * height);
                        mazeRects[i][j] = rect;
                        MazeCanvas.Children.Add(mazeRects[i][j]);
                    }
                }
            };
            UpdateMaze();
            //Rectangle r = new Rectangle();
            //MazeGrid.Children.Add(r);
        }

        void UpdateMaze()
        {
            if (MazeBoard == null) return;

            double width = MazeCanvas.Width / MazeBoard.Width;
            double height = MazeCanvas.Height / MazeBoard.Height;
            double xMargin = (MazeCanvas.Width - width * MazeBoard.Width) / 2;
            double yMargin = (MazeCanvas.Height - height * MazeBoard.Height) / 2;
            Size size = new Size(width, height);
            Point point = new Point(0, 0);

            for (int i = 0; i < MazeBoard.Width; i++)
            {
                for (int j = 0; j < MazeBoard.Height; j++)
                    UpdateCell(i, j);
            }
            //MazeCanvas.UpdateLayout();
        }

        void UpdateCell(int x, int y)
        {
            Brush brush = new SolidColorBrush(CellBrushes[(int)MazeBoard.Cells[x][y].CellStatus]);
            mazeRects[x][y].Fill = brush;
        }
    }
}

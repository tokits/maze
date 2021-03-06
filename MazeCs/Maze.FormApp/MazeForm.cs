﻿//using Maze.SilverLight.Logic;
using Maze.Logic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// 迷路プログラムのnamespace
/// </summary>
namespace Maze.FormApp
{
    /// <summary>
    /// 迷路プログラムのメインウィンドウ
    /// </summary>
    public partial class MazeForm : Form
    {
        /// <summary>
        /// 迷路の横ブロック数
        /// </summary>
        const int MazeWidth = 40;
        /// <summary>
        /// 迷路の縦ブロック数
        /// </summary>
        const int MazeHieght = 30;
        //const int MazeWidth = 100;
        //const int MazeHieght = 80;

        /// <summary>
        /// 迷路ロジック
        /// </summary>
        MazeBoard MazeBoard = new MazeBoard(MazeWidth, MazeHieght);

        /// <summary>
        /// 迷路描画用のブラシ
        /// </summary>
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
        Brush[] CellBrushes = {
            Brushes.DarkGray,
            Brushes.SaddleBrown,
            Brushes.Red,
            Brushes.Blue,
            Brushes.White,
            Brushes.MediumPurple,
            Brushes.White
        };

        /// <summary>
        /// 迷路生成用タイマ
        /// </summary>
        Timer createMazeTimer = new Timer();
        /// <summary>
        /// 迷路回答用タイマ
        /// </summary>
        Timer answerMazeTimer = new Timer();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MazeForm()
        {
            InitializeComponent();

            answerStartButton.Enabled = false;
            answerClearButton.Enabled = false;

            mazeCreateButton.Click += (s, e) =>
                {
                    createMazeTimer.Stop();
                    answerMazeTimer.Stop();

                    answerStartButton.Enabled = false;
                    answerClearButton.Enabled = false;

                    MazeBoard = new MazeBoard(MazeWidth, MazeHieght);
                    MazeBoard.CreationStart();
                    createMazeTimer.Start();
                };
            mazeClearButton.Click += (s, e) =>
                {
                    createMazeTimer.Stop();
                    answerMazeTimer.Stop();

                    answerStartButton.Enabled = false;
                    answerClearButton.Enabled = false;

                    MazeBoard = new MazeBoard(MazeWidth, MazeHieght);
                    //MazeBoard.CreationStart();
                    //while (MazeBoard.CreationNext()) ;
                    mazePictureBox.Invalidate();
                };
            answerStartButton.Click += (s, e) =>
                {
                    if (MazeBoard.IsCompleted)
                    {
                        MazeBoard.AnswerStart();
                        answerMazeTimer.Start();
                    }
                };
            answerClearButton.Click += (s, e) =>
                {
                    createMazeTimer.Stop();
                    answerMazeTimer.Stop();

                    if (MazeBoard.IsCompleted)
                    {
                        MazeBoard.AnswerClear();
                        mazePictureBox.Invalidate();
                    }
                };

            createMazeTimer.Interval = 1;
            createMazeTimer.Tick += (s, e) =>
            {
                if (MazeBoard.CreationNext())
                    mazePictureBox.Invalidate();
                else
                {
                    answerStartButton.Enabled = true;
                    answerClearButton.Enabled = true;
                    createMazeTimer.Stop();
                    MessageBox.Show("Completed!");
                }
            };
            answerMazeTimer.Interval = 1;
            answerMazeTimer.Tick += (s, e) =>
            {
                if (MazeBoard.AnswerNext())
                    mazePictureBox.Invalidate();
                else
                {
                    answerMazeTimer.Stop();
                    MessageBox.Show("Answer Completed!");
                }
            };

            mazePictureBox.Paint += (s, e) => mazePictureBox_Paint(e);
            this.Resize += (s, e) => MazeForm_Resize();
        }

        /// <summary>
        /// mazePictureBoxのPaintイベントハンドラ
        /// </summary>
        /// <param name="e"></param>
        private void mazePictureBox_Paint(PaintEventArgs e)
        {
            if (MazeBoard == null) return;

            int width = mazePictureBox.Size.Width / MazeBoard.Width;
            int height = mazePictureBox.Size.Height / MazeBoard.Height;
            int xMargin = (mazePictureBox.Size.Width - width * MazeBoard.Width)/2;
            int yMargin = (mazePictureBox.Size.Height - height * MazeBoard.Height) / 2;
            SizeF size = new SizeF(width, height);
            PointF point = new PointF(0, 0);
            Graphics g = e.Graphics;

            for (int i = 0; i < MazeBoard.Width; i++)
            {
                for (int j = 0; j < MazeBoard.Height; j++)
                {
                    point.X = i * size.Width + xMargin;
                    point.Y = j * size.Height + yMargin;
                    RectangleF rect = new RectangleF(point, size);
                    Brush brush = CellBrushes[(int)MazeBoard.Cells[i][j].CellStatus];
                    g.FillRectangle(brush, rect);
                }
            }
        }

        /// <summary>
        /// メインウィンドウのResizeイベントハンドラ
        /// </summary>
        private void MazeForm_Resize()
        {
            mazePictureBox.Invalidate();
        }
    }
}

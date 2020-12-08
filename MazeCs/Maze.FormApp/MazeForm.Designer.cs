namespace Maze.FormApp
{
    partial class MazeForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.mazePictureBox = new System.Windows.Forms.PictureBox();
            this.mazeCreateButton = new System.Windows.Forms.Button();
            this.mazeClearButton = new System.Windows.Forms.Button();
            this.answerStartButton = new System.Windows.Forms.Button();
            this.answerClearButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mazePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mazePictureBox
            // 
            this.mazePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mazePictureBox.BackColor = System.Drawing.Color.Black;
            this.mazePictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mazePictureBox.Location = new System.Drawing.Point(13, 13);
            this.mazePictureBox.Name = "mazePictureBox";
            this.mazePictureBox.Size = new System.Drawing.Size(456, 318);
            this.mazePictureBox.TabIndex = 0;
            this.mazePictureBox.TabStop = false;
            // 
            // mazeCreateButton
            // 
            this.mazeCreateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mazeCreateButton.Location = new System.Drawing.Point(13, 339);
            this.mazeCreateButton.Name = "mazeCreateButton";
            this.mazeCreateButton.Size = new System.Drawing.Size(97, 23);
            this.mazeCreateButton.TabIndex = 1;
            this.mazeCreateButton.Text = "Maze Create";
            this.mazeCreateButton.UseVisualStyleBackColor = true;
            // 
            // mazeClearButton
            // 
            this.mazeClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mazeClearButton.Location = new System.Drawing.Point(116, 339);
            this.mazeClearButton.Name = "mazeClearButton";
            this.mazeClearButton.Size = new System.Drawing.Size(97, 23);
            this.mazeClearButton.TabIndex = 2;
            this.mazeClearButton.Text = "Maze Clear";
            this.mazeClearButton.UseVisualStyleBackColor = true;
            // 
            // answerStartButton
            // 
            this.answerStartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.answerStartButton.Location = new System.Drawing.Point(219, 339);
            this.answerStartButton.Name = "answerStartButton";
            this.answerStartButton.Size = new System.Drawing.Size(97, 23);
            this.answerStartButton.TabIndex = 3;
            this.answerStartButton.Text = "Answer Start";
            this.answerStartButton.UseVisualStyleBackColor = true;
            // 
            // answerClearButton
            // 
            this.answerClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.answerClearButton.Location = new System.Drawing.Point(322, 339);
            this.answerClearButton.Name = "answerClearButton";
            this.answerClearButton.Size = new System.Drawing.Size(97, 23);
            this.answerClearButton.TabIndex = 4;
            this.answerClearButton.Text = "Answer Clear";
            this.answerClearButton.UseVisualStyleBackColor = true;
            // 
            // MazeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 374);
            this.Controls.Add(this.answerClearButton);
            this.Controls.Add(this.answerStartButton);
            this.Controls.Add(this.mazeClearButton);
            this.Controls.Add(this.mazeCreateButton);
            this.Controls.Add(this.mazePictureBox);
            this.Name = "MazeForm";
            this.Text = "MazeForm";
            ((System.ComponentModel.ISupportInitialize)(this.mazePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox mazePictureBox;
        private System.Windows.Forms.Button mazeCreateButton;
        private System.Windows.Forms.Button mazeClearButton;
        private System.Windows.Forms.Button answerStartButton;
        private System.Windows.Forms.Button answerClearButton;
    }
}


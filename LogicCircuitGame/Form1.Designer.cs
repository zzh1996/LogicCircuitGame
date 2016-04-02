namespace LogicCircuitGame
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.模拟ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始模拟ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止模拟ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(0, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(682, 438);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.模拟ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(706, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // 模拟ToolStripMenuItem
            // 
            this.模拟ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始模拟ToolStripMenuItem,
            this.停止模拟ToolStripMenuItem});
            this.模拟ToolStripMenuItem.Name = "模拟ToolStripMenuItem";
            this.模拟ToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.模拟ToolStripMenuItem.Text = "模拟(&S)";
            // 
            // 开始模拟ToolStripMenuItem
            // 
            this.开始模拟ToolStripMenuItem.Name = "开始模拟ToolStripMenuItem";
            this.开始模拟ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.开始模拟ToolStripMenuItem.Text = "开始模拟";
            this.开始模拟ToolStripMenuItem.Click += new System.EventHandler(this.开始模拟ToolStripMenuItem_Click);
            // 
            // 停止模拟ToolStripMenuItem
            // 
            this.停止模拟ToolStripMenuItem.Name = "停止模拟ToolStripMenuItem";
            this.停止模拟ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.停止模拟ToolStripMenuItem.Text = "停止模拟";
            this.停止模拟ToolStripMenuItem.Click += new System.EventHandler(this.停止模拟ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 462);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "LogicCircuitGame";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 模拟ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始模拟ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止模拟ToolStripMenuItem;
    }
}


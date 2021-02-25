
namespace WinFormsHostNet50Sample
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
            this._toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._clearToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._logVerboseToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._logDebugToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._logInformationToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._logWarningToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._logErrorToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._logFatalToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._richTextBoxPanel = new System.Windows.Forms.Panel();
            this._logParallelForToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._logTaskRunToolStripButton = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip1
            // 
            this._toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._clearToolStripButton,
            this._logVerboseToolStripButton,
            this._logDebugToolStripButton,
            this._logInformationToolStripButton,
            this._logWarningToolStripButton,
            this._logErrorToolStripButton,
            this._logFatalToolStripButton,
            toolStripSeparator1,
            this._logParallelForToolStripButton,
            toolStripSeparator2,
            this._logTaskRunToolStripButton});
            this._toolStrip1.Location = new System.Drawing.Point(0, 0);
            this._toolStrip1.Name = "_toolStrip1";
            this._toolStrip1.Size = new System.Drawing.Size(784, 25);
            this._toolStrip1.TabIndex = 0;
            this._toolStrip1.Text = "toolStrip";
            // 
            // _clearToolStripButton
            // 
            this._clearToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._clearToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_clearToolStripButton.Image")));
            this._clearToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._clearToolStripButton.Name = "_clearToolStripButton";
            this._clearToolStripButton.Size = new System.Drawing.Size(38, 22);
            this._clearToolStripButton.Text = "Clear";
            // 
            // _logVerboseToolStripButton
            // 
            this._logVerboseToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._logVerboseToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_logVerboseToolStripButton.Image")));
            this._logVerboseToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._logVerboseToolStripButton.Name = "_logVerboseToolStripButton";
            this._logVerboseToolStripButton.Size = new System.Drawing.Size(52, 22);
            this._logVerboseToolStripButton.Text = "Verbose";
            // 
            // _logDebugToolStripButton
            // 
            this._logDebugToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._logDebugToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_logDebugToolStripButton.Image")));
            this._logDebugToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._logDebugToolStripButton.Name = "_logDebugToolStripButton";
            this._logDebugToolStripButton.Size = new System.Drawing.Size(46, 22);
            this._logDebugToolStripButton.Text = "Debug";
            // 
            // _logInformationToolStripButton
            // 
            this._logInformationToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._logInformationToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_logInformationToolStripButton.Image")));
            this._logInformationToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._logInformationToolStripButton.Name = "_logInformationToolStripButton";
            this._logInformationToolStripButton.Size = new System.Drawing.Size(74, 22);
            this._logInformationToolStripButton.Text = "Information";
            // 
            // _logWarningToolStripButton
            // 
            this._logWarningToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._logWarningToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_logWarningToolStripButton.Image")));
            this._logWarningToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._logWarningToolStripButton.Name = "_logWarningToolStripButton";
            this._logWarningToolStripButton.Size = new System.Drawing.Size(56, 22);
            this._logWarningToolStripButton.Text = "Warning";
            // 
            // _logErrorToolStripButton
            // 
            this._logErrorToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._logErrorToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_logErrorToolStripButton.Image")));
            this._logErrorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._logErrorToolStripButton.Name = "_logErrorToolStripButton";
            this._logErrorToolStripButton.Size = new System.Drawing.Size(36, 22);
            this._logErrorToolStripButton.Text = "Error";
            // 
            // _logFatalToolStripButton
            // 
            this._logFatalToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._logFatalToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_logFatalToolStripButton.Image")));
            this._logFatalToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._logFatalToolStripButton.Name = "_logFatalToolStripButton";
            this._logFatalToolStripButton.Size = new System.Drawing.Size(36, 22);
            this._logFatalToolStripButton.Text = "Fatal";
            // 
            // _richTextBoxPanel
            // 
            this._richTextBoxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._richTextBoxPanel.Location = new System.Drawing.Point(0, 25);
            this._richTextBoxPanel.Name = "_richTextBoxPanel";
            this._richTextBoxPanel.Size = new System.Drawing.Size(784, 536);
            this._richTextBoxPanel.TabIndex = 1;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _logParallelForToolStripButton
            // 
            this._logParallelForToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._logParallelForToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_logParallelForToolStripButton.Image")));
            this._logParallelForToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._logParallelForToolStripButton.Name = "_logParallelForToolStripButton";
            this._logParallelForToolStripButton.Size = new System.Drawing.Size(106, 22);
            this._logParallelForToolStripButton.Text = "Parallel.For(100*6)";
            // 
            // _logTaskRunToolStripButton
            // 
            this._logTaskRunToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._logTaskRunToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_logTaskRunToolStripButton.Image")));
            this._logTaskRunToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._logTaskRunToolStripButton.Name = "_logTaskRunToolStripButton";
            this._logTaskRunToolStripButton.Size = new System.Drawing.Size(94, 22);
            this._logTaskRunToolStripButton.Text = "Task.Run(100*6)";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this._richTextBoxPanel);
            this.Controls.Add(this._toolStrip1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm (Windows Forms hosting WPF\'s RichTextBox on .NET 5)";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this._toolStrip1.ResumeLayout(false);
            this._toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip1;
        private System.Windows.Forms.Panel _richTextBoxPanel;
        private System.Windows.Forms.ToolStripButton _clearToolStripButton;
        private System.Windows.Forms.ToolStripButton _logVerboseToolStripButton;
        private System.Windows.Forms.ToolStripButton _logDebugToolStripButton;
        private System.Windows.Forms.ToolStripButton _logInformationToolStripButton;
        private System.Windows.Forms.ToolStripButton _logWarningToolStripButton;
        private System.Windows.Forms.ToolStripButton _logErrorToolStripButton;
        private System.Windows.Forms.ToolStripButton _logFatalToolStripButton;
        private System.Windows.Forms.ToolStripButton _logParallelForToolStripButton;
        private System.Windows.Forms.ToolStripButton _logTaskRunToolStripButton;
    }
}


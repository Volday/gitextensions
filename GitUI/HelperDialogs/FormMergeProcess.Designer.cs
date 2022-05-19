namespace GitUI.HelperDialogs
{
    partial class FormMergeProcess
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ResetAndRetry = new System.Windows.Forms.Button();
            this.ControlsPanel.Controls.Add(ResetAndRetry);
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOutput
            // 
            this.pnlOutput.Size = new System.Drawing.Size(565, 285);
            // 
            // MainPanel
            // 
            this.MainPanel.Size = new System.Drawing.Size(565, 285);
            // 
            // ResetAndRetry
            // 
            this.ResetAndRetry.AutoSize = true;
            this.ResetAndRetry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ResetAndRetry.Location = new System.Drawing.Point(290, 8);
            this.ResetAndRetry.MinimumSize = new System.Drawing.Size(75, 23);
            this.ResetAndRetry.Name = "ResetAndRetry";
            this.ResetAndRetry.Size = new System.Drawing.Size(100, 23);
            this.ResetAndRetry.TabIndex = 0;
            this.ResetAndRetry.Text = "Reset and Retry";
            this.ResetAndRetry.UseCompatibleTextRendering = true;
            this.ResetAndRetry.UseVisualStyleBackColor = true;
            this.ResetAndRetry.Click += new System.EventHandler(this.ResetAndRetry_Click);
            // 
            // FormMergeProcess
            // 

            this.ControlsPanel.Controls.SetChildIndex(this.ResetAndRetry, 2);
            this.MainPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Button ResetAndRetry;
    }
}

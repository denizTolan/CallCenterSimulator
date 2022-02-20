namespace CallCenterSimulator.Agent.Coordinator
{
    partial class Form1
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
            this.AgentQueue_ListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // AgentQueue_ListBox
            // 
            this.AgentQueue_ListBox.FormattingEnabled = true;
            this.AgentQueue_ListBox.ItemHeight = 15;
            this.AgentQueue_ListBox.Location = new System.Drawing.Point(1, 30);
            this.AgentQueue_ListBox.Name = "AgentQueue_ListBox";
            this.AgentQueue_ListBox.Size = new System.Drawing.Size(801, 214);
            this.AgentQueue_ListBox.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.AgentQueue_ListBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox AgentQueue_ListBox;
    }
}
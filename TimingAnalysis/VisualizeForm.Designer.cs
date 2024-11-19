namespace TimingAnalysis
{
    partial class VisualizeForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualizeForm));
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.stimulationTimer = new System.Windows.Forms.Timer(this.components);
            this.colorChangeTimer = new System.Windows.Forms.Timer(this.components);
            this.reseiveClientControl1 = new NetManager.Client.ReseiveClientControl();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPictureBox.BackColor = System.Drawing.Color.Black;
            this.mainPictureBox.Location = new System.Drawing.Point(0, 0);
            this.mainPictureBox.Name = "mainPictureBox";
            this.mainPictureBox.Size = new System.Drawing.Size(783, 450);
            this.mainPictureBox.TabIndex = 0;
            this.mainPictureBox.TabStop = false;
            // 
            // stimulationTimer
            // 
            this.stimulationTimer.Tick += new System.EventHandler(this.stimulationTimer_Tick);
            // 
            // colorChangeTimer
            // 
            this.colorChangeTimer.Tick += new System.EventHandler(this.colorChangeTimer_Tick);
            // 
            // reseiveClientControl1
            // 
            this.reseiveClientControl1.ClientName = "FlashStimulationClient";
            this.reseiveClientControl1.IPServer = ((System.Net.IPAddress)(resources.GetObject("reseiveClientControl1.IPServer")));
            this.reseiveClientControl1.Location = new System.Drawing.Point(598, 12);
            this.reseiveClientControl1.Name = "reseiveClientControl1";
            this.reseiveClientControl1.Size = new System.Drawing.Size(174, 98);
            this.reseiveClientControl1.TabIndex = 1;
            this.reseiveClientControl1.ReseiveData += new System.EventHandler<NetManager.EventClientMsgArgs>(this.MessageHandler);
            // 
            // VisualizeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 450);
            this.Controls.Add(this.mainPictureBox);
            this.Controls.Add(this.reseiveClientControl1);
            this.Name = "VisualizeForm";
            this.Text = "VisualizeForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VisualizeForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox mainPictureBox;
        private System.Windows.Forms.Timer stimulationTimer;
        private System.Windows.Forms.Timer colorChangeTimer;
        private NetManager.Client.ReseiveClientControl reseiveClientControl1;
    }
}
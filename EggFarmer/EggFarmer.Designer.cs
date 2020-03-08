namespace EggFarmer
{
    partial class formEggFinder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formEggFinder));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblEggsFound = new System.Windows.Forms.Label();
            this.lblEggsFoundValue = new System.Windows.Forms.Label();
            this.lblEggsTotal = new System.Windows.Forms.Label();
            this.lblEggsTotalValue = new System.Windows.Forms.Label();
            this.workerFluteRoutine = new System.ComponentModel.BackgroundWorker();
            this.lblStatus = new System.Windows.Forms.Label();
            this.workerEggFinder = new System.ComponentModel.BackgroundWorker();
            this.cboxAreas = new System.Windows.Forms.ComboBox();
            this.btnShowAreas = new System.Windows.Forms.Button();
            this.workerCombat = new System.ComponentModel.BackgroundWorker();
            this.pboxEggSuicide = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pboxEggSuicide)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(78, 60);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(96, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(78, 60);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(355, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(78, 60);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblEggsFound
            // 
            this.lblEggsFound.AutoSize = true;
            this.lblEggsFound.Location = new System.Drawing.Point(178, 12);
            this.lblEggsFound.Name = "lblEggsFound";
            this.lblEggsFound.Size = new System.Drawing.Size(67, 13);
            this.lblEggsFound.TabIndex = 7;
            this.lblEggsFound.Text = "Eggs Found:";
            // 
            // lblEggsFoundValue
            // 
            this.lblEggsFoundValue.AutoSize = true;
            this.lblEggsFoundValue.Location = new System.Drawing.Point(251, 12);
            this.lblEggsFoundValue.Name = "lblEggsFoundValue";
            this.lblEggsFoundValue.Size = new System.Drawing.Size(13, 13);
            this.lblEggsFoundValue.TabIndex = 8;
            this.lblEggsFoundValue.Text = "0";
            // 
            // lblEggsTotal
            // 
            this.lblEggsTotal.AutoSize = true;
            this.lblEggsTotal.Location = new System.Drawing.Point(270, 12);
            this.lblEggsTotal.Name = "lblEggsTotal";
            this.lblEggsTotal.Size = new System.Drawing.Size(61, 13);
            this.lblEggsTotal.TabIndex = 9;
            this.lblEggsTotal.Text = "Eggs Total:";
            // 
            // lblEggsTotalValue
            // 
            this.lblEggsTotalValue.AutoSize = true;
            this.lblEggsTotalValue.Location = new System.Drawing.Point(337, 12);
            this.lblEggsTotalValue.Name = "lblEggsTotalValue";
            this.lblEggsTotalValue.Size = new System.Drawing.Size(13, 13);
            this.lblEggsTotalValue.TabIndex = 10;
            this.lblEggsTotalValue.Text = "0";
            // 
            // workerFluteRoutine
            // 
            this.workerFluteRoutine.WorkerReportsProgress = true;
            this.workerFluteRoutine.WorkerSupportsCancellation = true;
            this.workerFluteRoutine.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerFluteRoutine_DoWork);
            this.workerFluteRoutine.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.workerFluteRoutine_ProgressChanged);
            this.workerFluteRoutine.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerFluteRoutine_WorkerComplete);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(12, 299);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 13;
            // 
            // workerEggFinder
            // 
            this.workerEggFinder.WorkerReportsProgress = true;
            this.workerEggFinder.WorkerSupportsCancellation = true;
            this.workerEggFinder.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerEggFinder_DoWork);
            this.workerEggFinder.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.workerEggFinder_ProgressChanged);
            this.workerEggFinder.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerEggFinder_WorkerComplete);
            // 
            // cboxAreas
            // 
            this.cboxAreas.FormattingEnabled = true;
            this.cboxAreas.Items.AddRange(new object[] {
            "Select Area",
            "Area 1",
            "Area 2",
            "Area 3",
            "Area 4",
            "Area 5"});
            this.cboxAreas.Location = new System.Drawing.Point(181, 52);
            this.cboxAreas.Name = "cboxAreas";
            this.cboxAreas.Size = new System.Drawing.Size(166, 21);
            this.cboxAreas.TabIndex = 14;
            this.cboxAreas.Text = "Select Area";
            this.cboxAreas.SelectedIndexChanged += new System.EventHandler(this.cboxAreas_SelectedIndexChanged);
            // 
            // btnShowAreas
            // 
            this.btnShowAreas.Location = new System.Drawing.Point(182, 330);
            this.btnShowAreas.Name = "btnShowAreas";
            this.btnShowAreas.Size = new System.Drawing.Size(75, 23);
            this.btnShowAreas.TabIndex = 15;
            this.btnShowAreas.Text = "Show Areas";
            this.btnShowAreas.UseVisualStyleBackColor = true;
            this.btnShowAreas.Click += new System.EventHandler(this.btnShowAreas_Click);
            // 
            // workerCombat
            // 
            this.workerCombat.WorkerReportsProgress = true;
            this.workerCombat.WorkerSupportsCancellation = true;
            this.workerCombat.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerCombat_DoWork);
            this.workerCombat.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.workerCombat_ProgressChanged);
            this.workerCombat.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerCombat_WorkerComplete);
            // 
            // pboxEggSuicide
            // 
            this.pboxEggSuicide.Image = ((System.Drawing.Image)(resources.GetObject("pboxEggSuicide.Image")));
            this.pboxEggSuicide.Location = new System.Drawing.Point(12, 78);
            this.pboxEggSuicide.Name = "pboxEggSuicide";
            this.pboxEggSuicide.Size = new System.Drawing.Size(421, 191);
            this.pboxEggSuicide.TabIndex = 16;
            this.pboxEggSuicide.TabStop = false;
            // 
            // formEggFinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 365);
            this.Controls.Add(this.pboxEggSuicide);
            this.Controls.Add(this.btnShowAreas);
            this.Controls.Add(this.cboxAreas);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblEggsTotalValue);
            this.Controls.Add(this.lblEggsTotal);
            this.Controls.Add(this.lblEggsFoundValue);
            this.Controls.Add(this.lblEggsFound);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.MaximizeBox = false;
            this.Name = "formEggFinder";
            this.Text = "(unisharp) Egg Farmer";
            this.Load += new System.EventHandler(this.formEggFinder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pboxEggSuicide)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblEggsFound;
        private System.Windows.Forms.Label lblEggsFoundValue;
        private System.Windows.Forms.Label lblEggsTotal;
        private System.Windows.Forms.Label lblEggsTotalValue;
        private System.ComponentModel.BackgroundWorker workerFluteRoutine;
        private System.Windows.Forms.Label lblStatus;
        private System.ComponentModel.BackgroundWorker workerEggFinder;
        private System.Windows.Forms.ComboBox cboxAreas;
        private System.Windows.Forms.Button btnShowAreas;
        private System.ComponentModel.BackgroundWorker workerCombat;
        private System.Windows.Forms.PictureBox pboxEggSuicide;
    }
}


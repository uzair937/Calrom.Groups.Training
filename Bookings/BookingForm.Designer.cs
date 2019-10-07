namespace Bookings
{
    partial class BookingForm
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
            this.topLeftBox = new System.Windows.Forms.TextBox();
            this.BookEntry1 = new System.Windows.Forms.TextBox();
            this.BookEntry2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CompareButton = new System.Windows.Forms.Button();
            this.DetailsBox = new System.Windows.Forms.RichTextBox();
            this.viewButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.viewBooking = new System.Windows.Forms.TextBox();
            this.RandomButt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // topLeftBox
            // 
            this.topLeftBox.BackColor = System.Drawing.SystemColors.Info;
            this.topLeftBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this.topLeftBox.Location = new System.Drawing.Point(12, 12);
            this.topLeftBox.Name = "topLeftBox";
            this.topLeftBox.Size = new System.Drawing.Size(479, 20);
            this.topLeftBox.TabIndex = 0;
            // 
            // BookEntry1
            // 
            this.BookEntry1.Location = new System.Drawing.Point(12, 318);
            this.BookEntry1.Name = "BookEntry1";
            this.BookEntry1.Size = new System.Drawing.Size(100, 20);
            this.BookEntry1.TabIndex = 1;
            this.BookEntry1.Text = "0";
            // 
            // BookEntry2
            // 
            this.BookEntry2.Location = new System.Drawing.Point(12, 344);
            this.BookEntry2.Name = "BookEntry2";
            this.BookEntry2.Size = new System.Drawing.Size(100, 20);
            this.BookEntry2.TabIndex = 2;
            this.BookEntry2.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(12, 302);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Compare Bookings";
            // 
            // CompareButton
            // 
            this.CompareButton.Location = new System.Drawing.Point(15, 370);
            this.CompareButton.Name = "CompareButton";
            this.CompareButton.Size = new System.Drawing.Size(75, 23);
            this.CompareButton.TabIndex = 4;
            this.CompareButton.Text = "Compare";
            this.CompareButton.UseVisualStyleBackColor = true;
            this.CompareButton.Click += new System.EventHandler(this.CompareButton_Click);
            // 
            // DetailsBox
            // 
            this.DetailsBox.BackColor = System.Drawing.SystemColors.Info;
            this.DetailsBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this.DetailsBox.Location = new System.Drawing.Point(12, 47);
            this.DetailsBox.Name = "DetailsBox";
            this.DetailsBox.ReadOnly = true;
            this.DetailsBox.Size = new System.Drawing.Size(479, 238);
            this.DetailsBox.TabIndex = 5;
            this.DetailsBox.Text = "";
            // 
            // viewButton
            // 
            this.viewButton.Location = new System.Drawing.Point(394, 344);
            this.viewButton.Name = "viewButton";
            this.viewButton.Size = new System.Drawing.Size(75, 23);
            this.viewButton.TabIndex = 9;
            this.viewButton.Text = "Search";
            this.viewButton.UseVisualStyleBackColor = true;
            this.viewButton.Click += new System.EventHandler(this.viewButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(391, 302);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "View Booking";
            // 
            // viewBooking
            // 
            this.viewBooking.Location = new System.Drawing.Point(391, 318);
            this.viewBooking.Name = "viewBooking";
            this.viewBooking.Size = new System.Drawing.Size(100, 20);
            this.viewBooking.TabIndex = 6;
            this.viewBooking.Text = "0";
            // 
            // RandomButt
            // 
            this.RandomButt.Location = new System.Drawing.Point(214, 344);
            this.RandomButt.Name = "RandomButt";
            this.RandomButt.Size = new System.Drawing.Size(75, 23);
            this.RandomButt.TabIndex = 10;
            this.RandomButt.Text = "Random";
            this.RandomButt.UseVisualStyleBackColor = true;
            this.RandomButt.Click += new System.EventHandler(this.RandomButt_Click);
            // 
            // BookingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(506, 407);
            this.Controls.Add(this.RandomButt);
            this.Controls.Add(this.viewButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.viewBooking);
            this.Controls.Add(this.DetailsBox);
            this.Controls.Add(this.CompareButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BookEntry2);
            this.Controls.Add(this.BookEntry1);
            this.Controls.Add(this.topLeftBox);
            this.Name = "BookingForm";
            this.Text = "Booking Interface";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox topLeftBox;
        public System.Windows.Forms.RichTextBox DetailsBox;
        public System.Windows.Forms.TextBox BookEntry1;
        public System.Windows.Forms.TextBox BookEntry2;
        public System.Windows.Forms.Button CompareButton;
        public System.Windows.Forms.Button viewButton;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox viewBooking;
        public System.Windows.Forms.Button RandomButt;
    }
}
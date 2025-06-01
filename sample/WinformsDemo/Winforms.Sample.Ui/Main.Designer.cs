namespace Winforms.Sample.Ui
{
    partial class Main
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
            ButtonSubmitLecturer = new Button();
            LabelName = new Label();
            TextBoxName = new TextBox();
            PictureBoxErrorIcon = new PictureBox();
            PanelErrorList = new Panel();
            LabelErrors = new Label();
            splitContainer1 = new SplitContainer();
            TextBoxInstructionsLecturer = new TextBox();
            LabelUnitId = new Label();
            TextBoxUnitId = new TextBox();
            TextBoxInstructionsUnit = new TextBox();
            PanelErrorList2 = new Panel();
            LabelErrors2 = new Label();
            PictureBoxErrorIcon2 = new PictureBox();
            ButtonSubmitUnit = new Button();
            LabelUnitName = new Label();
            TextBoxUnit = new TextBox();
            ((System.ComponentModel.ISupportInitialize)PictureBoxErrorIcon).BeginInit();
            PanelErrorList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            PanelErrorList2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxErrorIcon2).BeginInit();
            SuspendLayout();
            // 
            // ButtonSubmitLecturer
            // 
            ButtonSubmitLecturer.Location = new Point(85, 149);
            ButtonSubmitLecturer.Name = "ButtonSubmitLecturer";
            ButtonSubmitLecturer.Size = new Size(75, 23);
            ButtonSubmitLecturer.TabIndex = 0;
            ButtonSubmitLecturer.Text = "Submit";
            ButtonSubmitLecturer.UseVisualStyleBackColor = true;
            ButtonSubmitLecturer.Click += ButtonSubmitLecturer_Click;
            // 
            // LabelName
            // 
            LabelName.AutoSize = true;
            LabelName.Location = new Point(85, 83);
            LabelName.Name = "LabelName";
            LabelName.Size = new Size(118, 15);
            LabelName.TabIndex = 1;
            LabelName.Text = "Enter Lecturer Name:";
            // 
            // TextBoxName
            // 
            TextBoxName.Location = new Point(85, 110);
            TextBoxName.Name = "TextBoxName";
            TextBoxName.Size = new Size(118, 23);
            TextBoxName.TabIndex = 2;
            // 
            // PictureBoxErrorIcon
            // 
            PictureBoxErrorIcon.Image = Properties.Resources.OverlayError;
            PictureBoxErrorIcon.Location = new Point(0, 0);
            PictureBoxErrorIcon.Margin = new Padding(0);
            PictureBoxErrorIcon.Name = "PictureBoxErrorIcon";
            PictureBoxErrorIcon.Size = new Size(32, 32);
            PictureBoxErrorIcon.TabIndex = 3;
            PictureBoxErrorIcon.TabStop = false;
            // 
            // PanelErrorList
            // 
            PanelErrorList.Controls.Add(LabelErrors);
            PanelErrorList.Controls.Add(PictureBoxErrorIcon);
            PanelErrorList.Location = new Point(85, 194);
            PanelErrorList.Name = "PanelErrorList";
            PanelErrorList.Size = new Size(320, 220);
            PanelErrorList.TabIndex = 4;
            PanelErrorList.Visible = false;
            // 
            // LabelErrors
            // 
            LabelErrors.Location = new Point(10, 40);
            LabelErrors.Name = "LabelErrors";
            LabelErrors.Size = new Size(300, 128);
            LabelErrors.TabIndex = 4;
            LabelErrors.Text = "label1";
            // 
            // splitContainer1
            // 
            splitContainer1.BorderStyle = BorderStyle.FixedSingle;
            splitContainer1.Location = new Point(12, 12);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(TextBoxInstructionsLecturer);
            splitContainer1.Panel1.Controls.Add(ButtonSubmitLecturer);
            splitContainer1.Panel1.Controls.Add(PanelErrorList);
            splitContainer1.Panel1.Controls.Add(LabelName);
            splitContainer1.Panel1.Controls.Add(TextBoxName);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(LabelUnitId);
            splitContainer1.Panel2.Controls.Add(TextBoxUnitId);
            splitContainer1.Panel2.Controls.Add(TextBoxInstructionsUnit);
            splitContainer1.Panel2.Controls.Add(PanelErrorList2);
            splitContainer1.Panel2.Controls.Add(ButtonSubmitUnit);
            splitContainer1.Panel2.Controls.Add(LabelUnitName);
            splitContainer1.Panel2.Controls.Add(TextBoxUnit);
            splitContainer1.Size = new Size(926, 687);
            splitContainer1.SplitterDistance = 466;
            splitContainer1.TabIndex = 6;
            // 
            // TextBoxInstructionsLecturer
            // 
            TextBoxInstructionsLecturer.Location = new Point(85, 20);
            TextBoxInstructionsLecturer.Multiline = true;
            TextBoxInstructionsLecturer.Name = "TextBoxInstructionsLecturer";
            TextBoxInstructionsLecturer.ReadOnly = true;
            TextBoxInstructionsLecturer.Size = new Size(320, 54);
            TextBoxInstructionsLecturer.TabIndex = 9;
            // 
            // LabelUnitId
            // 
            LabelUnitId.AutoSize = true;
            LabelUnitId.Location = new Point(185, 117);
            LabelUnitId.Name = "LabelUnitId";
            LabelUnitId.Size = new Size(75, 15);
            LabelUnitId.TabIndex = 9;
            LabelUnitId.Text = "Enter Unit Id:";
            // 
            // TextBoxUnitId
            // 
            TextBoxUnitId.Location = new Point(185, 144);
            TextBoxUnitId.Name = "TextBoxUnitId";
            TextBoxUnitId.Size = new Size(118, 23);
            TextBoxUnitId.TabIndex = 10;
            // 
            // TextBoxInstructionsUnit
            // 
            TextBoxInstructionsUnit.Location = new Point(54, 20);
            TextBoxInstructionsUnit.Multiline = true;
            TextBoxInstructionsUnit.Name = "TextBoxInstructionsUnit";
            TextBoxInstructionsUnit.ReadOnly = true;
            TextBoxInstructionsUnit.Size = new Size(356, 86);
            TextBoxInstructionsUnit.TabIndex = 8;
            // 
            // PanelErrorList2
            // 
            PanelErrorList2.Controls.Add(LabelErrors2);
            PanelErrorList2.Controls.Add(PictureBoxErrorIcon2);
            PanelErrorList2.Location = new Point(52, 228);
            PanelErrorList2.Name = "PanelErrorList2";
            PanelErrorList2.Size = new Size(358, 220);
            PanelErrorList2.TabIndex = 7;
            PanelErrorList2.Visible = false;
            // 
            // LabelErrors2
            // 
            LabelErrors2.Location = new Point(10, 40);
            LabelErrors2.Name = "LabelErrors2";
            LabelErrors2.Size = new Size(340, 128);
            LabelErrors2.TabIndex = 1;
            LabelErrors2.Text = "label1";
            // 
            // PictureBoxErrorIcon2
            // 
            PictureBoxErrorIcon2.Image = Properties.Resources.OverlayError;
            PictureBoxErrorIcon2.Location = new Point(0, 0);
            PictureBoxErrorIcon2.Margin = new Padding(0);
            PictureBoxErrorIcon2.Name = "PictureBoxErrorIcon2";
            PictureBoxErrorIcon2.Size = new Size(32, 32);
            PictureBoxErrorIcon2.TabIndex = 0;
            PictureBoxErrorIcon2.TabStop = false;
            // 
            // ButtonSubmitUnit
            // 
            ButtonSubmitUnit.Location = new Point(52, 183);
            ButtonSubmitUnit.Name = "ButtonSubmitUnit";
            ButtonSubmitUnit.Size = new Size(75, 23);
            ButtonSubmitUnit.TabIndex = 6;
            ButtonSubmitUnit.Text = "Submit";
            ButtonSubmitUnit.UseVisualStyleBackColor = true;
            ButtonSubmitUnit.Click += ButtonSubmitUnit_Click;
            // 
            // LabelUnitName
            // 
            LabelUnitName.AutoSize = true;
            LabelUnitName.Location = new Point(52, 117);
            LabelUnitName.Name = "LabelUnitName";
            LabelUnitName.Size = new Size(97, 15);
            LabelUnitName.TabIndex = 6;
            LabelUnitName.Text = "Enter Unit Name:";
            // 
            // TextBoxUnit
            // 
            TextBoxUnit.Location = new Point(52, 144);
            TextBoxUnit.Name = "TextBoxUnit";
            TextBoxUnit.Size = new Size(118, 23);
            TextBoxUnit.TabIndex = 6;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(953, 712);
            Controls.Add(splitContainer1);
            Name = "Main";
            Text = "Main";
            ((System.ComponentModel.ISupportInitialize)PictureBoxErrorIcon).EndInit();
            PanelErrorList.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            PanelErrorList2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PictureBoxErrorIcon2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button ButtonSubmitLecturer;
        private Label LabelName;
        private TextBox TextBoxName;
        private PictureBox PictureBoxErrorIcon;
        private Panel PanelErrorList;
        private Label LabelErrors;
        private SplitContainer splitContainer1;
        private TextBox TextBoxUnit;
        private Button ButtonSubmitUnit;
        private Label LabelUnitName;
        private Panel PanelErrorList2;
        private Label LabelErrors2;
        private PictureBox PictureBoxErrorIcon2;
        private TextBox TextBoxInstructionsUnit;
        private TextBox TextBoxInstructionsLecturer;
        private Label LabelUnitId;
        private TextBox TextBoxUnitId;
    }
}

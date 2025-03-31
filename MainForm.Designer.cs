namespace AssemblyPublicizerUI;

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
        this.dropPanel = new System.Windows.Forms.Panel();
        this.dropLabel = new System.Windows.Forms.Label();
        this.openFileButton = new System.Windows.Forms.Button();
        this.titleLabel = new System.Windows.Forms.Label();
        this.logTextBox = new System.Windows.Forms.TextBox();
        this.logLabel = new System.Windows.Forms.Label();
        this.stripMethodsCheckBox = new System.Windows.Forms.CheckBox();
        this.headerPanel = new System.Windows.Forms.Panel();
        this.infoLabel = new System.Windows.Forms.Label();
        this.bottomPanel = new System.Windows.Forms.Panel();
        this.versionLabel = new System.Windows.Forms.Label();
        this.dropPanel.SuspendLayout();
        this.headerPanel.SuspendLayout();
        this.bottomPanel.SuspendLayout();
        this.SuspendLayout();
        // 
        // headerPanel
        // 
        this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
        this.headerPanel.Controls.Add(this.titleLabel);
        this.headerPanel.Controls.Add(this.infoLabel);
        this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
        this.headerPanel.Location = new System.Drawing.Point(0, 0);
        this.headerPanel.Name = "headerPanel";
        this.headerPanel.Size = new System.Drawing.Size(550, 80);
        this.headerPanel.TabIndex = 7;
        // 
        // titleLabel
        // 
        this.titleLabel.AutoSize = true;
        this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.titleLabel.ForeColor = System.Drawing.Color.White;
        this.titleLabel.Location = new System.Drawing.Point(20, 20);
        this.titleLabel.Name = "titleLabel";
        this.titleLabel.Size = new System.Drawing.Size(115, 32);
        this.titleLabel.TabIndex = 3;
        this.titleLabel.Text = "Public1ty";
        // 
        // infoLabel
        // 
        this.infoLabel.AutoSize = true;
        this.infoLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.infoLabel.ForeColor = System.Drawing.Color.LightGray;
        this.infoLabel.Location = new System.Drawing.Point(145, 30);
        this.infoLabel.Name = "infoLabel";
        this.infoLabel.Size = new System.Drawing.Size(242, 15);
        this.infoLabel.TabIndex = 8;
        this.infoLabel.Text = "Make private assemblies public for modding";
        // 
        // dropPanel
        // 
        this.dropPanel.AllowDrop = true;
        this.dropPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
        this.dropPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.dropPanel.Controls.Add(this.dropLabel);
        this.dropPanel.Location = new System.Drawing.Point(20, 100);
        this.dropPanel.Name = "dropPanel";
        this.dropPanel.Size = new System.Drawing.Size(510, 180);
        this.dropPanel.TabIndex = 0;
        this.dropPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropPanel_DragDrop);
        this.dropPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.dropPanel_DragEnter);
        // 
        // dropLabel
        // 
        this.dropLabel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dropLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.dropLabel.ForeColor = System.Drawing.Color.White;
        this.dropLabel.Location = new System.Drawing.Point(0, 0);
        this.dropLabel.Name = "dropLabel";
        this.dropLabel.Size = new System.Drawing.Size(508, 178);
        this.dropLabel.TabIndex = 0;
        this.dropLabel.Text = "Drag and drop a DLL file here";
        this.dropLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // stripMethodsCheckBox
        //
        this.stripMethodsCheckBox.AutoSize = true;
        this.stripMethodsCheckBox.Checked = true;
        this.stripMethodsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
        this.stripMethodsCheckBox.Location = new System.Drawing.Point(20, 295);
        this.stripMethodsCheckBox.Name = "stripMethodsCheckBox";
        this.stripMethodsCheckBox.Size = new System.Drawing.Size(304, 19);
        this.stripMethodsCheckBox.TabIndex = 6;
        this.stripMethodsCheckBox.Text = "Strip proprietary code (keep method signatures only)";
        this.stripMethodsCheckBox.UseVisualStyleBackColor = true;
        this.stripMethodsCheckBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.stripMethodsCheckBox.ForeColor = System.Drawing.Color.LightGray;
        // 
        // openFileButton
        // 
        this.openFileButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
        this.openFileButton.FlatAppearance.BorderSize = 0;
        this.openFileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.openFileButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.openFileButton.ForeColor = System.Drawing.Color.White;
        this.openFileButton.Location = new System.Drawing.Point(380, 290);
        this.openFileButton.Name = "openFileButton";
        this.openFileButton.Size = new System.Drawing.Size(150, 30);
        this.openFileButton.TabIndex = 2;
        this.openFileButton.Text = "Browse for DLL...";
        this.openFileButton.UseVisualStyleBackColor = false;
        this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
        // 
        // logLabel
        // 
        this.logLabel.AutoSize = true;
        this.logLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.logLabel.ForeColor = System.Drawing.Color.LightGray;
        this.logLabel.Location = new System.Drawing.Point(20, 330);
        this.logLabel.Name = "logLabel";
        this.logLabel.Size = new System.Drawing.Size(89, 17);
        this.logLabel.TabIndex = 5;
        this.logLabel.Text = "Process Log:";
        // 
        // logTextBox
        // 
        this.logTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
        this.logTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.logTextBox.ForeColor = System.Drawing.Color.LightGray;
        this.logTextBox.Location = new System.Drawing.Point(20, 350);
        this.logTextBox.Multiline = true;
        this.logTextBox.Name = "logTextBox";
        this.logTextBox.ReadOnly = true;
        this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.logTextBox.Size = new System.Drawing.Size(510, 160);
        this.logTextBox.TabIndex = 4;
        this.logTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        // 
        // bottomPanel
        // 
        this.bottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
        this.bottomPanel.Controls.Add(this.versionLabel);
        this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.bottomPanel.Location = new System.Drawing.Point(0, 525);
        this.bottomPanel.Name = "bottomPanel";
        this.bottomPanel.Size = new System.Drawing.Size(550, 25);
        this.bottomPanel.TabIndex = 9;
        // 
        // versionLabel
        // 
        this.versionLabel.AutoSize = true;
        this.versionLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.versionLabel.ForeColor = System.Drawing.Color.LightGray;
        this.versionLabel.Location = new System.Drawing.Point(20, 6);
        this.versionLabel.Name = "versionLabel";
        this.versionLabel.Size = new System.Drawing.Size(78, 13);
        this.versionLabel.TabIndex = 10;
        this.versionLabel.Text = "Version 1.0.0";
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
        this.ClientSize = new System.Drawing.Size(550, 550);
        this.Controls.Add(this.bottomPanel);
        this.Controls.Add(this.headerPanel);
        this.Controls.Add(this.stripMethodsCheckBox);
        this.Controls.Add(this.logLabel);
        this.Controls.Add(this.logTextBox);
        this.Controls.Add(this.openFileButton);
        this.Controls.Add(this.dropPanel);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Public1ty";
        this.dropPanel.ResumeLayout(false);
        this.headerPanel.ResumeLayout(false);
        this.headerPanel.PerformLayout();
        this.bottomPanel.ResumeLayout(false);
        this.bottomPanel.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Panel dropPanel;
    private System.Windows.Forms.Label dropLabel;
    private System.Windows.Forms.Button openFileButton;
    private System.Windows.Forms.Label titleLabel;
    private System.Windows.Forms.TextBox logTextBox;
    private System.Windows.Forms.Label logLabel;
    private System.Windows.Forms.CheckBox stripMethodsCheckBox;
    private System.Windows.Forms.Panel headerPanel;
    private System.Windows.Forms.Label infoLabel;
    private System.Windows.Forms.Panel bottomPanel;
    private System.Windows.Forms.Label versionLabel;
}

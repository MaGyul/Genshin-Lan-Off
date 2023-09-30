﻿namespace Genshin_Lan_Off
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.panel1 = new System.Windows.Forms.Panel();
            this.editBtn = new System.Windows.Forms.Button();
            this.keyBox = new System.Windows.Forms.TextBox();
            this._key_label = new System.Windows.Forms.Label();
            this.applyBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.firewallName = new System.Windows.Forms.TextBox();
            this._firewall_label = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.editBtn);
            this.panel1.Controls.Add(this.keyBox);
            this.panel1.Controls.Add(this._key_label);
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.MaximumSize = new System.Drawing.Size(200, 75);
            this.panel1.MinimumSize = new System.Drawing.Size(200, 75);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 75);
            this.panel1.TabIndex = 0;
            // 
            // editBtn
            // 
            this.editBtn.Location = new System.Drawing.Point(7, 45);
            this.editBtn.MaximumSize = new System.Drawing.Size(85, 25);
            this.editBtn.MinimumSize = new System.Drawing.Size(85, 25);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(85, 25);
            this.editBtn.TabIndex = 2;
            this.editBtn.Text = "변경";
            this.editBtn.UseVisualStyleBackColor = true;
            this.editBtn.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Settings_KeyPress);
            // 
            // keyBox
            // 
            this.keyBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keyBox.Location = new System.Drawing.Point(3, 18);
            this.keyBox.MaximumSize = new System.Drawing.Size(193, 21);
            this.keyBox.MinimumSize = new System.Drawing.Size(193, 21);
            this.keyBox.Name = "keyBox";
            this.keyBox.ReadOnly = true;
            this.keyBox.Size = new System.Drawing.Size(193, 21);
            this.keyBox.TabIndex = 1;
            this.keyBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Settings_KeyPress);
            // 
            // _key_label
            // 
            this._key_label.AutoSize = true;
            this._key_label.Location = new System.Drawing.Point(3, 3);
            this._key_label.Name = "_key_label";
            this._key_label.Size = new System.Drawing.Size(97, 12);
            this._key_label.TabIndex = 0;
            this._key_label.Text = "랜뽑 실행 단축키";
            // 
            // applyBtn
            // 
            this.applyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.applyBtn.Location = new System.Drawing.Point(7, 2);
            this.applyBtn.MaximumSize = new System.Drawing.Size(85, 25);
            this.applyBtn.MinimumSize = new System.Drawing.Size(85, 25);
            this.applyBtn.Name = "applyBtn";
            this.applyBtn.Size = new System.Drawing.Size(85, 25);
            this.applyBtn.TabIndex = 1;
            this.applyBtn.Text = "적용";
            this.applyBtn.UseVisualStyleBackColor = true;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.Location = new System.Drawing.Point(107, 2);
            this.cancelBtn.MaximumSize = new System.Drawing.Size(85, 25);
            this.cancelBtn.MinimumSize = new System.Drawing.Size(85, 25);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(85, 25);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "취소";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.firewallName);
            this.panel2.Controls.Add(this._firewall_label);
            this.panel2.Location = new System.Drawing.Point(5, 86);
            this.panel2.MaximumSize = new System.Drawing.Size(200, 45);
            this.panel2.MinimumSize = new System.Drawing.Size(200, 45);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 45);
            this.panel2.TabIndex = 3;
            // 
            // firewallName
            // 
            this.firewallName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.firewallName.Location = new System.Drawing.Point(3, 18);
            this.firewallName.MaximumSize = new System.Drawing.Size(193, 21);
            this.firewallName.MinimumSize = new System.Drawing.Size(193, 21);
            this.firewallName.Name = "firewallName";
            this.firewallName.Size = new System.Drawing.Size(193, 21);
            this.firewallName.TabIndex = 1;
            this.firewallName.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Settings_KeyPress);
            // 
            // _firewall_label
            // 
            this._firewall_label.AutoSize = true;
            this._firewall_label.Location = new System.Drawing.Point(3, 3);
            this._firewall_label.Name = "_firewall_label";
            this._firewall_label.Size = new System.Drawing.Size(97, 12);
            this._firewall_label.TabIndex = 0;
            this._firewall_label.Text = "방화벽 규칙 이름";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.applyBtn);
            this.panel3.Controls.Add(this.cancelBtn);
            this.panel3.Location = new System.Drawing.Point(5, 137);
            this.panel3.MaximumSize = new System.Drawing.Size(200, 30);
            this.panel3.MinimumSize = new System.Drawing.Size(200, 30);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 30);
            this.panel3.TabIndex = 4;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(209, 171);
            this.ControlBox = false;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(225, 210);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(225, 210);
            this.Name = "Settings";
            this.ShowIcon = false;
            this.Text = "설정";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox keyBox;
        private System.Windows.Forms.Label _key_label;
        private System.Windows.Forms.Button editBtn;
        private System.Windows.Forms.Button applyBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox firewallName;
        private System.Windows.Forms.Label _firewall_label;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
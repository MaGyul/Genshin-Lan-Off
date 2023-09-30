namespace Genshin_Lan_Off
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tray = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.statusBox = new System.Windows.Forms.ToolStripMenuItem();
            this.catchBox = new System.Windows.Forms.ToolStripMenuItem();
            this.shotBox = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.settings = new System.Windows.Forms.ToolStripMenuItem();
            this.exit = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tray
            // 
            this.tray.ContextMenuStrip = this.contextMenu;
            this.tray.Icon = ((System.Drawing.Icon)(resources.GetObject("tray.Icon")));
            this.tray.Text = "원신 랜뽑";
            this.tray.Visible = true;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBox,
            this.catchBox,
            this.shotBox,
            this._toolStripSeparator,
            this.settings,
            this.exit});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(147, 120);
            // 
            // statusBox
            // 
            this.statusBox.Enabled = false;
            this.statusBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(146, 22);
            this.statusBox.Text = "상태: None";
            this.statusBox.Visible = false;
            // 
            // catchBox
            // 
            this.catchBox.Enabled = false;
            this.catchBox.Name = "catchBox";
            this.catchBox.Size = new System.Drawing.Size(146, 22);
            this.catchBox.Text = "방화벽: None";
            this.catchBox.Visible = false;
            // 
            // shotBox
            // 
            this.shotBox.Enabled = false;
            this.shotBox.Name = "shotBox";
            this.shotBox.Size = new System.Drawing.Size(146, 22);
            this.shotBox.Text = "단축키: None";
            this.shotBox.Visible = false;
            // 
            // _toolStripSeparator
            // 
            this._toolStripSeparator.Name = "_toolStripSeparator";
            this._toolStripSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // settings
            // 
            this.settings.Name = "settings";
            this.settings.Size = new System.Drawing.Size(146, 22);
            this.settings.Text = "설정";
            // 
            // exit
            // 
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(146, 22);
            this.exit.Text = "종료";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(120, 0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Form";
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon tray;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripSeparator _toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem settings;
        private System.Windows.Forms.ToolStripMenuItem exit;
        private System.Windows.Forms.ToolStripMenuItem statusBox;
        private System.Windows.Forms.ToolStripMenuItem catchBox;
        private System.Windows.Forms.ToolStripMenuItem shotBox;
    }
}


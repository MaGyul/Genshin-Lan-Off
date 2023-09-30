using NetFwTypeLib;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Genshin_Lan_Off
{
    public partial class Settings : Form
    {
        private bool editKey = false;
        private Keys shotKey;
        private bool shift = false;
        private bool ctrl = false;
        private bool alt = false;

        private bool editBtnKeyInput = false;

        public Settings()
        {
            InitializeComponent();

            shotKey = Program.ramShotKey = (Keys)Program.settingsReg.GetValue("shot_key", 0);
            shift = Program.ramShift = (int)Program.settingsReg.GetValue("shot_shift", 0) == 1;
            ctrl = Program.ramCtrl = (int)Program.settingsReg.GetValue("shot_ctrl", 0) == 1;
            alt = Program.ramAlt = (int)Program.settingsReg.GetValue("shot_alt", 0) == 1;
            firewallName.Text = Program.settingsReg.GetValue("firewall_name", "").ToString();

            try {
                Program.UpdateFireWall(firewallName.Text);
            } catch (MethodAccessException) {}

            Program.Update();

            keyBox.Text = shotKey.ToString();

            editBtn.Click += EditBtn_Click;
            cancelBtn.Click += CancelBtn_Click;
            applyBtn.Click += ApplyBtn_Click;
        }

        private void Apply()
        {
            editKey = false;
            if (string.IsNullOrEmpty(firewallName.Text))
            {
                MessageBox.Show(this, "방화벽 규칙 이름이 비어있습니다", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (Program.UpdateFireWall(firewallName.Text))
                {
                    Program.settingsReg.SetValue("shot_key", (int)shotKey, Microsoft.Win32.RegistryValueKind.DWord);
                    Program.settingsReg.SetValue("shot_shift", shift ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord);
                    Program.settingsReg.SetValue("shot_ctrl", ctrl ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord);
                    Program.settingsReg.SetValue("shot_alt", alt ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord);
                    Program.settingsReg.SetValue("firewall_name", firewallName.Text, Microsoft.Win32.RegistryValueKind.String);
                    Program.ramShotKey = shotKey;
                    Program.ramShift = shift;
                    Program.ramCtrl = ctrl;
                    Program.ramAlt = alt;
                    Program.Update();
                    Close();
                }
                else
                {
                    var result = MessageBox.Show(this, $"{firewallName.Text}라는 방화벽 규칙을 찾을 수 없습니다.\n방화벽 규칙을 생성할까요?", "방화벽 규칙 찾기 실패", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        var fdb = new FolderBrowserDialog();
                        fdb.ShowNewFolderButton = false;
                        fdb.RootFolder = Environment.SpecialFolder.MyComputer;
                        fdb.Description = "원신 게임이 있는 폴더를 선택해주세요.\nex) C:\\Program Files\\Genshin Impact\\Genshin Impact Game";
                        if (fdb.ShowDialog() == DialogResult.OK)
                        {
                            var path = fdb.SelectedPath;
                            Console.WriteLine(string.Join(", ", Directory.GetFiles(path)));
                            if (Directory.GetFiles(path).Any(file => file.EndsWith("GenshinImpact.exe")))
                            {
                                var gameExe = path + "\\GenshinImpact.exe";
                                Console.WriteLine(gameExe);
                                if (File.Exists(gameExe))
                                {
                                    var rule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));

                                    rule.Name = firewallName.Text;
                                    rule.Description = "원신 랜뽑 프로그램이 생성한 규칙";
                                    rule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
                                    rule.Enabled = false;
                                    rule.ApplicationName = gameExe;
                                    rule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                                    rule.Profiles = 2147483647;

                                    Program.AddFirewall(rule);

                                    MessageBox.Show(this, "방화벽 규칙을 생성헀습니다.", "생성", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    Apply();
                                    return;
                                }
                            }
                            MessageBox.Show(this, "해당 폴더에서 GenshinImpact.exe를 찾을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            } catch (MethodAccessException e)
            {
                MessageBox.Show(this, e.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private new void Close()
        {
            editKey = false;
            base.Close();
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (editBtnKeyInput)
            {
                toolTip.Show("변경 버튼은 마우스로 조작해주세요.", editBtn, 1000);
                editBtnKeyInput = false;
                return;
            }

            if (editKey)
            {
                _key_label.Text = "실행 단축키";
                editBtn.Text = "변경";
                editKey = false;
            }
            else
            {
                _key_label.Text = "실행 단축키 (키 인식 중)";
                editBtn.Text = "완료";
                editKey = true;
            }
        }

        public void onKey(Keys key, bool shift, bool ctrl, bool alt, bool keyup)
        {
            if (editKey)
            {
                var text = "";
                if (ctrl) text += "Ctrl + ";
                if (alt) text += "Alt + ";
                if (shift) text += "Shift + ";
                text += key.ToString();
                keyBox.Text = text;
                shotKey = key;
                this.shift = shift;
                this.ctrl = ctrl;
                this.alt = alt;
                if (keyup)
                {
                    _key_label.Text = "실행 단축키";
                    editBtn.Text = "변경";
                    editKey = false;
                }
            }
        }

        private void Settings_KeyPress(object sender, PreviewKeyDownEventArgs e)
        {
            if (sender == editBtn)
            {
                editBtnKeyInput = true;
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                Apply();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}

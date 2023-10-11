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

        public Settings()
        {
            InitializeComponent();

            var ini = new IniFile();

            if (File.Exists(GetInIPath()))
            {
                ini.Load(GetInIPath());
            }

            shotKey = Program.ramShotKey = (Keys)ini["shotKey"]["key"].ToInt(0);
            shift = Program.ramShift = ini["shotKey"]["shift"].ToBool(false);
            ctrl = Program.ramCtrl = ini["shotKey"]["ctrl"].ToBool(false);
            alt = Program.ramAlt = ini["shotKey"]["alt"].ToBool(false);
            showNoti.Checked = Program.ramShowNoti = ini["noti"]["show"].ToBool(true);
            firewallName.Text = ini["firewall"]["name"].GetString();

            try {
                if (Program.UpdateFireWall(firewallName.Text)) Program.Update();
            } catch (MethodAccessException) {}

            var text = "";
            if (ctrl) text += "Ctrl + ";
            if (alt) text += "Alt + ";
            if (shift) text += "Shift + ";
            text += shotKey.ToString();
            keyBox.Text = text;

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
                    var ini = new IniFile();

                    ini["shotKey"]["key"] = (int)shotKey;
                    ini["shotKey"]["shift"] = shift;
                    ini["shotKey"]["ctrl"] = ctrl;
                    ini["shotKey"]["alt"] = alt;
                    ini["noti"]["show"] = showNoti.Checked;
                    ini["firewall"]["name"] = firewallName.Text;
                    Program.ramShowNoti = showNoti.Checked;
                    Program.ramShotKey = shotKey;
                    Program.ramShift = shift;
                    Program.ramCtrl = ctrl;
                    Program.ramAlt = alt;
                    Program.Update();

                    try
                    {
                        ini.Save(GetInIPath());
                    } 
                    catch (IOException)
                    {
                        MessageBox.Show(this, "설정 파일을 저장하지 못했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
            if (editKey)
            {
                if (shotKey == Keys.None)
                {
                    MessageBox.Show(this, "키 설정이 완료되지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _key_label.Text = "실행 단축키";
                editBtn.Text = "변경";
                editKey = false;
            }
            else
            {
                keyBox.Text = "";
                _key_label.Text = "실행 단축키 (키 인식 중)";
                editBtn.Text = "완료";
                editKey = true;
            }
        }

        public void onKey(KeyEventArgs e, bool keyup)
        {
            if (editKey)
            {
                var is_sp = false;
                var ctrl = e.Control;
                var alt = e.Alt;
                var shift = e.Shift;
                var key = e.KeyCode;
                if (key == Keys.LShiftKey || key == Keys.LControlKey || key == Keys.LMenu)
                {
                    shotKey = Keys.None;
                    is_sp = true;
                }

                var text = "";
                if (ctrl) text += "Ctrl + ";
                if (alt) text += "Alt + ";
                if (shift) text += "Shift + ";
                if (keyup && !is_sp)
                {
                    text += key.ToString();
                    editKey = false;
                    shotKey = key;
                    this.shift = shift;
                    this.ctrl = ctrl;
                    this.alt = alt;
                    _key_label.Text = "실행 단축키";
                    editBtn.Text = "변경";
                    keyBox.Text = text;
                }
                else keyBox.Text = text;
            }
        }

        private void Settings_KeyPress(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Apply();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private string GetInIPath()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path = appData + "\\GLO";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + "\\settings.ini";
        }
    }
}

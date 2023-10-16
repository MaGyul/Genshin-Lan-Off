using Genshin_LAN_Off.Properties;
using NetFwTypeLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Genshin_Lan_Off
{
    public static class Program
    {
        private static NotifyIcon tray;
        private static ContextMenuStrip contextMenu;
        private static ToolStripMenuItem settings;
        private static ToolStripMenuItem exit;
        private static ToolStripMenuItem statusBox;
        private static ToolStripMenuItem catchBox;
        private static ToolStripMenuItem shotBox;
        private static ToolStripMenuItem notiBox;

        private static readonly KeyboardHook kHook = new KeyboardHook();
        private static readonly INetFwPolicy2 policy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

        private static Settings settingsForm;
        private static INetFwRule fwRule = null;

        public static Keys ramShotKey = Keys.None;
        public static bool ramShift = false;
        public static bool ramCtrl = false;
        public static bool ramAlt = false;
        public static bool ramShowNoti = false;

        private static bool disposed = false;

        [STAThread]
        static void Main()
        {
            Console.WriteLine("[Info] Program Started, Checking Administrator...");
            if (IsAdministrator() == false)
            {
                /*
                Console.WriteLine("[Warn-Run Administrator] No Administrator, Running Administrator...");
                try
                {
                    ProcessStartInfo procInfo = new ProcessStartInfo
                    {
                        UseShellExecute = true,
                        FileName = Application.ExecutablePath,
                        WorkingDirectory = Environment.CurrentDirectory,
                        Verb = "runas"
                    };
                    Process.Start(procInfo);
                    Console.WriteLine("[Info-Run Administrator] Successful!");
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"[Error-Run Administrator] Failed! ({ex.Message})");
                }
                */
                Console.WriteLine("[Warn] No Administrator");
                MessageBox.Show("관리자 권한으로 실행해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Console.WriteLine("[Info-Firewall] Checking Firewall...");

            if (!FireWallEnabled())
            {
                Console.WriteLine("[Warn-Firewall] Firewall is disabled, Enable it.");
                var result = MessageBox.Show("원신 방화벽 핫키는 방화벽을 이용해서 원신 인터넷을 차단합니다\n방화벽을 켜시곘습니까?", "방화벽 꺼져있음", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (FireWallEnabled(true))
                    {
                        Console.WriteLine("[Info-Firewall] Firewall enable successful!");
                        MessageBox.Show("방화벽을 켰습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } 
                    else
                    {
                        Console.WriteLine("[Error-Firewall] Firewall enable failed!");
                        MessageBox.Show("방화벽을 켤수없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("[Error-Firewall] Firewall is not available!");
                    MessageBox.Show("방화벽을 사용할 수 없으므로 프로그램을 종료합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            Console.WriteLine("[Info-UI] Init UI...");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            InitializeComponent();

            settingsForm = new Settings();

            Application.ApplicationExit += (object sender, EventArgs args) =>
            {
                if (!disposed) Dispose();
            };

            tray.MouseClick += (object sender, MouseEventArgs args) =>
            {
                if (args.Button == MouseButtons.Left)
                {
                    var method = tray.GetType().GetMethod("ShowContextMenu", BindingFlags.NonPublic | BindingFlags.Instance);
                    method.Invoke(tray, new object[] { });
                }
            };
            exit.Click += (object sender, EventArgs args) =>
            {
                Dispose();
                Application.Exit();
                Application.ExitThread();
            };
            settings.Click += (object sender, EventArgs args) =>
            {
                settingsForm.ShowDialog();
            };
            Console.WriteLine("[Info-UI] Successful!");

            Console.WriteLine("[Info-HK] Hooking keyboard...");
            kHook.Hook();
            kHook.KeyUp += KHook_KeyUp;
            kHook.KeyDown += KHook_KeyDown;
            Console.WriteLine("[Info-HK] Successful!");
            ShowNoti("원신 방화벽 핫키", "원신 방화벽 핫키가 실행되었습니다.\n트레이에서 보실 수 있습니다.");

            //(new Form1()).Show();

            Console.WriteLine("[Info] Done.");
            Application.Run();
        }

        private static bool FireWallEnabled(bool enable = false)
        {
            var enabled = policy.FirewallEnabled[NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC] && policy.FirewallEnabled[NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE];

            if (enable && !enabled)
            {
                policy.FirewallEnabled[NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE] = true;
                policy.FirewallEnabled[NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC] = true;
                enabled = policy.FirewallEnabled[NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC] && policy.FirewallEnabled[NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE];
            }

            return enabled;
        }

        public static void AddFirewall(INetFwRule rule)
        {
            policy.Rules.Add(rule);
            Console.WriteLine($"[Info-UI] Added firewall ({rule.Name})");
        }

        public static bool UpdateFireWall(string name)
        {
            var rules = policy.Rules.Cast<INetFwRule>();

            if (!rules.Any(rule => rule.Name == name))
            {
                Console.WriteLine($"[Warn-UI] Firewall not found ({name})");
                return false;
            }

            fwRule = rules.FirstOrDefault(rule => rule.Name == name);

            if (fwRule.Direction != NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT) throw new MethodAccessException("아웃바이트 규칙이 아닙니다.");
            if (fwRule.Action != NET_FW_ACTION_.NET_FW_ACTION_BLOCK) throw new MethodAccessException("인터넷 차단을 위해 방화벽 규칙이 연결 차단이여야 합니다.");
            if (!fwRule.ApplicationName.Contains("GenshinImpact.exe")) throw new MethodAccessException("해당 방화벽 규칙은 GenshinImpact.exe으로 지정되어 있지 않습니다.");

            Console.WriteLine($"[Info-UI] Firewall updated ({name})");
            return true;
        }

        public static void Update()
        {
            if (!statusBox.Visible) statusBox.Visible = true;
            if (!catchBox.Visible) catchBox.Visible = true;
            if (!shotBox.Visible) shotBox.Visible = true;
            if (!notiBox.Visible) notiBox.Visible = true;

            if (fwRule != null)
            {
                statusBox.Text = $"상태: {(fwRule.Enabled ? "활성화" : "비활성화")}";
                catchBox.Text = $"방화벽: {fwRule.Name}";
            }
            var text = "";
            if (ramCtrl) text += "Ctrl + ";
            if (ramAlt) text += "Alt + ";
            if (ramShift) text += "Shift + ";
            text += ramShotKey.ToString();
            shotBox.Text = $"단축키: {text}";
            notiBox.Text = $"알림: {(ramShowNoti ? "활성화" : "비활성화")}";
            Console.WriteLine("[Info-UI] Tray updated");
        }

        private static void KHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (settingsForm.Visible)
            {
                settingsForm.OnKey(e, false);
            }
        }

        private static void KHook_KeyUp(object sender, KeyEventArgs e)
        {
            if (settingsForm.Visible)
            {
                settingsForm.OnKey(e, true);
            }
            else
            {
                if (e.Control == ramCtrl &&
                    e.Shift == ramShift &&
                    e.Alt == ramAlt &&
                    e.KeyCode == ramShotKey)
                {
                    if (fwRule != null)
                    {
                        if (fwRule.Enabled)
                        {
                            Console.WriteLine("[Info-Firewall] Firewall disable");
                            fwRule.Enabled = false;
                            ShowNoti("상태", "방화벽 규칙이 비활성화 되었습니다.");
                        }
                        else
                        {
                            Console.WriteLine("[Info-Firewall] Firewall enable");
                            fwRule.Enabled = true;
                            ShowNoti("상태", "방화벽 규칙이 활성화 되었습니다.");
                        }
                        statusBox.Text = $"상태: {(fwRule.Enabled ? "활성화" : "비활성화")}";
                        Console.WriteLine("[Info-UI] Tray updated");
                    }
                }
            }
        }

        private static void ShowNoti(string title, string text, ToolTipIcon icon = ToolTipIcon.Info)
        {
            if (ramShowNoti)
            {
                tray.BalloonTipTitle = title;
                tray.BalloonTipText = text;
                tray.BalloonTipIcon = icon;
                tray.ShowBalloonTip(0);
            }
        }

        private static void InitializeComponent()
        {
            tray = new NotifyIcon();
            contextMenu = new ContextMenuStrip();
            statusBox = new ToolStripMenuItem();
            catchBox = new ToolStripMenuItem();
            shotBox = new ToolStripMenuItem();
            notiBox = new ToolStripMenuItem();
            settings = new ToolStripMenuItem();
            exit = new ToolStripMenuItem();
            // 
            // tray
            // 
            tray.ContextMenuStrip = contextMenu;
            tray.Icon = Resources.IconGroup103;
            tray.Text = "원신 방화벽 핫키";
            tray.Visible = true;
            // 
            // statusBox
            // 
            statusBox.Enabled = false;
            statusBox.Visible = false;
            // 
            // catchBox
            // 
            catchBox.Enabled = false;
            catchBox.Visible = false;
            // 
            // shotBox
            // 
            shotBox.Enabled = false;
            shotBox.Visible = false;
            // 
            // notiBox
            // 
            notiBox.Enabled = false;
            notiBox.Visible = false;
            // 
            // settings
            // 
            settings.Text = "설정";
            // 
            // exit
            // 
            exit.Text = "종료";
            // 
            // contextMenu
            // 
            contextMenu.Items.AddRange(new ToolStripItem[] {
            statusBox,
            catchBox,
            shotBox,
            notiBox,
            new ToolStripSeparator(),
            settings,
            exit});
        }

        private static void Dispose()
        {
            statusBox.Dispose();
            catchBox.Dispose();
            shotBox.Dispose();
            notiBox.Dispose();
            settings.Dispose();
            exit.Dispose();
            contextMenu.Dispose();
            tray.Dispose();

            kHook.Unhook();
            if (fwRule != null && fwRule.Enabled)
            {
                fwRule.Enabled = false;
            }
            disposed = true;
        }

        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            if (null != identity)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            return false;
        }
    }
}

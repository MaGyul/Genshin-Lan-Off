using Microsoft.Win32;
using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Genshin_Lan_Off
{
    public partial class MainForm : Form
    {
        private static INetFwPolicy2 policy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

        public RegistryKey settingsReg;
        private Settings settingsForm;
        private INetFwRule fwRule = null;

        public Keys ramShotKey = Keys.None;
        public bool ramShift = false;
        public bool ramCtrl = false;
        public bool ramAlt = false;
        public Dictionary<Keys, bool> spKeys = new Dictionary<Keys, bool>();

        public MainForm()
        {
            InitializeComponent();
            ShowInTaskbar = false; Visible = false; WindowState = FormWindowState.Minimized;
            spKeys[Keys.LShiftKey] = spKeys[Keys.LControlKey] = spKeys[Keys.LMenu] = false;

            settingsReg = Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("GenshinLanOff");

            //settingsForm = new Settings(this);

            Application.ApplicationExit += (object sender, EventArgs args) =>
            {
                KeyboardHook.UnHook();
                RegClose();
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
                KeyboardHook.UnHook();
                RegClose();
                Application.Exit();
            };
            settings.Click += (object sender, EventArgs args) =>
            {
                settingsForm.ShowDialog();
            };

            FormClosing += Form_Closing;
            Load += Form_Load;
        }

        public bool UpdateFireWall(string name)
        {
            var rules = policy.Rules.Cast<INetFwRule>();

            if (!rules.Any(rule => rule.Name == name)) return false;

            fwRule = rules.FirstOrDefault(rule => rule.Name == name);

            statusBox.Visible = true;
            catchBox.Visible = true;
            shotBox.Visible = true;

            statusBox.Text = $"상태: {(fwRule.Enabled ? "활성화" : "비활성화")}";
            catchBox.Text = $"방화벽: {fwRule.Name}";
            var text = "";
            if (ramCtrl) text += "Ctrl + ";
            if (ramAlt) text += "Alt + ";
            if (ramShift) text += "Shift + ";
            text += ramShotKey.ToString();
            shotBox.Text = $"단축키: {text}";

            return true;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            KeyboardHook.SetHook(KeyboardHookProc);
            ShowNoti("원신 랜뽑", "원신 랜뽑이 실행되었습니다.\n트레이에서 보실 수 있습니다.");
        }

        private IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                Keys key = KeyboardHook.ReadKey(lParam);
                if (wParam == (IntPtr)KeyboardHook.WM_KEYUP)
                {
                    if (key == Keys.LControlKey || key == Keys.LMenu || key == Keys.LShiftKey)
                    {
                        spKeys[key] = false;
                        return KeyboardHook.NextHook(code, (int)wParam, lParam);
                    }

                    if (settingsForm.Visible)
                    {
                        settingsForm.onKey(key, spKeys[Keys.LShiftKey], spKeys[Keys.LControlKey], spKeys[Keys.LMenu], true);
                    }
                    else
                    {
                        if (spKeys[Keys.LControlKey] == ramCtrl &&
                            spKeys[Keys.LShiftKey] == ramShift &&
                            spKeys[Keys.LMenu] == ramAlt &&
                            key == ramShotKey)
                        {
                            if (fwRule != null)
                            {
                                if (fwRule.Enabled)
                                {
                                    fwRule.Enabled = false;
                                    ShowNoti("랜뽑 상태", "랜뽑이 비활성화 되었습니다.");
                                }
                                else
                                {
                                    fwRule.Enabled = true;
                                    ShowNoti("랜뽑 상태", "랜뽑이 활성화 되었습니다.");
                                }
                                statusBox.Text = $"상태: {(fwRule.Enabled ? "활성화" : "비활성화")}";
                            }
                        }
                    }
                }
                else if (wParam == (IntPtr)KeyboardHook.WM_KEYDOWN || wParam == (IntPtr)KeyboardHook.WM_SYSKEYDOWN)
                {
                    if (key == Keys.LControlKey || key == Keys.LMenu || key == Keys.LShiftKey)
                    {
                        spKeys[key] = true;
                        if (settingsForm.Visible)
                        {
                            settingsForm.onKey(Keys.None, spKeys[Keys.LShiftKey], spKeys[Keys.LControlKey], spKeys[Keys.LMenu], false);
                        }
                    }
                }
                return KeyboardHook.NextHook(code, (int)wParam, lParam);
            }
            else
            {
                return KeyboardHook.NextHook(code, (int)wParam, lParam);
            }
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            KeyboardHook.UnHook();
            RegClose();
        }

        private void RegClose()
        {
            if (settingsReg == null) return;

            settingsReg.Close();
        }

        private void ShowNoti(string title, string text, ToolTipIcon icon = ToolTipIcon.Info)
        {
            tray.BalloonTipTitle = title;
            tray.BalloonTipText = text;
            tray.BalloonTipIcon = icon;
            tray.ShowBalloonTip(0);
        }
    }
}

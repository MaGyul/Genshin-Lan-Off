﻿using Genshin_LAN_Off.Properties;
using Microsoft.Win32;
using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Genshin_Lan_Off
{
    public static class Program
    {
        private static INetFwPolicy2 policy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

        public static RegistryKey settingsReg;
        private static Settings settingsForm;
        private static INetFwRule fwRule = null;

        public static Keys ramShotKey = Keys.None;
        public static bool ramShift = false;
        public static bool ramCtrl = false;
        public static bool ramAlt = false;
        public static Dictionary<Keys, bool> spKeys = new Dictionary<Keys, bool>();

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!FireWallEnabled())
            {
                var result = MessageBox.Show("원신 랜뽑은 방화벽을 이용해서 원신 인터넷을 차단합니다\n방화벽을 켜시곘습니까?", "방화벽 꺼져있음", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    FireWallEnabled(true);
                    MessageBox.Show("방화벽을 켰습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("방화벽을 사용할 수 없으므로 프로그램을 종료합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            InitializeComponent();

            spKeys[Keys.LShiftKey] = spKeys[Keys.LControlKey] = spKeys[Keys.LMenu] = false;

            settingsReg = Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("GenshinLanOff");

            settingsForm = new Settings();

            Application.ApplicationExit += (object sender, EventArgs args) =>
            {
                KeyboardHook.UnHook();
                RegClose();
                components.Dispose();
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
                Application.Exit();
            };
            settings.Click += (object sender, EventArgs args) =>
            {
                settingsForm.ShowDialog();
            };

            KeyboardHook.SetHook(KeyboardHookProc);
            ShowNoti("원신 랜뽑", "원신 랜뽑이 실행되었습니다.\n트레이에서 보실 수 있습니다.");

            Application.Run();
        }

        private static bool FireWallEnabled(bool enable = false)
        {
            var enabled = policy.FirewallEnabled[NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC] && policy.FirewallEnabled[NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE];

            if (enable && !enabled)
            {
                policy.FirewallEnabled[NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE] = true;
                policy.FirewallEnabled[NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC] = true;
            }

            return enabled;
        }

        public static void AddFirewall(INetFwRule rule)
        {
            policy.Rules.Add(rule);
        }

        public static bool UpdateFireWall(string name)
        {
            var rules = policy.Rules.Cast<INetFwRule>();

            if (!rules.Any(rule => rule.Name == name)) return false;

            fwRule = rules.FirstOrDefault(rule => rule.Name == name);

            if (fwRule.Direction != NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT) throw new MethodAccessException("아웃바이트 규칙이 아닙니다.");
            if (fwRule.Action != NET_FW_ACTION_.NET_FW_ACTION_BLOCK) throw new MethodAccessException("원신 랜뽑을 위해 방화벽 규칙이 연결 차단이여야 합니다.");
            if (!fwRule.ApplicationName.Contains("GenshinImpact.exe")) throw new MethodAccessException("해당 방화벽 규칙은 GenshinImpact.exe으로 지정되어 있지 않습니다.");

            return true;
        }

        public static void Update()
        {
            statusBox.Visible = true;
            catchBox.Visible = true;
            shotBox.Visible = true;

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
        }

        private static IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam)
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

        private static void RegClose()
        {
            if (settingsReg == null) return;

            settingsReg.Close();
        }

        private static void ShowNoti(string title, string text, ToolTipIcon icon = ToolTipIcon.Info)
        {
            tray.BalloonTipTitle = title;
            tray.BalloonTipText = text;
            tray.BalloonTipIcon = icon;
            tray.ShowBalloonTip(0);
        }

        private static void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tray = new NotifyIcon(components);
            contextMenu = new ContextMenuStrip(components);
            statusBox = new ToolStripMenuItem();
            catchBox = new ToolStripMenuItem();
            shotBox = new ToolStripMenuItem();
            _toolStripSeparator = new ToolStripSeparator();
            settings = new ToolStripMenuItem();
            exit = new ToolStripMenuItem();
            contextMenu.SuspendLayout();
            // 
            // tray
            // 
            tray.ContextMenuStrip = contextMenu;
            tray.Icon = Resources.IconGroup103;
            tray.Text = "원신 랜뽑";
            tray.Visible = true;
            // 
            // contextMenu
            // 
            contextMenu.Items.AddRange(new ToolStripItem[] {
            statusBox,
            catchBox,
            shotBox,
            _toolStripSeparator,
            settings,
            exit});
            contextMenu.Name = "contextMenu";
            contextMenu.Size = new System.Drawing.Size(147, 120);
            // 
            // statusBox
            // 
            statusBox.Enabled = false;
            statusBox.ForeColor = System.Drawing.SystemColors.ControlText;
            statusBox.Name = "statusBox";
            statusBox.Size = new System.Drawing.Size(146, 22);
            statusBox.Text = "상태: None";
            statusBox.Visible = false;
            // 
            // catchBox
            // 
            catchBox.Enabled = false;
            catchBox.Name = "catchBox";
            catchBox.Size = new System.Drawing.Size(146, 22);
            catchBox.Text = "방화벽: None";
            catchBox.Visible = false;
            // 
            // shotBox
            // 
            shotBox.Enabled = false;
            shotBox.Name = "shotBox";
            shotBox.Size = new System.Drawing.Size(146, 22);
            shotBox.Text = "단축키: None";
            shotBox.Visible = false;
            // 
            // _toolStripSeparator
            // 
            _toolStripSeparator.Name = "_toolStripSeparator";
            _toolStripSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // settings
            // 
            settings.Name = "settings";
            settings.Size = new System.Drawing.Size(146, 22);
            settings.Text = "설정";
            // 
            // exit
            // 
            exit.Name = "exit";
            exit.Size = new System.Drawing.Size(146, 22);
            exit.Text = "종료";
            // 
            // 
            // 
            contextMenu.ResumeLayout(false);

        }

        private static System.ComponentModel.IContainer components = null;
        private static NotifyIcon tray;
        private static ContextMenuStrip contextMenu;
        private static ToolStripSeparator _toolStripSeparator;
        private static ToolStripMenuItem settings;
        private static ToolStripMenuItem exit;
        private static ToolStripMenuItem statusBox;
        private static ToolStripMenuItem catchBox;
        private static ToolStripMenuItem shotBox;
    }
}

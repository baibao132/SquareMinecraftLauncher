using MahApps.Metro.Controls.Dialogs;
using SquareMinecraftLauncher;
using SquareMinecraftLauncher.Minecraft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SquareMinecraftLauncher.userControl
{
    /// <summary>
    /// login.xaml 的交互逻辑
    /// </summary>
    public partial class login : UserControl
    {
        public login()
        {
            InitializeComponent();
        }
        SquareMinecraftLauncherWPF.Core Core = new SquareMinecraftLauncherWPF.Core();
        #region 启动游戏
    DispatcherTimer GameTextTimer = new DispatcherTimer();
        ProgressDialogController loading = null;
    private async void BT1_Click(object sender, RoutedEventArgs e)
    {
        if (DIYvar.Main1.GameVersion.SelectedIndex != -1)
        {
            if (!await libraries(DIYvar.Main1.GameVersion.Text))
            {
                var loading = await DIYvar.Main.ShowProgressAsync("提示","正在补全中...");
                    loading.SetIndeterminate();
                    await DIYvar.Main.lib(loading);
                    await loading.CloseAsync();
            }
        }


        string arg = null;
        if (DIYvar.Main1.Sever.Text != "")
        {
            arg = "--server " + DIYvar.Main1.Sever.Text;
            if (DIYvar.Main1.port.Text == "")
            {
                arg += " --port 25565";
            }
            else
            {
                arg += " --port " + DIYvar.Main1.port.Text;
            }
        }
        if (DIYvar.Main1.hz.Text != "" && arg == null)
        {
            arg = DIYvar.Main1.hz.Text;
        }
        else if (DIYvar.Main1.hz.Text != "" && arg != null)
        {
            arg += " " + DIYvar.Main1.hz.Text;
        }
            loading = await DIYvar.Main.ShowProgressAsync("提示","启动中...");
            loading.SetIndeterminate();
            SquareMinecraftLauncher.Minecraft.Game game = new Game();
        game.LogEvent += new Game.LogDel(log);
        game.ErrorEvent += new Game.ErrorDel(error);
        try
        {
            switch (cb1.SelectedIndex)
            {
                case 0:
                    await game.StartGame(DIYvar.Main1.GameVersion.Text, DIYvar.Main1.javaw.Text, Convert.ToInt32(DIYvar.Main1.RAM.Text), TB1.Text, DIYvar.Main1.JVM.Text, arg);
                    break;
                case 1:
                    await game.StartGame(DIYvar.Main1.GameVersion.Text, DIYvar.Main1.javaw.Text, Convert.ToInt32(DIYvar.Main1.RAM.Text), TB1.Text, TB2.Password, DIYvar.Main1.JVM.Text, arg);
                    break;
                case 2:
                    switch (DIYvar.Main1.yz.SelectedIndex)
                    {
                        case 0:
                            if (DIYvar.Main1.rw.SelectedIndex != -1)
                            {
                                int r = DIYvar.Main1.rw.SelectedIndex;
                                DIYvar.Main1.Button_Click(sender, e);
                                DIYvar.Main1.rw.SelectedIndex = r;
                                await game.StartGame(DIYvar.Main1.GameVersion.Text, DIYvar.Main1.javaw.Text, Convert.ToInt32(DIYvar.Main1.RAM.Text), DIYvar.Pass.name, DIYvar.Pass.id, DIYvar.Pass.accessToken, DIYvar.Main1.ID.Text, DIYvar.Main1.JVM.Text, arg, AuthenticationServerMode.UnifiedPass);
                            }
                            else
                            {
                                    SquareMinecraftLauncherWPF.Core.Message(DIYvar.Main, "未选择人物", true);
                                return;
                            }
                            break;
                        case 1:
                            if (DIYvar.Main1.rw.SelectedIndex != -1)
                            {
                                int r = DIYvar.Main1.rw.SelectedIndex;
                                DIYvar.Main1.Button_Click(sender, e);
                                DIYvar.Main1.rw.SelectedIndex = r;
                                await game.StartGame(DIYvar.Main1.GameVersion.Text, DIYvar.Main1.javaw.Text, Convert.ToInt32(DIYvar.Main1.RAM.Text), DIYvar.Name.Name, DIYvar.Name.uuid, DIYvar.Skin.accessToken, DIYvar.Main1.ID.Text, DIYvar.Main1.JVM.Text, arg, AuthenticationServerMode.yggdrasil);
                            }
                            else
                            {
                                    SquareMinecraftLauncherWPF.Core.Message(DIYvar.Main, "未选择人物", true);
                                return;
                            }
                            break;
                    }
                    break;

            }
            BT1.IsEnabled = false;
            logs = null;
            GameTextTimer = Core.timer(GameText, 2000);
            GameTextTimer.Start();
            await loading.CloseAsync();
            }
        catch (SquareMinecraftLauncherException ex)
        {
            await loading.CloseAsync();
            SquareMinecraftLauncherWPF.Core.Message(DIYvar.Main, ex.Message, true);
            Han = false;
        }
    }
    private void error(Game.Error error)
    {
        Dispatcher.Invoke((Action)async delegate ()
        {
            if (error.SeriousError != null)
            {
                try
                {
                    await loading.CloseAsync();
                }
                catch(Exception)
                { }
                GameTextTimer.Stop();
                DIYvar.Main.Show();
                DIYvar.Main.login.BT1.IsEnabled = true;
                SquareMinecraftLauncherWPF.Core.Message(DIYvar.Main, "发生严重错误：" + "\n" + error.SeriousError, false);
            }
        });
    }
    internal static string logs = null;
    private void log(Game.Log log)
    {
        logs += log.Message + "\n";
        Console.WriteLine(log.Message);
    }
    #endregion
    Tools tools = new Tools();
    static bool Han = true;
    private void GameText(object ob, EventArgs arg)
    {
        if (Han)
        {
            if (windows.WinAPI.GetHandle("LWJGL") != null || windows.WinAPI.GetHandle("GLFW30") != null)
            {
                DIYvar.Main.Hide();
                tools.ChangeTheTitle(DIYvar.Main1.GameText.Text);
                Han = false;
                GameTextTimer.Interval = TimeSpan.FromMilliseconds(3000);

            }
        }
        if (windows.WinAPI.GetHandle("LWJGL") == (IntPtr)0 && windows.WinAPI.GetHandle("GLFW30") == (IntPtr)0)
        {
            if (logs != null)
            {
                Core.SetFile(Directory.GetCurrentDirectory() + @"\SikaDeerLauncher\VersionLog");
                Core.wj(Directory.GetCurrentDirectory() + @"\SikaDeerLauncher\VersionLog\" + DIYvar.Main1.GameVersion.Text + ".log", logs);
            }
            BT1.IsEnabled = true;
            Han = true;
                DIYvar.Main.Show();
            GameTextTimer.Stop();
        }
    }
        private void Label_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            label1.Visibility = Visibility.Collapsed;
            TB2.Focus();
        }

        private void Cb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cb1.SelectedIndex)
            {
                case 0:
                    TB1.Text = "游戏名";
                    TB1.Visibility = Visibility.Visible;
                    TB2.Visibility = Visibility.Collapsed;
                    label1.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    TB1.Text = "邮箱";
                    TB1.Visibility = Visibility.Visible;
                    TB2.Visibility = Visibility.Visible;
                    label1.Visibility = Visibility.Visible;
                    break;
                case 2:
                    TB1.Visibility = Visibility.Collapsed;
                    TB2.Visibility = Visibility.Collapsed;
                    label1.Visibility = Visibility.Collapsed;
                    break;
            }

        }

        private void TB1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TB1.Text = "";
        }
        static int libraries1 = 0;
        static int libraries2 = 0;
        DispatcherTimer libTimer = new DispatcherTimer();
        MCDownload[] downloads = new MCDownload[0];
        DispatcherTimer AssetTimer = new DispatcherTimer();
        static int d = 0;
        public static bool Asset = false;
        public async Task<bool> libraries(string version)
        {
            var assetbool = await assetAsync(version);
            if (!Mi)
            {
                tools.DownloadSourceInitialization(DownloadSource.bmclapiSource);
            }
            libraries1 = sz.id;
            MCDownload[] File = tools.GetMissingFile(version);
            if (File.Length != 0)
            {

                foreach (var i in File)
                {
                    DIYvar.Main1.Download(i.path, "补全", i.Url);
                }
                libraries2 = sz.id;
                return false;
            }
            if (!assetbool)
            {
                return false;
            }
                return true;
        }
        public async Task<bool> assetAsync(string version)
        {
            try
            {
                downloads = tools.GetMissingAsset(version);
            }
            catch
            {
                return true;
            }
            if (downloads.Length != 0)
            {
                MetroDialogSettings settings = new MetroDialogSettings();
                settings.AffirmativeButtonText = "下载";
                settings.NegativeButtonText = "取消";
                var loading = await DIYvar.Main.ShowMessageAsync("提示", "是否下载资源文件", MessageDialogStyle.AffirmativeAndNegative, settings);
                if (loading != MessageDialogResult.Affirmative)
                {
                    return true;
                }
                d = 3;
                for (int i = 0; i < 3; i++)
                {
                    if (downloads.Length <= 3)
                    {
                        for (int t = 0; t < downloads.Length; t++)
                        {
                            zy[t] = DIYvar.Main1.Download(downloads[t].path, "资源补全", downloads[t].Url);
                        }
                        break;
                    }
                    zy[i] = DIYvar.Main1.Download(downloads[i].path, "资源补全", downloads[i].Url);
                }
                AssetTimer.Start();
                Asset = true;
                return false;
            }
            return true;
        }
        int[] zy = new int[3];
        public bool AssetTimerEnvent(ref string pdc)
        {
            for (int i = 0; i < zy.Length; i++)
            {
                if (DIYvar.Main1.xzItems[zy[i]].xzwz.IndexOf("无法") >= 0 || DIYvar.Main1.xzItems[zy[i]].xzwz.IndexOf("完成") >= 0 || DIYvar.Main1.xzItems[zy[i]].xzwz.IndexOf("失败") >= 0)
                {
                    if (downloads.Length - d < 3)
                    {
                        for (int t = d - 1; t < downloads.Length; t++)
                        {
                            DIYvar.Main1.Assetxz.Text = d + "/" + downloads.Length;
                            DIYvar.Main1.Download(downloads[t].path, "资源补全", downloads[t].Url);
                        }
                        return true;
                    }
                    else
                    {
                        for (int t = 0; t < 3; t++, d++)
                        {
                            DIYvar.Main1.Assetxz.Text = d + "/" + downloads.Length;
                            pdc = d + "/" + downloads.Length;
                            zy[i] = DIYvar.Main1.Download(downloads[d].path, "资源补全", downloads[d].Url);
                        }
                    }
                }
            }
            return false;
        }
        bool Mi = false;
        public bool librariesTimer(ref string lt)
        {
            int ap = 0;
            if (libraries2 == 0)
            {
                return true;
            }
            for (int i = libraries1; i < libraries2; i++)
            {
                if (DIYvar.Main1.xzItems[i].xzwz.IndexOf("无法") >= 0 || DIYvar.Main1.xzItems[i].xzwz.IndexOf("失败") >= 0 || DIYvar.Main1.xzItems[i].xzwz.IndexOf("完成") >= 0)
                {
                    ap++;
                }
            }
            if (ap == libraries2 - libraries1)
            {
                for (int i = 0; i < libraries2; i++)
                {
                    if (DIYvar.Main1.xzItems[i].xzwz.IndexOf("无法") >= 0 || DIYvar.Main1.xzItems[i].xzwz.IndexOf("失败") >= 0)
                    {
                        tools.DownloadSourceInitialization(DownloadSource.MinecraftSource);
                        Mi = true;
                        libraries1 = sz.id;
                        MCDownload[] File = tools.GetMissingFile(DIYvar.Main1.GameVersion.Text);
                        lt = ap + "/" + Convert.ToString(libraries2 - libraries1);
                        if (File.Length != 0)
                        {

                            foreach (var s in File)
                            {
                                DIYvar.Main1.Download(s.path, "补全", s.Url);
                            }
                            libraries2 = sz.id;
                            return false;
                        }
                        return true;
                    }
                }
                return true;
            }
            lt = ap + "/" + Convert.ToString(libraries2 - libraries1);
            return false;
        }
    }
   
}

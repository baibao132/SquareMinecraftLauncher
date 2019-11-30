using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Gac;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using MinecraftServer.Server;
using SquareMinecraftLauncher;
using SquareMinecraftLauncher.Core;
using SquareMinecraftLauncher.Minecraft;

namespace SquareMinecraftLauncher
{
    /// <summary>
    /// sz.xaml 的交互逻辑
    /// </summary>
    public partial class sz : MetroWindow
    {
        private static void CurrentDomain_UnhandleException(object sender, UnhandledExceptionEventArgs e)
        {
            exception exception = new exception();
            exception.ex.Text = e.ExceptionObject.ToString();
            exception.ShowDialog();
        }
        public sz()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandleException);
            InitializeComponent();
        }
        public ObservableCollection<string> Capture = new ObservableCollection<string>();
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            xz.ItemsSource = Core.ItemAdd(xzItems.ToArray());
            dlf.doSendMsg += new DownLoadFile.dlgSendMsg(SendMsgHander);
        }
        #region 验证
        internal void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ZH.Text == "" || MM.Password == "" || ID.Text == "")
            {
                SquareMinecraftLauncherWPF.Core.Message(this, "任何内容都不可为空", true);
                return;
            }
            try
            {
                DIYvar.Pass = null;
                SquareMinecraftLauncher.Minecraft.Tools tools = new SquareMinecraftLauncher.Minecraft.Tools();
                if (yz.SelectedIndex == 0)
                {
                    SquareMinecraftLauncher.Minecraft.UnifiedPass pass = tools.GetUnifiedPass(ID.Text, ZH.Text, MM.Password);
                    rw.Items.Clear();
                    rw.Items.Add(pass.name);
                    DIYvar.Pass = pass;
                }
                else
                {
                    SquareMinecraftLauncher.Minecraft.Skin skin = tools.GetAuthlib_Injector(ID.Text, ZH.Text, MM.Password);
                    rw.Items.Clear();
                    foreach (SquareMinecraftLauncher.Minecraft.SkinName a in skin.NameItem)
                    {
                        rw.Items.Add(a.Name);
                    }
                    DIYvar.Skin = skin;
                    DIYvar.Name = skin.NameItem[0];
                }
            }
            catch (SquareMinecraftLauncher.SquareMinecraftLauncherException ex)
            {
                SquareMinecraftLauncherWPF.Core.Message(this, ex.Message, true);
            }
        }
        #endregion

        private void WinColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (colorc == null)
            {
                colorc = "BaseLight";
            }
            color = (this.WinColor.SelectedItem as ListBoxItem).Content.ToString();
            ChangeTheme(color, colorc);
        }
        /// <summary>
        /// 设置App样式
        /// </summary>
        /// <param name="accentName">窗口标题栏样式</param>
        /// <param name="themeName">背景样式</param>
        public void ChangeTheme(string accentName, string themeName)
        {

            Accent expectedAccent = ThemeManager.Accents.First(x => x.Name == accentName); //"Blue"
            AppTheme expectedTheme = ThemeManager.GetAppTheme(themeName);  //"BaseDark","BaseLight"
            ThemeManager.ChangeAppStyle(Application.Current, expectedAccent, expectedTheme);
        }
        static string color = null;
        static string colorc = null;
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (color == null)
            {
                color = "Blue";
            }
            colorc = (this.WinC.SelectedItem as ListBoxItem).Content.ToString();
            ChangeTheme(color, colorc);
        }

        SquareMinecraftLauncherWPF.Core Core = new SquareMinecraftLauncherWPF.Core();
        #region 浏览
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "打开";
            openFileDialog.Filter = "PNG图片|*.png";
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    Core.SetFile("SquareMinecraftLauncher");
                    Core.SetFile(@"SquareMinecraftLauncher\bj");
                    File.Copy(openFileDialog.FileName, @"SquareMinecraftLauncher\bj\bj.png", true);
                    Image1.Source = Core.BitmapToBitmapImage(new System.Drawing.Bitmap(@"SquareMinecraftLauncher\bj\bj.png"));
                    this.Background = Core.brush(@"SquareMinecraftLauncher\bj\bj.png");
                    DIYvar.Main.Background = Core.brush(@"SquareMinecraftLauncher\bj\bj.png");
                }
            }
            catch
            {
                this.ShowMessageAsync("提示", "图片编码有误，请换张图");
                File.Delete(@"SquareMinecraftLauncher\bj\bj.png");
            }
        }
        #endregion
        private void Rw_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (yz.SelectedIndex == 1)
            {
                if (DIYvar.Skin.NameItem != null)
                {
                    DIYvar.Name = DIYvar.Skin.NameItem[rw.SelectedIndex];
                    return;
                }
                else
                {
                    SquareMinecraftLauncherWPF.Core.Message(this, "请先通过验证", true);
                }
            }
        }
        public List<xzItem> xzItems = new List<xzItem>();
        public Gac.DownLoadFile dlf = new DownLoadFile();
        public static int id = 0;
        internal int Download(string path, string ly, string url)
        {
            this.dlf.AddDown(url, path.Replace(System.IO.Path.GetFileName(path), ""), System.IO.Path.GetFileName(path), id);
            this.dlf.StartDown(3);
            id++;
            xzItem xzItem = new xzItem(System.IO.Path.GetFileName(path), 0, ly, "等待中", url, path);
            xzItems.Add(xzItem);
            xz.ItemsSource = Core.ItemAdd(xzItems.ToArray());
            zjd.Maximum += 100;
            return id - 1;
        }
        private void SendMsgHander(DownMsg msg)
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                DownStatus tag = msg.Tag;
                if (tag == DownStatus.Start)
                {
                    xzItems[msg.Id].xzwz = "开始下载";
                    xz.ItemsSource = Core.ItemAdd(xzItems.ToArray());
                    return;
                }
                if (tag == DownStatus.End)
                {
                    xzItems[msg.Id].xzwz = "完成";
                    zjd.Value += msg.Progress - Convert.ToDouble(xzItems[msg.Id].Template);
                    xzItems[msg.Id].Template = 100;
                    ws.Content = msg.SpeedInfo + "/S";
                    xz.ItemsSource = Core.ItemAdd(xzItems.ToArray());
                    return;
                }
                if (tag == DownStatus.Error)
                {
                    xzItems[msg.Id].xzwz = msg.ErrMessage;
                    xz.ItemsSource = Core.ItemAdd(xzItems.ToArray());
                    zjd.Value -= Convert.ToDouble(xzItems[msg.Id].Template);
                    zjd.Maximum -= 100;
                    return;
                }
                if (tag == DownStatus.DownLoad)
                {
                    xzItems[msg.Id].xzwz = "下载中";
                    zjd.Value += msg.Progress - Convert.ToDouble(xzItems[msg.Id].Template);
                    xzItems[msg.Id].Template = msg.Progress;
                    ws.Content = msg.SpeedInfo + "/S";
                    xz.ItemsSource = Core.ItemAdd(xzItems.ToArray());
                    return;
                }
            });
        }
        internal static McVersionList[] mcVersionLists = new McVersionList[0];
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (yxlx.SelectedIndex != -1)
            {
                switch (yxlx.SelectedIndex)
                {
                    case 0:
                        mcVersionLists = DIYvar.minecraft1.ToArray();
                        break;
                    case 1:
                        mcVersionLists = DIYvar.minecraft2.ToArray();
                        break;
                    case 2:
                        mcVersionLists = DIYvar.minecraft3.ToArray();
                        break;
                    case 3:
                        mcVersionLists = DIYvar.minecraft4.ToArray();
                        break;
                    case 4:
                        mcVersionLists = DIYvar.minecraft5.ToArray();
                        break;
                }
                if (GameVersionList != null)
                {
                    GameVersionList.ItemsSource = Core.ItemAdd(mcVersionLists);
                }
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        Tools tools = new Tools();
        SquareMinecraftLauncher.MinecraftDownload MinecraftDownload = new SquareMinecraftLauncher.MinecraftDownload();
        static int JarID, JsonID;
        static System.Windows.Threading.DispatcherTimer JarTimer = new System.Windows.Threading.DispatcherTimer();
        internal static bool JarTimerBool = false;
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MCDownload download = MinecraftDownload.MCjarDownload(mcVersionLists[GameVersionList.SelectedIndex].version);
            JarID = Download(download.path, "游戏核心", download.Url);
            download = MinecraftDownload.MCjsonDownload(mcVersionLists[GameVersionList.SelectedIndex].version);
            JsonID = Download(download.path, "游戏核心", download.Url);
            SquareMinecraftLauncherWPF.Core.Message(this, "开始下载中，请转到下载页面查看", true);
            JarTimer = Core.timer(MCjarInstall, 3000);
            JarTimer.Start();
            JarTimerBool = true;
        }

        private void MCjarInstall(object ob, EventArgs a)
        {
            if (xzItems[JarID].xzwz == "完成" && xzItems[JsonID].xzwz == "完成")
            {
                this.GameVersion.Items.Clear();
                foreach (var i in tools.GetAllTheExistingVersion())
                {
                    GameVersion.Items.Add(i.version);
                }
                SquareMinecraftLauncherWPF.Core.Message(this, "安装完成", true);
                JarTimerBool = false;
                JarTimer.Stop();
            }
        }
        private void ZH_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ZH.Text = "";
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Javaw|javaw.exe";
            open.Title = "打开";
            if (open.ShowDialog() == true)
            {
                javaw.Text = open.FileName;
            }
        }
        static int JavaID = 0;
        MCDownload java = new MCDownload();
        System.Windows.Threading.DispatcherTimer JavaTimer = new System.Windows.Threading.DispatcherTimer();
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            java = MinecraftDownload.JavaFileDownload();
            JavaID = id;
            Download(java.path, "Java下载", java.Url);
            SquareMinecraftLauncherWPF.Core.Message(this, "开始下载，请转到下载页面查看", true);
            JavaTimer = Core.timer(JavaDown, 3000);
            JavaTimer.Start();
        }
        private void JavaDown(object sender, EventArgs e)
        {
            if (xzItems[JavaID].xzwz == "完成")
            {
                Process.Start(java.path);
                JavaTimer.Stop();
                return;
            }
            if (xzItems[JavaID].xzwz.IndexOf("失败") > 0 || xzItems[JavaID].xzwz.IndexOf("无法") > 0)
            {
                JavaTimer.Stop();
                return;
            }

        }

        private void Label1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            label1.Visibility = Visibility.Collapsed;
            MM.Focus();
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
           
        }
        OptiFineList[] optis = new OptiFineList[0];
        private async void KzbGameVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var loading = await DIYvar.Main1.ShowProgressAsync("提示", "正在获取该版本扩展包信息");
            loading.SetIndeterminate();
            try
            {
                Fabricmc.Items.Clear();
                SquareMinecraftLauncher.Core.fabricmc.fabricmc fabricmc1 = new SquareMinecraftLauncher.Core.fabricmc.fabricmc();
                var fab = await fabricmc1.FabricmcList(kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString());
                Fabricmc.Items.Add("无");
                foreach (var i in fab)
                {
                    Fabricmc.Items.Add(i);
                }
                Fabricmc.SelectedIndex = 1;
            }
            catch(SquareMinecraftLauncherException ex)
            {
                if (ex.Message == "访问失败")
                {
                    Fabricmc.Items.Add("未获取到该版本Fabricmc，请检查网络后重试");
                    Fabricmc.SelectedIndex = 0;
                    await loading.CloseAsync();
                    return;
                }
                else
                {
                    Fabricmc.Items.Add("无相关Fabricmc版本");
                    Fabricmc.SelectedIndex = 0;
                }
            }
            try
            {
                Forge.Items.Clear();
                var a = await tools.GetForgeList(kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString());
                Forge.Items.Add("无");
                foreach (var i in a)
                {
                    Forge.Items.Add(i.ForgeVersion);
                }
                Forge.SelectedIndex = 1;
            }
            catch (SquareMinecraftLauncherException ex)
            {
                if (ex.Message == "访问失败")
                {
                    Forge.Items.Add("未获取到该版本Forge，请检查网络后重试");
                    Forge.SelectedIndex = 0;
                    await loading.CloseAsync();
                    return;
                }
                else
                {
                    Forge.Items.Add("无相关Forge版本");
                    Forge.SelectedIndex = 0;
                }
            }

            try
            {
                Liteloader.Items.Clear();
                var a = await tools.GetLiteloaderList();
                Liteloader.Items.Add("无");
                int xp = 0;
                foreach (var i in a)
                {
                    if (i.mcversion == DIYvar.ForgeGameVersion[kzbGameVersion.SelectedIndex].IdVersion)
                    {
                        Liteloader.Items.Add(i.version);
                        xp++;
                    }
                }
                if (xp == 0)
                {
                    Liteloader.SelectedIndex = 0;
                }
                else
                {
                    Liteloader.SelectedIndex = 1;
                }
            }
            catch (SquareMinecraftLauncherException ex)
            {
                if (ex.Message == "获取失败")
                {
                    Liteloader.Items.Add("未获取到该版本Liteloader，请检查网络后重试");
                    Liteloader.SelectedIndex = 0;

                }
                else
                {
                    Liteloader.Items.Add("无相关Liteloader版本");
                    Liteloader.SelectedIndex = 0;
                }
            }
            try
            {
                Optifine.Items.Clear();
                optis = await tools.GetOptiFineList(kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString());
                Optifine.Items.Add("无");
                foreach (var i in optis)
                {
                    Optifine.Items.Add(i.filename);
                }
                Optifine.SelectedIndex = 1;
            }
            catch (SquareMinecraftLauncherException ex)
            {
                if (ex.Message == "获取失败")
                {
                    Optifine.Items.Add("未获取到该版本Optifine，请检查网络后重试");
                    Optifine.SelectedIndex = 0;
                    await loading.CloseAsync();
                    return;
                }
                else
                {
                    Optifine.Items.Add("无相关Optifine版本");
                    Optifine.SelectedIndex = 0;
                }
            }
            await loading.CloseAsync();
        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (javaw.Text == "")
            {
                SquareMinecraftLauncherWPF.Core.Message(this, "需要安装java", true);
                return;
            }
            string title = "正在安装中...";
            var loading = await this.ShowProgressAsync("提示", title);
            loading.SetIndeterminate();
            MinecraftDownload minecraft = new MinecraftDownload();
            if (Optifine.SelectedIndex != 0)
            {
                try
                {
                await tools.OptifineInstall(kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString(), optis[Optifine.SelectedIndex].patch);
                title += "\nOptifine安装完成";
                loading.SetMessage(title);
            }
                catch
            {
                title += "\nOptifine已经安装了，如需更换版本请先卸载该扩展包";
                loading.SetMessage(title);
            }
        }
            if (Liteloader.SelectedIndex != 0)
            {
                try
                {
                    await tools.liteloaderInstall(kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString());
                    title += "\nliteloader安装完成";
                    loading.SetMessage(title);
                }
                catch
                {
                    title += "\nliteloader已经安装了，如需更换版本请先卸载该扩展包";
                    loading.SetMessage(title);
                }
            }

            if (Forge.SelectedIndex != 0)
            {
                GameVersionF = kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString();
                ForgeD = minecraft.ForgeDownload(kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString(), Forge.Items[Forge.SelectedIndex].ToString());
                Console.WriteLine(ForgeD.Url);
                if (File.Exists(ForgeD.path))
                {
                    if (await tools.ForgeInstallation(ForgeD.path, GameVersionF, javaw.Text))
                    {
                        title += "\nForge安装完成";
                        loading.SetMessage(title);
                    }
                    else
                    {
                        title += "\nForge安装失败";
                        loading.SetMessage(title);//该提示一般出现于1.13版本以上的Forge安装，因为1.13版本json发生了极大的变化导致Forge安装也发生了变化
                    }
                    return;
                }
                ForgeID = Download(ForgeD.path, "Forge扩展包", ForgeD.Url);
                await ForgeTimerEvent(loading,title);
            }
            if (Fabricmc.SelectedIndex != 0)
            {
                SquareMinecraftLauncher.Core.fabricmc.fabricmc fabricmc = new SquareMinecraftLauncher.Core.fabricmc.fabricmc();
                bool fvi = await fabricmc.FabricmcVersionInstall(kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString(), Fabricmc.Items[Fabricmc.SelectedIndex].ToString());
                if (fvi)
                {
                    title += "\nFabricmc安装完成";
                    loading.SetMessage(title);
                }
                else 
                {
                    title += "\nFabricmc安装失败";
                    loading.SetMessage(title);
                }
            }
            await loading.CloseAsync();
        }
        string GameVersionF = null;
        MCDownload ForgeD = new MCDownload();
        int ForgeID = 0;
        System.Windows.Threading.DispatcherTimer ForgeTimer = new System.Windows.Threading.DispatcherTimer();
        private async Task ForgeTimerEvent(ProgressDialogController loading,string title)
        {
            bool flag = true;
            await Task.Factory.StartNew(async () =>
            {
                while (flag)
                {
                    if (xzItems[ForgeID].xzwz == "完成")
                    {
                        xzItems[ForgeID].xzwz = "安装中";
                        if (await tools.ForgeInstallation(ForgeD.path, GameVersionF, javaw.Text))
                        {
                            xzItems[ForgeID].xzwz = "安装完成";
                            title += "\nForge安装完成";
                            loading.SetMessage(title);
                        }
                        else
                        {
                            xzItems[ForgeID].xzwz = "安装失败";
                            title += "\nForge安装失败";
                            loading.SetMessage(title);
                        }
                        flag = false;
                    }
                    if (xzItems[ForgeID].xzwz.IndexOf("无法") >= 0 || xzItems[ForgeID].xzwz.IndexOf("失败") >= 0)
                    {
                        title += "\nForge安装失败";
                        loading.SetMessage(title);
                        flag = false;
                    }
                        Thread.Sleep(3000);
                }
            });
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            try
            {
                tools.UninstallTheExpansionPack(ExpansionPack.Forge, DIYvar.l[GameGLVersion.SelectedIndex].Content.ToString().Replace(" ", ""));
                yxglForge.Text = "没有安装Forge";
                SquareMinecraftLauncherWPF.Core.Message(this, "卸载完成", true);
            }
            catch (SquareMinecraftLauncherException ex)
            {
                SquareMinecraftLauncherWPF.Core.Message(this, ex.Message, true);
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                tools.UninstallTheExpansionPack(ExpansionPack.Liteloader, DIYvar.l[GameGLVersion.SelectedIndex].Content.ToString().Replace(" ", ""));
                yxglForge.Text = "没有安装Liteloader";
                SquareMinecraftLauncherWPF.Core.Message(this, "卸载完成", true);
            }
            catch (SquareMinecraftLauncherException ex)
            {
                SquareMinecraftLauncherWPF.Core.Message(this, ex.Message, true);
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            try
            {
                tools.UninstallTheExpansionPack(ExpansionPack.Optifine, DIYvar.l[GameGLVersion.SelectedIndex].Content.ToString().Replace(" ", ""));
                yxglForge.Text = "没有安装Optifine";
                SquareMinecraftLauncherWPF.Core.Message(this, "卸载完成", true);
            }
            catch (SquareMinecraftLauncherException ex)
            {
                SquareMinecraftLauncherWPF.Core.Message(this, ex.Message, true);
            }
        }

        private void Ss_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (ss.Text.Length >= 3)
            {
                for (int i = 0; i < mcVersionLists.Length; i++)
                {
                    if (mcVersionLists[i].version == ss.Text)
                    {
                        GameVersionList.SelectedIndex = i;
                        GameVersionList.ScrollIntoView(mcVersionLists[i]);
                        return;
                    }
                }
            }
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ss.Text = "";
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Control.SelectedIndex = 6 ;
            Show();
            return;
        }
        private void GameGLVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GameGLVersion.SelectedIndex == -1)
            {
                return;
            }
            foreach (var i in DIYvar.l)
            {
                i.bj.Visibility = Visibility.Collapsed;
                i.q.Height = 80;
                i.Height = 80;
            }
            DIYvar.l[GameGLVersion.SelectedIndex].q.Height = 120;
            DIYvar.l[GameGLVersion.SelectedIndex].Height = 120;
            DIYvar.l[GameGLVersion.SelectedIndex].bj.Visibility = Visibility.Visible;
            GameGLVersion.ItemsSource = DIYvar.l;
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "javaw.exe|javaw.exe";
            if (openFileDialog.ShowDialog() == true)
            {
                javaw.Text = openFileDialog.FileName;
            }
        }

        private void IsAutoJavaToggle_Click(object sender, RoutedEventArgs e)
        {
            if (isAutoJavaToggle.IsChecked == true)
            {
                RAM.Text = tools.GetMemorySize().AppropriateMemory.ToString();
                RAM.IsEnabled = false;
            }
            else if (isAutoJavaToggle.IsChecked == false)
            {
                RAM.IsEnabled = true;
            }
        }
    }
}

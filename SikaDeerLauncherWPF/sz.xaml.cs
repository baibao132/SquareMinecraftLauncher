using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Shapes;
using Gac;
using MahApps.Metro;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using MinecraftServer.Server;
using SikaDeerLauncher;
using SikaDeerLauncher.Minecraft;

namespace SikaDeerLauncherWPF
{
    /// <summary>
    /// sz.xaml 的交互逻辑
    /// </summary>
    public partial class Sz : MetroWindow
    {
        public Sz()
        {
            InitializeComponent();
        }
        public ObservableCollection<string> Capture = new ObservableCollection<string>();
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            xz.ItemsSource = Core.ItemAdd(xzItems.ToArray());
            dlf.doSendMsg += new DownLoadFile.dlgSendMsg(SendMsgHander);
            this.itemsControl.ItemsSource = Capture;
        }
        #region 验证
        internal void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ZH.Text == "" || MM.Password == "" || ID.Text == "")
            {
                Core.Message("任何内容都不可为空", true);
                return;
            }
            try
            {
                DIYvar.Pass = null;
                SikaDeerLauncher.Minecraft.Tools tools = new SikaDeerLauncher.Minecraft.Tools();
                if (yz.SelectedIndex == 0)
                {
                    SikaDeerLauncher.Minecraft.UnifiedPass pass = tools.GetUnifiedPass(ID.Text, ZH.Text, MM.Password);
                    rw.Items.Clear();
                    rw.Items.Add(pass.name);
                    DIYvar.Pass = pass;
                }
                else
                {
                    SikaDeerLauncher.Minecraft.Skin skin = tools.GetAuthlib_Injector(ID.Text, ZH.Text, MM.Password);
                    rw.Items.Clear();
                    foreach (SikaDeerLauncher.Minecraft.SkinName a in skin.NameItem)
                    {
                        rw.Items.Add(a.Name);
                    }
                    DIYvar.Skin = skin;
                    DIYvar.Name = skin.NameItem[0];
                }
            }
            catch (SikaDeerLauncher.SikaDeerLauncherException ex)
            {
                Core.Message(ex.Message, true);
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

        Core Core = new Core();
        #region 浏览
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "打开";
            openFileDialog.Filter = "所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                Core.SetFile("SikaDeerLauncher");
                Core.SetFile(@"SikaDeerLauncher\bj");
                File.Copy(openFileDialog.FileName, @"SikaDeerLauncher\bj\bj.jpg", true);
                Image1.Source = Core.BitmapToBitmapImage(new System.Drawing.Bitmap(@"SikaDeerLauncher\bj\bj.jpg"));
                this.Background = Core.brush(@"SikaDeerLauncher\bj\bj.jpg");
                DIYvar.Main.Background = Core.brush(@"SikaDeerLauncher\bj\bj.jpg"); 
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
                    Core.Message("请先通过验证", true);
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
        SikaDeerLauncher.MinecraftDownload MinecraftDownload = new SikaDeerLauncher.MinecraftDownload();
        static int JarID, JsonID;
        static System.Windows.Threading.DispatcherTimer JarTimer = new System.Windows.Threading.DispatcherTimer();
        internal static bool JarTimerBool = false;
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MCDownload download = MinecraftDownload.MCjarDownload(mcVersionLists[GameVersionList.SelectedIndex].version);
            JarID = Download(download.path, "游戏核心", download.Url);
            download = MinecraftDownload.MCjsonDownload(mcVersionLists[GameVersionList.SelectedIndex].version);
            JsonID = Download(download.path, "游戏核心", download.Url);
            Core.Message("开始下载中，请转到下载页面查看", true);
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
                Core.Message("安装完成", true);
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
            Core.Message("开始下载，请转到下载页面查看", true);
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

        private void jzz(string text)
        {

        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
        OptiFineList[] optis = new OptiFineList[0];
        private void KzbGameVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Forge.Items.Clear();
                var a = tools.GetForgeList(kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString());
                Forge.Items.Add("无");
                foreach (var i in a)
                {
                    Forge.Items.Add(i.ForgeVersion);
                }
                Forge.SelectedIndex = 1;
            }
            catch (SikaDeerLauncherException ex)
            {
                if (ex.Message == "访问失败")
                {
                    Forge.Items.Add("未获取到该版本Forge，请检查网络后重试");
                    Forge.SelectedIndex = 0;
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
                var a = tools.GetLiteloaderList();
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
            catch (SikaDeerLauncherException ex)
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
                optis = tools.GetOptiFineList(kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString());
                Optifine.Items.Add("无");
                foreach (var i in optis)
                {
                    Optifine.Items.Add(i.filename);
                }
                Optifine.SelectedIndex = 1;
            }
            catch (SikaDeerLauncherException ex)
            {
                if (ex.Message == "获取失败")
                {
                    Optifine.Items.Add("未获取到该版本Optifine，请检查网络后重试");
                    Optifine.SelectedIndex = 0;
                    return;
                }
                else
                {
                    Optifine.Items.Add("无相关Optifine版本");
                    Optifine.SelectedIndex = 0;
                }
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (javaw.Text == "")
            {
                Core.Message("需要安装java", true);
                return;
            }
            MinecraftDownload minecraft = new MinecraftDownload();
            if (Optifine.SelectedIndex != 0)
            {
                tools.OptifineInstall(kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString(), optis[Optifine.SelectedIndex].patch);
                Core.Message("Optifine安装完成", true);
            }
            if (Liteloader.SelectedIndex != 0)
            {
                tools.liteloaderInstall(kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString());
                Core.Message("liteloader安装完成", true);
            }

            if (Forge.SelectedIndex != 0)
            {
                GameVersionF = kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString();
                ForgeD = minecraft.ForgeDownload(kzbGameVersion.Items[kzbGameVersion.SelectedIndex].ToString(), Forge.Items[Forge.SelectedIndex].ToString());
                if (File.Exists(ForgeD.path))
                {
                    if (tools.ForgeInstallation(ForgeD.path, GameVersionF, javaw.Text))
                    {
                        Core.Message("Forge安装完成", true);
                    }
                    else
                    {
                        Core.Message("Forge安装失败，可能网络不稳定", true);//该提示一般出现于1.13版本以上的Forge安装，因为1.13版本json发生了极大的变化导致Forge安装也发生了变化
                    }
                    return;
                }
                ForgeID = Download(ForgeD.path, "Forge扩展包", ForgeD.Url);
                ForgeTimer = Core.timer(ForgeTimerEvent, 3000);
                ForgeTimer.Start();
                Core.Message("Forge下载中", true);
            }
        }
        string GameVersionF = null;
        MCDownload ForgeD = new MCDownload();
        int ForgeID = 0;
        System.Windows.Threading.DispatcherTimer ForgeTimer = new System.Windows.Threading.DispatcherTimer();
        private void ForgeTimerEvent(object a, EventArgs args)
        {
            if (xzItems[ForgeID].xzwz == "完成")
            {
                xzItems[ForgeID].xzwz = "安装中";
                if (tools.ForgeInstallation(ForgeD.path, GameVersionF, javaw.Text))
                {
                    xzItems[ForgeID].xzwz = "安装完成";
                    Core.Message("Forge安装完成", true);
                }
                else
                {
                    xzItems[ForgeID].xzwz = "安装失败";
                    Core.Message("Forge安装失败，可能网络不稳定", true);
                }
                ForgeTimer.Stop();
            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            try
            {
                tools.UninstallTheExpansionPack(ExpansionPack.Forge, DIYvar.l[GameGLVersion.SelectedIndex].Content.ToString().Replace(" ", ""));
                yxglForge.Text = "没有安装Forge";
                Core.Message("卸载完成", true);
            }
            catch (SikaDeerLauncherException ex)
            {
                Core.Message(ex.Message, true);
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                tools.UninstallTheExpansionPack(ExpansionPack.Liteloader, DIYvar.l[GameGLVersion.SelectedIndex].Content.ToString().Replace(" ", ""));
                yxglForge.Text = "没有安装Liteloader";
                Core.Message("卸载完成", true);
            }
            catch (SikaDeerLauncherException ex)
            {
                Core.Message(ex.Message, true);
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            try
            {
                tools.UninstallTheExpansionPack(ExpansionPack.Optifine, DIYvar.l[GameGLVersion.SelectedIndex].Content.ToString().Replace(" ", ""));
                yxglForge.Text = "没有安装Optifine";
                Core.Message("卸载完成", true);
            }
            catch (SikaDeerLauncherException ex)
            {
                Core.Message(ex.Message, true);
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
        #region 反馈
        /// <summary>
        /// 上传截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpLoadCaptrue_Click(object sender, MouseButtonEventArgs e)
        {
            if (Capture.ToArray().Length == 5)
            {
                Core.Message("截图只能加入5张", true);
                return;
            }
            OpenFileDialog logoSelected = new OpenFileDialog();
            logoSelected.Filter = "图片|*.jpg;*.png;*.bmp;*.gif";
            if (logoSelected.ShowDialog() == true)
            {
                Capture.Add(logoSelected.FileName);
            }
        }

        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (fk.Text == "请描述您遇到的bug、启动游戏时的崩溃或建议。")
            {
                fk.Text = "";
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            List<string> email = new List<string>();
            email.Add("2817541592@qq.com");
            List<string> a = new List<string>();
            foreach (var i in Capture)
            {
                a.Add(i.ToString());
            }
            if (SmtpClass.sendmail("baibaostudio@126.com","baibaostudio","2817541592@qq.com","2817541592", "反馈", fkxx.Text + "\n" + fk.Text + "\n" + yx.Text, a.ToArray(),"smtp.126.com","baibaostudio@126.com","Zz3481133"))
            {
                Core.Message("反馈成功，后续会通过邮件回复您\n感谢您的反馈，我们会通过您提交的反馈做到越来越好",true);
                Capture.Clear();
                fk.Text = "";
                yx.Text = "";
                return;
            }
            Core.Message("反馈失败，请确认是否被安全软件拦截",true);
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Control.SelectedIndex = 6 ;
            Show();
            return;
        }

        private void ToggleSwitchButton_Checked(object sender, RoutedEventArgs e)
        {
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
                i.q.Height = 110;
                i.Height = 110;
            }
            DIYvar.l[GameGLVersion.SelectedIndex].q.Height = 160;
            DIYvar.l[GameGLVersion.SelectedIndex].Height = 160;
            DIYvar.l[GameGLVersion.SelectedIndex].bj.Visibility = Visibility.Visible;
            GameGLVersion.ItemsSource = DIYvar.l;
        }



        /// <summary>
        /// 移除截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReMoveCaptrue_Click(object sender, MouseButtonEventArgs e)
        {
            Capture.Remove(((Image)sender).DataContext.ToString());
        }
        #endregion
    }
}

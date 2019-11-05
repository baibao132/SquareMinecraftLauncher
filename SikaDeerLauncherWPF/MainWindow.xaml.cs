using System;
using System.Collections.Generic;
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
using MahApps.Metro;
using MahApps.Metro.Controls;
using System.Windows.Threading;
using mcbbs;
using SikaDeerLauncher;
using SikaDeerLauncher.Minecraft;
using MinecraftServer.Server;
using System.IO;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;

namespace SikaDeerLauncherWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        #region 启动游戏
        DispatcherTimer GameTextTimer = new DispatcherTimer();
        private void BT1_Click(object sender, RoutedEventArgs e)
        {
            if (Sz.GameVersion.SelectedIndex != -1)
            {
                if (!libraries(Sz.GameVersion.Text))
                {
                    Core.Message("需要补全文件", true);
                    return;
                }
            }


            string arg = null;
            if (Sz.Sever.Text != "")
            {
                arg = "--server " + Sz.Sever.Text;
                if (Sz.port.Text == "")
                {
                    arg += " --port 25565"; 
                }
                else
                {
                    arg += " --port " + Sz.port.Text;
                }
            }
            if (Sz.hz.Text != "" && arg == null)
            {
                arg = Sz.hz.Text;
            }
            else if (Sz.hz.Text != "" && arg != null)
            {
                arg += " " + Sz.hz.Text;
            }

            SikaDeerLauncher.Minecraft.Game game = new Game();
            game.LogEvent += new Game.LogDel(log);
            game.ErrorEvent += new Game.ErrorDel(error);
            try
            {
                switch (cb1.SelectedIndex)
                {
                    case 0:
                        game.StartGame(Sz.GameVersion.Text, Sz.javaw.Text, Convert.ToInt32(Sz.RAM.Text), TB1.Text, Sz.JVM.Text, arg);
                        break;
                    case 1:
                        game.StartGame(Sz.GameVersion.Text, Sz.javaw.Text, Convert.ToInt32(Sz.RAM.Text), TB1.Text, TB2.Password, Sz.JVM.Text, arg);
                        break;
                    case 2:
                        switch (Sz.yz.SelectedIndex)
                        {
                            case 0:
                                if (Sz.rw.SelectedIndex != -1)
                                {
                                    int r = Sz.rw.SelectedIndex;
                                    Sz.Button_Click(sender, e);
                                    Sz.rw.SelectedIndex = r;
                                    game.StartGame(Sz.GameVersion.Text, Sz.javaw.Text, Convert.ToInt32(Sz.RAM.Text), DIYvar.Pass.name, DIYvar.Pass.id, DIYvar.Pass.accessToken, Sz.ID.Text, Sz.JVM.Text, arg, AuthenticationServerMode.UnifiedPass);
                                }
                                else
                                {
                                    Core.Message("未选择人物", true);
                                    return;
                                }
                                break;
                            case 1:
                                if (Sz.rw.SelectedIndex != -1)
                                {
                                    int r = Sz.rw.SelectedIndex;
                                    Sz.Button_Click(sender, e);
                                    Sz.rw.SelectedIndex = r;
                                    game.StartGame(Sz.GameVersion.Text, Sz.javaw.Text, Convert.ToInt32(Sz.RAM.Text), DIYvar.Name.Name, DIYvar.Name.uuid, DIYvar.Skin.accessToken, Sz.ID.Text, Sz.JVM.Text, arg, AuthenticationServerMode.yggdrasil);
                                }
                                else
                                {
                                    Core.Message("未选择人物", true);
                                    return;
                                }
                                break;
                        }
                        break;

                }
                BT1.IsEnabled = false;
                logs = null;
                LabelGame.Visibility = Visibility.Visible;
                GameTextTimer = Core.timer(GameText,15000);
                GameTextTimer.Start();
            }
            catch (SikaDeerLauncherException ex)
            {
                Core.Message(ex.Message,true);
                Han = false;
            }
        }
        private void error(Game.Error error)
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                if (error.SeriousError != null)
                {
                        Core.Message("发生严重错误：" + "\n" + error.SeriousError, false);
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
        private void GameText(object ob,EventArgs arg)
        {
            if (Han)
            {
                if (windows.WinAPI.GetHandle("LWJGL") != null || windows.WinAPI.GetHandle("GLFW30") != null)
                {
                    LabelGame.Visibility = Visibility.Collapsed;
                    this.Hide();
                    tools.ChangeTheTitle(Sz.GameText.Text);
                        Han = false;
                    GameTextTimer.Interval = TimeSpan.FromMilliseconds(3000);

                }
            }
            if (windows.WinAPI.GetHandle("LWJGL") == (IntPtr)0 && windows.WinAPI.GetHandle("GLFW30") == (IntPtr)0)
            {
                if (logs != null)
                {
                    Core.SetFile(Directory.GetCurrentDirectory() + @"\SikaDeerLauncher\VersionLog");
                    Core.wj(Directory.GetCurrentDirectory() + @"\SikaDeerLauncher\VersionLog\" + Sz.GameVersion.Text + ".log", logs);
                }
                BT1.IsEnabled = true;
                Han = true;
                this.Show();
                GameTextTimer.Stop();
            }
        }
        #region 窗体创建完毕
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

            DoubleAnimation aero = new DoubleAnimation(); //建立线性动画对象aero
            aero.From = 0;    //动画初始的值
            aero.To = 10;      //动画结束的值
            aero.Duration = TimeSpan.FromSeconds(1);         //动画持续时间
            mh.Effect = new BlurEffect() //为Image bg的effect添加blureffect属性
            {
                Radius = 0	//模糊半径初始化
            };
            mh.Effect.BeginAnimation(BlurEffect.RadiusProperty,aero);
            DIYvar.Main = this;
            DIYvar.Main1 = Sz;
            #region 读更新
            if (SikaDeerLauncher.Core.ping.CheckServeStatus())
            {
                SikaDeerLauncher.Download download = new Download();
                var a = download.getHtml("http://118.31.6.246/libraries/SikaDeerLauncher/gx");
                if (a != null)
                {
                    if (a != System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())
                    {
                        gx1.Visibility = Visibility.Visible;
                        gx2.Visibility = Visibility.Visible;
                        gx3.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        gx1.Visibility = Visibility.Collapsed;
                        gx2.Visibility = Visibility.Collapsed;
                        gx3.Visibility = Visibility.Collapsed;
                    }
                }
            }
            #endregion
            #region 新闻部分
            mcbbsnews mcbbsnews = new mcbbsnews();

            mcbbsnews.News(ref news);
            if (news.Length != 0)
            {
                image1.Source = Core.brush(news[0].IMG, null).ImageSource;
                Core.timer(mcbbs, 3000).Start();
            }
            #endregion
            #region 读配置项
            Core.iniwv = false;
            Core.iniWirte(Sz, this);
            #endregion
            #region 读版本
            try
            {
                AllTheExistingVersion[] all = tools.GetAllTheExistingVersion();
                foreach (var i in all)
                {
                    Sz.GameVersion.Items.Add(i.version);
                }
                Sz.GameVersion.SelectedIndex = 0;
            }
            catch { }
            #endregion

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        }
        #endregion
        Core Core = new Core();
        #region 设置展开收起按钮
        static bool a = true;
        private void Ret_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            animation animation = new animation();
            if (a)
            {

                animation.c(a, config, -485);
                a = false;
                ret.Fill = Core.brush(null, "go_to_link1");
                image1.Visibility = Visibility.Collapsed;//新闻隐藏
            }
            else
            {

                animation.c(a, config, -891);
                a = true;
                ret.Fill = Core.brush(null, "go_to_link");
                image1.Visibility = Visibility.Visible;//新闻显示
            }
        }
        #endregion
        #region 新闻
        static mcbbsnews.newsArray[] news = new mcbbsnews.newsArray[0];
        static int newsi = 1;
        void mcbbs(object a, EventArgs s)
        {
            if (newsi != news.Length)
            {
                image1.Source = Core.brush(news[newsi].IMG, null).ImageSource;
                newsi++;
            }
            else
            {
                newsi = 0;
            }
        }
        #endregion
        #region 设置
        private void Sz_Click(object sender, RoutedEventArgs e)
        {
            Sz.Control.SelectedIndex = 0;
            Sz.ShowDialog();
        }
        #endregion
        #region 自定义登录
        private void Zdy_Click(object sender, RoutedEventArgs e)
        {
            Sz.Control.SelectedIndex = 0;
            Sz.szControl.SelectedIndex = 0;
            Sz.ShowDialog();
        }
        #endregion
        #region 个性化
        private void Gxh_Click(object sender, RoutedEventArgs e)
        {
            Sz.Control.SelectedIndex = 0;
            Sz.szControl.SelectedIndex = 1;
            Sz.ShowDialog();
        }
        #endregion

        private void Xz_Click(object sender, RoutedEventArgs e)
        {
            Sz.Control.SelectedIndex = 1;
            Sz.ShowDialog();
        }
        Sz Sz = new Sz();
        private void Hx_Click(object sender, RoutedEventArgs e)
        {
            if (Sz.JarTimerBool == true)
            {
                Core.Message("目前有版本正在下载，请稍后再试",true);
                return;
            }
            Sz.Control.SelectedIndex = 2;
            Tools tools = new Tools();
            MCVersionList[] mc = new MCVersionList[0];
            try
            {
                mc = tools.GetMCVersionList();
            }
            catch
            {
                Core.Message("未获取到游戏下载列表，请重试", true);
                return;
            }
            foreach (var i in mc)
            {
                switch (i.type)
                {
                    case "正式版":
                        McVersionList a = new McVersionList(i.id,i.releaseTime);
                        DIYvar.minecraft1.Add(a);
                        break;
                    case "快照版":
                        McVersionList b = new McVersionList(i.id, i.releaseTime);
                        DIYvar.minecraft2.Add(b);
                        break;
                    case "基岩版":
                        McVersionList c = new McVersionList(i.id, i.releaseTime);
                        DIYvar.minecraft3.Add(c);
                        break;
                    case "远古版":
                        McVersionList d = new McVersionList(i.id, i.releaseTime);
                        DIYvar.minecraft4.Add(d);
                        break;
                }
                var DT = DateTime.Parse(i.releaseTime);
                if (DT.ToString("MM-dd") == "04-01")
                {
                    McVersionList s = new McVersionList(i.id, i.releaseTime);
                    DIYvar.minecraft5.Add(s);
                }
                
            }
            Sz.mcVersionLists = DIYvar.minecraft1.ToArray();
            Sz.GameVersionList.ItemsSource = Core.ItemAdd(DIYvar.minecraft1.ToArray());
            Sz.ShowDialog();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            #region 写配置项
            Core.iniwv = true;
            Core.iniWirte(Sz, this);
            #endregion
            System.Environment.Exit(0);
        }

        private void Label_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            label1.Visibility = Visibility.Collapsed;
            TB2.Focus();
        }

        private void Cb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(cb1.SelectedIndex)
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
        static int Asset = 0;
        public bool libraries(string version)
        {
            if (!Mi)
            {
                tools.DownloadSourceInitialization(DownloadSource.bmclapiSource);
            }
            libraries1 = Sz.id;
            MCDownload[] File = tools.GetMissingFile(version);
            if (File.Length != 0)
            {

                foreach (var i in File)
                {
                    Sz.Download(i.path,"补全", i.Url);
                }
                libraries2 = Sz.id;
                libTimer = Core.timer(librariesTimer, 3000);
                BT1.IsEnabled = false;
                libTimer.Start();
                return false;
            }
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
                if (MessageBox.Show("是否下载资源文件", "提示", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                {
                    return true;
                }
                AssetTimer = Core.timer(AssetTimerEnvent, 3000);
                d = 3;
                for (int i = 0; i < 3; i++)
                {
                    if (downloads.Length <= 3)
                    {
                        for (int t = 0; t < downloads.Length; t++)
                        {
                            zy[t] = Sz.Download(downloads[t].path, "资源补全", downloads[t].Url);
                        }
                        break;
                    }
                    zy[i] = Sz.Download(downloads[i].path, "资源补全", downloads[i].Url);
                }
                BT1.IsEnabled = false;
                AssetTimer.Start();
                return false;
            }
            return true;
        }
        int[] zy = new int[3];
        private void AssetTimerEnvent(object ob,EventArgs a)
        {
            for (int i = 0; i< zy.Length;i++)
            {
                if (Sz.xzItems[zy[i]].xzwz.IndexOf("无法") >= 0 || Sz.xzItems[zy[i]].xzwz.IndexOf("完成") >= 0 || Sz.xzItems[zy[i]].xzwz.IndexOf("失败") >= 0)
                {
                    if (downloads.Length - d < 3)
                    {
                        for (int t = d - 1;t < downloads.Length;t++)
                        {
                            Sz.Assetxz.Text = d + "/" + downloads.Length;
                            Sz.Download(downloads[t].path, "资源补全", downloads[t].Url);
                        }
                        BT1.IsEnabled = true;
                        Core.Message("资源文件下载完成", true);
                        AssetTimer.Stop();
                        return;
                    }
                    else
                    {
                        for (int t = 0; t < 3; t++, d++)
                        {
                            Sz.Assetxz.Text = d + "/" + downloads.Length;
                            zy[i] = Sz.Download(downloads[d].path, "资源补全", downloads[d].Url);
                        }
                    }
                }
            }
        }
        bool Mi = false;
        private void librariesTimer(object a,EventArgs args)
        {
            int ap = 0;
            for (int i = libraries1; i < libraries2; i++)
            {
                if (Sz.xzItems[i].xzwz.IndexOf("无法") >= 0 || Sz.xzItems[i].xzwz.IndexOf("失败") >= 0 || Sz.xzItems[i].xzwz.IndexOf("完成") >= 0)
                {
                    ap++;
                }
            }
            if (ap == libraries2 - libraries1)
            {
                for (int i = 0; i < libraries2; i++)
                {
                    if (Sz.xzItems[i].xzwz.IndexOf("无法") >= 0 || Sz.xzItems[i].xzwz.IndexOf("失败") >= 0)
                    {
                        libTimer.Stop();
                        tools.DownloadSourceInitialization(DownloadSource.MinecraftSource);
                        Mi = true;
                        libraries(Sz.GameVersion.Text);
                        return;
                    }
                }
                BT1.IsEnabled = true;
                Core.Message("依赖库下载完成", true);
                libTimer.Stop();
            }
        }

        private void Kzb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Sz.kzbGameVersion.Items.Clear();
                DIYvar.ForgeGameVersion = tools.GetAllTheExistingVersion();
                foreach (var i in DIYvar.ForgeGameVersion)
                {
                    Sz.kzbGameVersion.Items.Add(i.version);
                }
                Sz.kzbGameVersion.SelectedIndex = 0;
                Sz.Control.SelectedIndex = 3;
                Sz.Show();
            }
            catch(Exception ex)
            {
                Sz.kzbGameVersion.SelectedIndex = 0;
                Sz.Control.SelectedIndex = 3;
                Sz.Show();
            }
        }

        private void Gl_Click(object sender, RoutedEventArgs e)
        {
            AllTheExistingVersion[] t = new AllTheExistingVersion[0];
            try
            {
                t = tools.GetAllTheExistingVersion();
            }
            catch
            {
                Core.Message("无任何版本", true);
                return;
            }
            List<UserControl1> user1 = new List<UserControl1>();
            for (int i = 0; i < t.Length; i++)
            {
                UserControl1 user = new UserControl1();
                user.aa.Content = t[i].version;
                user1.Add(user);
            }
            DIYvar.l = user1;
            Sz.GameGLVersion.ItemsSource = user1.ToArray();
                Sz.GameGLVersion.SelectedIndex = 0;
            Sz.Control.SelectedIndex = 6;
            Sz.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Sz.Control.SelectedIndex = 5;
            Sz.Show();
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://118.31.6.246/2019/10/29/sikadeerlauncher%E5%90%AF%E5%8A%A8%E5%99%A8%E4%B8%8B%E8%BD%BD/");
        }
    }
}

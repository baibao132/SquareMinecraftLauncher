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
using SquareMinecraftLauncher;
using SquareMinecraftLauncher.Minecraft;
using MinecraftServer.Server;
using System.IO;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;

namespace SquareMinecraftLauncher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static void CurrentDomain_UnhandleException(object sender, UnhandledExceptionEventArgs e)
        {
            exception exception = new exception();
            exception.ex.Text = e.ExceptionObject.ToString();
            exception.ShowDialog();
        }

        public MainWindow()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            DIYvar.Main = this;
            DIYvar.Main1 = Sz;
            #region 读配置项
            SquareMinecraftLauncherWPF.Core.iniwv = false;
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
            #region 读更新

            if (SquareMinecraftLauncher.Core.ping.CheckServeStatus())
            {
                SquareMinecraftLauncher.Download download = new Download();
                var a = download.getHtml("http://118.31.6.246/libraries/SikaDeerLauncher/gx");
                if (a != null)
                {
                    if (a != System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())
                    {
                        update update = new update();
                        update.version.Text = a;
                        update.text.Text = download.getHtml("http://118.31.6.246/libraries/SikaDeerLauncher/gxlr.txt");
                        update.ShowDialog();
                        return;
                    }
                }
            }
            #endregion
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }
        #region 窗体创建完毕
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DIYvar.Main1.Control.SelectedIndex = 5;
            DIYvar.Main1.Show();
        }
        SquareMinecraftLauncherWPF.Core Core = new SquareMinecraftLauncherWPF.Core();
        #region 设置展开收起按钮
        static bool a = true;
        public void dh(Ellipse ret)
        {
            animation animation = new animation();
            if (a)
            {

                animation.c(a, config, 15);
                a = false;
                ret.Fill = Core.brush(null, "go_to_link1");
                mcbbs.Visibility = Visibility.Collapsed;//新闻隐藏
            }
            else
            {

                animation.c(a, config, -365);
                a = true;
                ret.Fill = Core.brush(null, "go_to_link");
                mcbbs.Visibility = Visibility.Visible;//新闻显示
            }
        }
        #endregion
        sz Sz = new sz();
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            #region 写配置项
            SquareMinecraftLauncherWPF.Core.iniwv = true;
            Core.iniWirte(Sz, this);
            #endregion
            System.Environment.Exit(0);
        }
        Tools tools = new Tools();
        #region 下载更新
        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://118.31.6.246/2019/10/29/SquareMinecraftLauncher%E5%90%AF%E5%8A%A8%E5%99%A8%E4%B8%8B%E8%BD%BD/");
        }
        #endregion
        #region 下载补全
        public async Task lib(ProgressDialogController pdc)
        {
            bool al = true;
            bool asset = true;
            bool lib = false ;
            string at = "0/0",lt = "0/0";
            await Task.Factory.StartNew(() =>
            {
                while(al)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {

                        if (userControl.login.Asset)
                        {
                            if (login.AssetTimerEnvent(ref at))
                            {
                                asset = true;
                            }
                            else
                            {
                                asset = false;
                            }
                        }
                        if (!lib)
                        {
                            if (!login.librariesTimer(ref lt))
                            {
                                lib = false;
                            }
                            else
                            {

                                lib = true;
                            }
                        }
                        pdc.SetMessage("正在下载补全...\n已下载的资源文件：" + at + "\n已下载的依赖库：" + lt);
                        if (lib == true && asset == true)
                        {
                            al = false;
                        }
                    }));
                    Thread.Sleep(2000);
                }
            });
        }
        #endregion
    }
}

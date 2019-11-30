using System;
using System.Collections.Generic;
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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace SquareMinecraftLauncher
{
    /// <summary>
    /// update.xaml 的交互逻辑
    /// </summary>
    public partial class update : MetroWindow
    {
        public update()
        {
            InitializeComponent();
        }
        ProgressDialogController loading = null;
        SquareMinecraftLauncherWPF.Core Core = new SquareMinecraftLauncherWPF.Core();
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            loading = await this.ShowProgressAsync("提示", "正在更新中\n已更新：0%");
            loading.SetIndeterminate();
            Download(Directory.GetCurrentDirectory() + @"\SikaDeerLauncher-" + version.Text + ".exe", "更新", "http://118.31.6.246/libraries/SikaDeerLauncher/SikaDeerLauncher.exe");
        }
        public Gac.DownLoadFile dlf = new DownLoadFile();
        public static int id = 0;
        internal int Download(string path, string ly, string url)
        {
            dlf.doSendMsg += SendMsgHander;
            this.dlf.AddDown(url, path.Replace(System.IO.Path.GetFileName(path), ""), System.IO.Path.GetFileName(path), id);
            this.dlf.StartDown(3);
            id++;
            return id - 1;
        }
        private void SendMsgHander(DownMsg msg)
        {
            Dispatcher.Invoke((Action)async delegate ()
            {
                DownStatus tag = msg.Tag;
                if (tag == DownStatus.End)
                {
                    loading.SetMessage("正在更新中\n已更新：100%");
                    Thread.Sleep(2000);
                    await loading.CloseAsync();
                    Process.Start(Directory.GetCurrentDirectory() + @"\SikaDeerLauncher-" + version.Text + ".exe");
                    #region 写配置项
                    SquareMinecraftLauncherWPF.Core.iniwv = true;
                    Core.iniWirte(DIYvar.Main1, DIYvar.Main);
                    #endregion
                    System.Environment.Exit(0);
                    return;
                }
                if (tag == DownStatus.Error)
                {
                    loading.SetMessage("正在更新中\n已更新：无法下载正在转到下载网址");
                    Thread.Sleep(2000);
                    await loading.CloseAsync();
                    Process.Start("http://118.31.6.246/2019/10/29/sikadeerlauncher%E5%90%AF%E5%8A%A8%E5%99%A8%E4%B8%8B%E8%BD%BD/");
                    #region 写配置项
                    SquareMinecraftLauncherWPF.Core.iniwv = true;
                    Core.iniWirte(DIYvar.Main1, DIYvar.Main);
                    #endregion
                    System.Environment.Exit(0);
                }
                if (tag == DownStatus.DownLoad)
                {
                    loading.SetMessage("正在更新中\n已更新："+msg.Progress+"%");
                    return;
                }
            });
        }
    }
}

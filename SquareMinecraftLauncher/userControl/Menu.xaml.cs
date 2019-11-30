using MahApps.Metro.Controls.Dialogs;
using SquareMinecraftLauncher.Minecraft;
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

namespace SquareMinecraftLauncher.userControl
{
    /// <summary>
    /// Menu.xaml 的交互逻辑
    /// </summary>
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();
        }
        #region 设置
        private void Sz_Click(object sender, RoutedEventArgs e)
        {
            DIYvar.Main1.Control.SelectedIndex = 0;
            DIYvar.Main1.ShowDialog();
        }
        #endregion
        #region 自定义登录
        private void Zdy_Click(object sender, RoutedEventArgs e)
        {
            DIYvar.Main1.Control.SelectedIndex = 0;
            DIYvar.Main1.szControl.SelectedIndex = 0;
            DIYvar.Main1.ShowDialog();
        }
        #endregion
        #region 个性化
        private void Gxh_Click(object sender, RoutedEventArgs e)
        {
            DIYvar.Main1.Control.SelectedIndex = 0;
            DIYvar.Main1.szControl.SelectedIndex = 1;
            DIYvar.Main1.ShowDialog();
        }
        #endregion

        private void Xz_Click(object sender, RoutedEventArgs e)
        {
            DIYvar.Main1.Control.SelectedIndex = 1;
            DIYvar.Main1.ShowDialog();
        }
        private async void Hx_Click(object sender, RoutedEventArgs e)
        {
            if (sz.JarTimerBool == true)
            {
                SquareMinecraftLauncherWPF.Core.Message(DIYvar.Main, "目前有版本正在下载，请稍后再试", true);
                return;
            }
            var loading = await DIYvar.Main.ShowProgressAsync("提示","正在获取版本信息...");
            loading.SetIndeterminate();
            DIYvar.Main1.Control.SelectedIndex = 2;
            Tools tools = new Tools();
            MCVersionList[] mc = new MCVersionList[0];
            try
            {
                mc = await tools.GetMCVersionList();
            }
            catch
            {
                await loading.CloseAsync();
                SquareMinecraftLauncherWPF.Core.Message(DIYvar.Main, "未获取到游戏下载列表，请重试", true);
                return;
            }
            foreach (var i in mc)
            {
                switch (i.type)
                {
                    case "正式版":
                        McVersionList a = new McVersionList(i.id, i.releaseTime);
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
            sz.mcVersionLists = DIYvar.minecraft1.ToArray();
            DIYvar.Main1.GameVersionList.ItemsSource = Core.ItemAdd(DIYvar.minecraft1.ToArray());
            await loading.CloseAsync();
            DIYvar.Main1.ShowDialog();
        }
        SquareMinecraftLauncherWPF.Core Core = new SquareMinecraftLauncherWPF.Core();
        Tools tools = new Tools();
        private void Kzb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DIYvar.Main1.kzbGameVersion.Items.Clear();
                DIYvar.ForgeGameVersion = tools.GetAllTheExistingVersion();
                foreach (var i in DIYvar.ForgeGameVersion)
                {
                    DIYvar.Main1.kzbGameVersion.Items.Add(i.version);
                }
                DIYvar.Main1.kzbGameVersion.SelectedIndex = 0;
                DIYvar.Main1.Control.SelectedIndex = 3;
                DIYvar.Main1.Show();
            }
            catch (Exception ex)
            {
                DIYvar.Main1.kzbGameVersion.SelectedIndex = 0;
                DIYvar.Main1.Control.SelectedIndex = 3;
                DIYvar.Main1.Show();
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
                SquareMinecraftLauncherWPF.Core.Message(DIYvar.Main, "无任何版本", true);
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
            DIYvar.Main1.GameGLVersion.ItemsSource = user1.ToArray();
            DIYvar.Main1.GameGLVersion.SelectedIndex = 0;
            DIYvar.Main1.Control.SelectedIndex = 6;
            DIYvar.Main1.Show();
        }

        #region 设置展开收起按钮
        static bool a = true;
        private void Ret_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DIYvar.Main.dh(ret);
        }
        #endregion
    }
}

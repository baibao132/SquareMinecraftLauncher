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

namespace SquareMinecraftLauncher
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
        Tools Tools = new Tools();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            DIYvar.Main1.yxglForge.Text = "";
            DIYvar.Main1.yxgloptifine.Text = "";
            DIYvar.Main1.yxglLiteloader.Text = "";
            string forge = null, Lite = null, Op = null;
            if (Tools.ForgeExist(DIYvar.l[DIYvar.Main1.GameGLVersion.SelectedIndex].aa.Content.ToString(), ref forge))
            {
                DIYvar.Main1.yxglForge.Text = forge;
            }
            else
            {
                DIYvar.Main1.yxglForge.Text = "没有安装Forge";
            }
            if (Tools.LiteloaderExist(DIYvar.l[DIYvar.Main1.GameGLVersion.SelectedIndex].aa.Content.ToString(), ref Lite))
            {
                DIYvar.Main1.yxglLiteloader.Text = Lite;
            }
            else
            {
                DIYvar.Main1.yxglLiteloader.Text = "没有安装Liteloader";
            }
            if (Tools.OptifineExist(DIYvar.l[DIYvar.Main1.GameGLVersion.SelectedIndex].aa.Content.ToString(), ref Op))
            {
                DIYvar.Main1.yxgloptifine.Text = Op;
            }
            else
            {
                DIYvar.Main1.yxgloptifine.Text = "没有安装Optifine";
            }
            string ap = null;
            DIYvar.Main1.rz.Items.Clear();
            try
            {
                ap = File.ReadAllText(@"SikaDeerLauncher\VersionLog\" + DIYvar.l[DIYvar.Main1.GameGLVersion.SelectedIndex].aa.Content + ".log");
                foreach (var i in ap.Split('\n'))
                {
                    DIYvar.Main1.rz.Items.Add(i);
                }
            }
            catch
            {
                DIYvar.Main1.rz.Items.Add("没有进行过任何一次启动");
            }
            DIYvar.Main1.Control.SelectedIndex = 4;
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            name.Visibility = Visibility.Visible;
            name.Focus();
        }

        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (name.Text == "")
            {
                name.Visibility = Visibility.Collapsed;
                return;
            }
            FileInfo fi = new FileInfo(@".minecraft\versions\" + aa.Content.ToString() + @"\" + aa.Content.ToString() + ".jar"); //xx/xx/aa.rar
            fi.MoveTo(@".minecraft\versions\" + aa.Content.ToString() + @"\" + name.Text + ".jar");
            FileInfo fi1 = new FileInfo(@".minecraft\versions\" + aa.Content.ToString() + @"\" + aa.Content.ToString() + ".json"); //xx/xx/aa.rar
            fi1.MoveTo(@".minecraft\versions\" + aa.Content.ToString() + @"\" + name.Text + ".json");
            System.IO.DirectoryInfo di = new DirectoryInfo(@".minecraft\versions\" + aa.Content.ToString());
            di.MoveTo(@".minecraft\versions\" + name.Text);
            aa.Content = name.Text;
            name.Text = "";
            name.Visibility = Visibility.Collapsed;
            DIYvar.Main1.GameVersion.Items.Clear();
            #region 读版本
            try
            {
                AllTheExistingVersion[] all = Tools.GetAllTheExistingVersion();
                foreach (var i in all)
                {
                    DIYvar.Main1.GameVersion.Items.Add(i.version);
                }
                DIYvar.Main1.GameVersion.SelectedIndex = 0;
            }
            catch { }
            #endregion
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                AllTheExistingVersion[] t = new AllTheExistingVersion[0];
                DeleteDir(@".minecraft\versions\" + aa.Content.ToString());
                t = Tools.GetAllTheExistingVersion();
                List<UserControl1> user1 = new List<UserControl1>();
                for (int i = 0; i < t.Length; i++)
                {
                    UserControl1 user = new UserControl1();
                    user.aa.Content = t[i].version;
                    user1.Add(user);
                }
                DIYvar.l = user1;
                DIYvar.Main1.GameGLVersion.ItemsSource = DIYvar.l;

            }
            catch
            {
                SquareMinecraftLauncherWPF.Core.Message(DIYvar.Main1,"删除失败", true);
            }
            DIYvar.Main1.GameVersion.Items.Clear();
            #region 读版本
                try
                {
                    AllTheExistingVersion[] all = Tools.GetAllTheExistingVersion();
                    foreach (var i in all)
                    {
                        DIYvar.Main1.GameVersion.Items.Add(i.version);
                    }
                    DIYvar.Main1.GameVersion.SelectedIndex = 0;
                }
                catch { }
                #endregion
        }
        /// <summary>
        ///直接删除指定目录下的所有文件及文件夹(保留目录)
        /// </summary>
        /// <param name="strPath">文件夹路径</param>
        /// <returns>执行结果</returns>

        public static void DeleteDir(string file)
        {

                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(file);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                //去除文件的只读属性
                System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);

                //判断文件夹是否还存在
                if (Directory.Exists(file))
                {

                    foreach (string f in Directory.GetFileSystemEntries(file))
                    {

                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                            Console.WriteLine(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            DeleteDir(f);
                        }

                    }

                    //删除空文件夹

                    Directory.Delete(file);

                }

            }

        }
    }


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Gac;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MinecraftServer.Server;
using SquareMinecraftLauncher;
using SquareMinecraftLauncher.Minecraft;

namespace SquareMinecraftLauncherWPF
{
    internal class Core
    {
        internal DispatcherTimer timer(EventHandler tick, int Interval)
        {
            DispatcherTimer timer1 = new DispatcherTimer();
            timer1.Interval = TimeSpan.FromMilliseconds(Interval);
            timer1.Tick += tick;
            return timer1;
        }
        internal ImageBrush brush(string url, string image)
        {
            var brush = new ImageBrush(); //定义图片画刷 
            var converter = new ImageSourceConverter();
            if (image != null)
            {
                brush.ImageSource = BitmapToBitmapImage(GetResourceBitmap(image));
                return brush;
            }
            brush.ImageSource = (ImageSource)converter.ConvertFromString(url);
            return brush;
        }
        internal ImageBrush brush(string image)
        {
            var brush = new ImageBrush(); //定义图片画刷 
            var converter = new ImageSourceConverter();
            brush.ImageSource = BitmapToBitmapImage(new Bitmap(image));
            return brush;
        }
        internal ImageBrush brush(BitmapImage image)
        {
            var brush = new ImageBrush(); //定义图片画刷 
            var converter = new ImageSourceConverter();
            brush.ImageSource = image;
            return brush;
        }
            private Bitmap GetResourceBitmap(string strImageName)
        {
            var obj = SquareMinecraftLauncher.Properties.Resources.ResourceManager.GetObject(strImageName, SquareMinecraftLauncher.Properties.Resources.Culture);
            return (Bitmap)obj;
        }
        public BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            System.IO.MemoryStream ms = null;
            BitmapImage bitmapImage = new BitmapImage();
            try
            {

                using (ms = new System.IO.MemoryStream())
                {
                    bitmap.Save(ms, bitmap.RawFormat);
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = ms;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    ms.Close();

                }
                bitmap.Dispose();

            }
            catch
            {
                ms.Close();
                bitmap.Dispose();
                throw new SquareMinecraftLauncher.SquareMinecraftLauncherException("a");

            }
            return bitmapImage;
        }
        internal void SetFile(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        internal void wj(string path, string text)
        {
            try
            {
                File.WriteAllText(path, text, Encoding.UTF8);
            }
            catch (Exception exception1)
            {
                Console.WriteLine(exception1.Message);
            }
        }
        public static void Message(MetroWindow metro,string message, bool a)
        {
            metro.ShowMessageAsync("提示", message);
        }
        internal ObservableCollection<T> ItemAdd<T>(T[] items)
        {
            ObservableCollection<T> personalInfoList = new ObservableCollection<T>();
            foreach (var i in items)
            {
                personalInfoList.Add(i);
            }
            return personalInfoList;
        }
    internal static bool iniwv = false;
    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public string IniWriteValue(string Section, string Key, string Value)
        {
            if (iniwv)
            {
                if (!File.Exists(@"SikaDeerLauncher\config.ini"))
                {
                    File.WriteAllText(@"SikaDeerLauncher\config.ini", "");
                }
                WritePrivateProfileString(Section, Key, Value, @"SikaDeerLauncher\config.ini");
                return Value;
            }
                string a = IniReadValue(Section, Key);
                if (a == "")
                {
                    return Value;
                }
                return a;
        }
        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section,  string Key)
        {
            if (!File.Exists(@"SikaDeerLauncher\config.ini"))
            {
                SetFile("SikaDeerLauncher");
                File.WriteAllText(@"SikaDeerLauncher\config.ini", "");
            }
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, @"SikaDeerLauncher\config.ini");
            return temp.ToString();
        }
        Tools tools = new Tools();
        internal void iniWirte(sz sz,MainWindow main)
        {
            DES.DESEncrypt dES = new DES.DESEncrypt();
            sz.JVM.Text = IniWriteValue("GameConfig","JVM",sz.JVM.Text);//JVM参数
            sz.hz.Text = IniWriteValue("GameConfig", "Rear", sz.hz.Text);//后置参数
            sz.RAM.Text = IniWriteValue("GameConfig", "RAM", sz.RAM.Text);//虚拟内存大小
            if (sz.RAM.Text == "")//虚拟内存大小是否为空
            {
                sz.RAM.Text = tools.GetMemorySize().AppropriateMemory.ToString();//取合适的虚拟内存大小
            }

            #region 自定义登录
            sz.ID.Text = IniWriteValue("GameConfig", "ID", sz.ID.Text); //读写外置登录ID
            sz.yz.SelectedIndex = Convert.ToInt32(IniWriteValue("Login", "Validation", sz.yz.SelectedIndex.ToString()));//读写外置登录验证
            sz.ZH.Text = dES.Decrypt(IniWriteValue("Login", "DIYName", dES.Encrypt(sz.ZH.Text, "ASCTRQAS")), "ASCTRQAS");//读写外置登录账号
            sz.MM.Password = dES.Decrypt(IniWriteValue("Login", "DIYPassword", dES.Encrypt(sz.MM.Password, "ASCTRQAS")), "ASCTRQAS");//读写外置登录密码
            if (sz.ZH.Text != "" && sz.MM.Password != "" && iniwv == false)//判断账号和密码不为空，确认是读配置项
            {
                sz.label1.Visibility = System.Windows.Visibility.Collapsed;//WPF中隐藏密码标签
                object a = new object();//声明对象
                System.Windows.RoutedEventArgs b = new System.Windows.RoutedEventArgs();//声明对象
                sz.Button_Click(a, b);//调用验证按钮事件函数
            }
            sz.rw.SelectedIndex = Convert.ToInt32(IniWriteValue("Login", "DIYFigure", sz.rw.SelectedIndex.ToString()));//读写人物选择
            #endregion
            sz.Sever.Text = IniWriteValue("GameConfig", "Server", sz.Sever.Text);//读写服务器IP
            sz.port.Text = IniWriteValue("GameConfig", "Port", sz.port.Text);//读写服务器端口
            sz.GameText.Text = IniWriteValue("GameConfig", "GameTitle", sz.GameText.Text);//读写游戏标题

            main.login.cb1.SelectedIndex = Convert.ToInt32(IniWriteValue("Login", "LoginWay", main.login.cb1.SelectedIndex.ToString()));//读写登录方式
            if (main.login.cb1.SelectedIndex == 1)//判断验证方式为正版登录
            {
                main.login.TB1.Text = dES.Decrypt(IniWriteValue("Login", "OnName", dES.Encrypt(main.login.TB1.Text, "ASCTRQAS")), "ASCTRQAS");//读写加解密账号
                main.login.TB2.Password = dES.Decrypt(IniWriteValue("Login", "OnPassword", dES.Encrypt(main.login.TB2.Password, "ASCTRQAS")), "ASCTRQAS");//读写加解密密码
                if (main.login.TB2.Password != "")
                {
                    main.login.label1.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            main.login.TB1.Text = IniWriteValue("Login", "LoginName", main.login.TB1.Text);//读写离线登录游戏名
            sz.WinC.SelectedIndex = Convert.ToInt32(IniWriteValue("Launcher", "Theme", sz.WinC.SelectedIndex.ToString()));//读写启动器主题
            sz.javaw.Text = IniWriteValue("GameConfig", "Java", sz.javaw.Text);//读写java位置
            if (sz.javaw.Text == "")//判断java位置是否为空
            {
                sz.javaw.Text = tools.GetJavaPath();//获取java位置
            }
            sz.WinColor.SelectedIndex = Convert.ToInt32(IniWriteValue("Launcher", "Color", sz.WinColor.SelectedIndex.ToString()));
            if (sz.WinColor.SelectedIndex != -1)
            {
                string WinC = "BaseLight";
                if (sz.WinC.SelectedIndex != -1)
                {
                    WinC = (sz.WinC.SelectedItem as ListBoxItem).Content.ToString();
                }
                sz.ChangeTheme((sz.WinColor.SelectedItem as ListBoxItem).Content.ToString(),WinC);
            }
                if (File.Exists(@"SikaDeerLauncher\bj\bj.png"))
                {
                    sz.Background = brush(@"SikaDeerLauncher\bj\bj.png");
                    DIYvar.Main.Background = brush(@"SikaDeerLauncher\bj\bj.png");
                    sz.Image1.Source = BitmapToBitmapImage(new System.Drawing.Bitmap(@"SikaDeerLauncher\bj\bj.png"));
                }}
            //if (sz.Sever.Text != "" || sz.port.Text != "")
            //{
            //    if (sz.port.Text == "")
            //    {
            //        sz.port.Text = "25565";
            //    }
            //    try
            //    {
            //        ServerInfo a = tools.GetServerInformation(sz.Sever.Text, Convert.ToInt32(sz.port.Text));
            //        main.Sever(a);
            //    }
            //    catch
            //    {
            //        Core.Message("无法识别的服务器", true);
            //    }
            //}
        }
    }
    public class xzItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        string _File,_ly,_xzwz;
        double _jd;
        public string File{get {return _File;}set {_File = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("File")); } }
        public double Template
        {
            get
            {
                return _jd;
            }
            set
            {
                _jd = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Template"));
            }
        }
        public string ly { get { return _ly; } set { _ly = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ly")); } }
        public string xzwz { get { return _xzwz; } set { _xzwz = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("xzwz")); } }
        public string url { get; set; }
        public string Path { get; set; }
        internal xzItem(string File,double jd,string ly,string xzwz,string url,string Path)
        {
            this._File = File;
            this._jd = jd;
            this._ly = ly;
            this._xzwz = xzwz;
            this.url = url;
            this.Path = Path;
        }

    }
    internal class McVersionList : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        string _version, _time;
        public string version { get { return _version; } set { _version = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("version")); } }
        public string time { get { return _time; } set { _time = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("time")); } }
        internal McVersionList(string version, string time)
        {
            this._version = version;
            this._time = time;
        }

    }

using mcbbs;
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
    /// mcbbs.xaml 的交互逻辑
    /// </summary>
    public partial class mcbbs : UserControl
    {
        public mcbbs()
        {
            InitializeComponent();
        }
            static mcbbsnews.newsArray[] news = new mcbbsnews.newsArray[0];
            static int newsi = 1;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            #region 新闻部分
            mcbbsnews mcbbsnews = new mcbbsnews();

            mcbbsnews.News(ref news);
            if (news.Length != 0)
            {
                image1.Source = Core.brush(news[0].IMG, null).ImageSource;
                Core.timer(Mcbbs, 3000).Start();
            }
            #endregion
        }
        SquareMinecraftLauncherWPF.Core Core = new SquareMinecraftLauncherWPF.Core();
        #region 新闻

            void Mcbbs(object a, EventArgs s)
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
    }
}

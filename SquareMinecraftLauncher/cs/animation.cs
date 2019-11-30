using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SquareMinecraftLauncher
{
    internal class animation
    {
        System.Windows.Threading.DispatcherTimer timer1;
        static bool a1;
        #region 缓入缓出
        internal void c(bool a,Grid grid,double NewLeft)
        {
            timer1 = null;
            a1 = a;
            config = grid;
            SquareMinecraftLauncherWPF.Core Core = new SquareMinecraftLauncherWPF.Core();
            timer1 = Core.timer(co, 2);
            timer1.Start();
            NL = NewLeft;
        }
        #endregion
        double NL = 0;
        Grid config = null;
        #region 缓出
        private void co( object sender, EventArgs e)
        {
                if (!a1)
                {
                    if (config.Margin.Left > NL)
                    {
                        config.Margin = new Thickness(config.Margin.Left - 10, config.Margin.Top, config.Margin.Right, config.Margin.Bottom);
                        if (config.Margin.Left <= NL - 20)
                        {
                            config.Margin = new Thickness(NL, config.Margin.Top, config.Margin.Right, config.Margin.Bottom);
                            timer1.Stop();
                        }
                    }
                }
                else
                {
                    if (config.Margin.Left < NL)
                    {
                        config.Margin = new Thickness(config.Margin.Left + 10, config.Margin.Top, config.Margin.Right, config.Margin.Bottom);
                        if (config.Margin.Left >= NL - 20)
                        {
                            config.Margin = new Thickness(NL, config.Margin.Top, config.Margin.Right, config.Margin.Bottom);
                            timer1.Stop();
                        }
                    }
                }
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SikaDeerLauncher.Minecraft;

namespace SikaDeerLauncherWPF
{
    internal class DIYvar
    {
        internal static UnifiedPass Pass = new UnifiedPass();
        internal static Skin Skin = new Skin();
        internal static SkinName Name = new SkinName();
        /// <summary>
        /// 正式版
        /// </summary>
        internal static List<McVersionList> minecraft1 = new List<McVersionList>();
        /// <summary>
        /// 快照版
        /// </summary>
        internal static List<McVersionList> minecraft2 = new List<McVersionList>();
        /// <summary>
        /// 基岩版
        /// </summary>
        internal static List<McVersionList> minecraft3 = new List<McVersionList>();
        /// <summary>
        /// 远古版
        /// </summary>
        internal static List<McVersionList> minecraft4 = new List<McVersionList>();
        /// <summary>
        /// 愚人节版本
        /// </summary>
        internal static List<McVersionList> minecraft5 = new List<McVersionList>();
        internal static MainWindow Main = new MainWindow();
        internal static SikaDeerLauncher.Minecraft.AllTheExistingVersion[] ForgeGameVersion = new AllTheExistingVersion[0];
        internal static List<UserControl1> l = new List<UserControl1>();
        internal static Sz Main1 = new Sz();
    }
}

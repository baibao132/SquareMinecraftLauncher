using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SikaDeerLauncherWPF
{

    internal class SmtpClass
    {
        //定义默认的 邮件服务器、帐户、密码、发邮件地址
        private static readonly string mailSvr = "smtp.126.com"; //域名也是OK的mail.163.com
        private static readonly string account = "baibaostudio@126.com";
        private static readonly string pwd = "Zz3481133";
        private static readonly string addr = "baibaostudio";

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sfrom">发送者邮箱</param>
        /// <param name="sfromer">发送人</param>
        /// <param name="sto">接受者邮箱</param>
        /// <param name="stoer">收件人</param>
        /// <param name="sSubject">主题</param>
        /// <param name="sBody">内容</param>
        /// <param name="sfile">附件</param>
        /// <param name="sSMTPHost">smtp服务器</param>
        /// <param name="sSMTPuser">邮箱</param>
        /// <param name="sSMTPpass">密码</param>
        /// <returns></returns>
        public static bool sendmail(string sfrom, string sfromer, string sto, string stoer, string sSubject, string sBody, string[] sfile, string sSMTPHost, string sSMTPuser, string sSMTPpass)
        {
            ////设置from和to地址
            MailAddress from = new MailAddress(sfrom, sfromer);
            MailAddress to = new MailAddress(sto, stoer);
            ////创建一个MailMessage对象
            MailMessage oMail = new MailMessage(from, to);
            //// 添加附件
            if (sfile.Length != 0)
            {
                foreach (var i in sfile)
                {
                    oMail.Attachments.Add(new Attachment(i));
                }
            }
            ////邮件标题
            oMail.Subject = sSubject;
            ////邮件内容
            oMail.Body = sBody;
            ////邮件格式
            oMail.IsBodyHtml = false;
            ////邮件采用的编码
            oMail.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312");
            ////设置邮件的优先级为高
            oMail.Priority = MailPriority.High;
            ////发送邮件
            SmtpClient client = new SmtpClient();
            ////client.UseDefaultCredentials = false; 
            client.Host = sSMTPHost;
            client.Credentials = new NetworkCredential(sSMTPuser, sSMTPpass);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                client.Send(oMail);
                return true;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message.ToString());
                return false;
            }
            finally
            {
                ////释放资源
                oMail.Dispose();
            }
        }
    }
}

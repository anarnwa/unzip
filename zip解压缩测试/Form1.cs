using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ionic.Zip;
using System.Configuration;
using System.Security.Cryptography;

namespace zip解压缩测试
{
    public partial class HCurse批量解压 : Form
    {
        public HCurse批量解压()
        {
            InitializeComponent();
        }
        //设置config文件
        public static void SetValue(string key, string value)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件   
        }
        //获取config的值
        public static string GetValue(string key)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
                return "";
            else
                return config.AppSettings.Settings[key].Value;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text= GetValue("zip");
        }
        public static bool UnZipFile(string targetDirectory, string zipFileName)
        {
            bool result = true;
            try
            {
                using (ZipFile zip = new ZipFile(zipFileName))
                {
                    zip.ExtractAll(targetDirectory);
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SetValue("zip", textBox1.Text);
            string zip = textBox1.Text;
            DirectoryInfo di = new DirectoryInfo(zip);
            foreach (FileInfo fi in di.GetFiles())
            {
                string exname = fi.Name.Substring(fi.Name.LastIndexOf(".") + 1);//得到后缀名    
                //判断当前文件后缀名是否与给定后缀名一样
                if (exname == "zip" || exname == "ZIP")
                {
                    if (UnZipFile(zip, fi.FullName))
                    {
                        File.Delete(fi.FullName);
                    }
                }
            }
            MessageBox.Show("解压完成  请自行复制 \n如有剩余压缩包 则为解压失败");
            System.Diagnostics.Process.Start(zip);
        }
    }
}

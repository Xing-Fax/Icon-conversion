using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Icon_conversion
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private void 取消_Click(object sender, RoutedEventArgs e)
        {
            BeginStoryboard((Storyboard)FindResource("自定义关闭"));
            自定义.IsChecked = false;
        }

        private void 自定义_Checked(object sender, RoutedEventArgs e)
        {
            BeginStoryboard((Storyboard)FindResource("自定义打开"));
            自定义.Content = "自定义";
            高.Text = null;
            宽.Text = null;
        }

        private void 主窗体_DragEnter(object sender, DragEventArgs e)
        {
            拖入文件.Visibility = Visibility.Visible;
        }

        private void 主窗体_DragLeave(object sender, DragEventArgs e)
        {
            拖入文件.Visibility = Visibility.Collapsed;
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { DragMove(); }
        }

        private void 关闭_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void 最小化_Click(object sender, RoutedEventArgs e)
        {
            主窗体.WindowState = WindowState.Minimized;
        }

        private void 图标_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/xingchuanzhen/");
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            主窗体.Topmost = (bool)前端显示.IsChecked;
        }

        private void 关闭关于_Click(object sender, RoutedEventArgs e)
        {
            BeginStoryboard((Storyboard)FindResource("关于关闭"));
        }

        private void 关于_Click(object sender, RoutedEventArgs e)
        {
            BeginStoryboard((Storyboard)FindResource("关于打开"));
        }

        private void 清空对象_Click(object sender, RoutedEventArgs e)
        {
            Number_file.Clear();
            文件列表.Items.Clear();
            文件下拉框.Header = "已选择0个对象";
            Assignment("清空文件列表");
        }

        private void 清除日志_Click(object sender, RoutedEventArgs e)
        {
            日志.Text = "[" + DateTime.Now.ToLongTimeString().ToString() + "]: 已经清空日志...";
        }

        private void 打开目录_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(out_file) == false)//判断输出目录是否存在
            {
                Assignment("输出目录不存在,重新创建目录");
                Directory.CreateDirectory(out_file);//创建新路径
            }
            Related_functions.CMD.RunCmd("explorer " + out_file + @"\");
        }

        private void 输出目录_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                CommonFileDialogResult result = dialog.ShowDialog();
                if (dialog.FileName != "")
                {
                    输出路径.Text = dialog.FileName;
                    out_file = dialog.FileName;
                    Assignment("重新定向输出文件夹：" + dialog.FileName);
                }
            }
            catch { }
        }
        /// <summary>
        /// 获取用户临时文件夹路径
        /// </summary>
        static string Temp_file = Environment.GetEnvironmentVariable("TMP");
        /// <summary>
        /// 获取桌面路径
        /// </summary>
        static string Desktop_file = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        /// <summary>
        /// 设定默认输出文件夹路径
        /// </summary>
        static string out_file = Desktop_file + @"\ICON输出";
        /// <summary>
        /// 用于存储已经添加的文件路径
        /// </summary>
        static ArrayList Number_file = new ArrayList();

        static System.Drawing.Size siz_customize = new System.Drawing.Size();

        public MainWindow()
        {
            InitializeComponent();

            //以下代码用作校验程序签名是否完整，调试时请将其注释

            if (Related_functions.check.Document_verification() != true)
            {
                //MessageBox.Show("签名校验失败,程序可能被篡改,轻击确定以退出程序", "警告");
                //Environment.Exit(0);
            }

            //到此结束
            日志.Text += "[" + DateTime.Now.ToLongTimeString().ToString() + "]: ICO图标转换程序启动  Copyright © xcz 2021";
            Assignment("获取临时文件夹：" + Temp_file);
            Assignment("获取桌面路径：" + Desktop_file);
            输出路径.Text = out_file;
            前端显示.IsChecked = true;
            主窗体.Topmost = true;
            Assignment("程序准备就绪...");
        }

        private void 添加文件_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "图像文件|*.jpg;*.png;*.bmp;*.jpeg";
            ofd.Multiselect = true;
            if (ofd.ShowDialog(this) == true)
            {
                foreach (string file in ofd.FileNames)
                {
                    if (Number_file.Contains(file) == false)//排除同类项
                    {
                        Number_file.Add(file);//将元素添加到数组末尾
                        文件列表.Items.Add(file);
                        Assignment("已添加文件：" + file);
                    }
                    else
                    {
                        Assignment("添加失败!原因：已存在此文件");
                    }
                }
            }
            文件下拉框.Header = "已选择" + Number_file.Count + "个对象";
        }

        private void 开始_Click(object sender, RoutedEventArgs e)
        {
            bool[] vs = new bool[6];
            vs[0] = (bool)尺寸16.IsChecked;
            vs[1] = (bool)尺寸32.IsChecked;
            vs[2] = (bool)尺寸48.IsChecked;
            vs[3] = (bool)尺寸128.IsChecked;
            vs[4] = (bool)尺寸255.IsChecked;
            vs[5] = (bool)自定义.IsChecked;

            if (Number_file .Count != 0)
            {
                bool temp = false;
                for(int i = 0; i < vs.Length;i ++)
                {
                    if (vs[i] == true) { temp = true; }
                }
                if (temp == false)
                {
                    Assignment("您还没有选择要转换的尺寸！");
                }
                else
                {
                    if (Directory.Exists(out_file) == false)//判断输出目录是否存在
                    {
                        Assignment("输出目录不存在,重新创建目录");
                        Directory.CreateDirectory(out_file);//创建新路径
                    }
                    进度条.IsIndeterminate = true;
                    using (BackgroundWorker bw = new BackgroundWorker())
                    {
                        bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                        bw.RunWorkerAsync(vs);
                    }
                }
            }
            else
            {
                Assignment("您还没有选择要转换的图片！");
            }
        }

        private void Assignment(string str)
        {
            Dispatcher.Invoke(new Action(delegate
            {
               日志.Text += "\n[" + DateTime.Now.ToLongTimeString().ToString() + "]: " + str;
               框.ScrollToVerticalOffset(框.ExtentHeight);
            }));
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)//后台
        {
            System.Drawing.Size size = new System.Drawing.Size();
            for (int i = 0;i < Number_file .Count;i ++)
            {

                bool temp;
                if (((bool[])e.Argument)[0])
                {
                    size.Height = 32;
                    size.Width = 32;
                    Assignment("开始执行第 " + (i + 1) + " 个文件" + "尺寸：" + size.Height + "X" + size.Width);
                    temp = Related_functions.Icon.ConvertImageToIcon(Number_file[i].ToString(), out_file + @"\" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + "_" + size.Height + "X" + size.Width + ".ico", size);
                    if (temp) { Assignment("文件：" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + " 执行成功！( •  ω •  )✧"); }
                    else { Assignment("文件：" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + " 执行失败！(っ °Д °;)っ"); }
                }

                if (((bool[])e.Argument)[1])
                {
                    size.Height = 64;
                    size.Width = 64;
                    Assignment("开始执行第 " + (i + 1) + " 个文件" + "尺寸：" + size.Height + "X" + size.Width);
                    temp = Related_functions.Icon.ConvertImageToIcon(Number_file[i].ToString(), out_file + @"\" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + "_" + size.Height + "X" + size.Width + ".ico", size);
                    if (temp) { Assignment("文件：" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + " 执行成功！( •  ω •  )✧"); }
                    else { Assignment("文件：" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + " 执行失败！(っ °Д °;)っ"); }
                }

                if (((bool[])e.Argument)[2])
                {
                    size.Height = 256;
                    size.Width = 256;
                    Assignment("开始执行第 " + (i + 1) + " 个文件" + "尺寸：" + size.Height + "X" + size.Width);
                    temp = Related_functions.Icon.ConvertImageToIcon(Number_file[i].ToString(), out_file + @"\" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + "_" + size.Height + "X" + size.Width + ".ico", size);
                    if (temp) { Assignment("文件：" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + " 执行成功！( •  ω •  )✧"); }
                    else { Assignment("文件：" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + " 执行失败！(っ °Д °;)っ"); }
                }

                if (((bool[])e.Argument)[3])
                {
                    size.Height = 512;
                    size.Width = 512;
                    Assignment("开始执行第 " + (i + 1) + " 个文件" + "尺寸：" + size.Height + "X" + size.Width);
                    temp = Related_functions.Icon.ConvertImageToIcon(Number_file[i].ToString(), out_file + @"\" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + "_" + size.Height + "X" + size.Width + ".ico", size);
                    if (temp) { Assignment("文件：" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + " 执行成功！( •  ω •  )✧"); }
                    else { Assignment("文件：" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + " 执行失败！(っ °Д °;)っ"); }
                }

                if (((bool[])e.Argument)[4])
                {
                    size.Height = 1024;
                    size.Width = 1024;
                    Assignment("开始执行第 " + (i + 1) + " 个文件" + "尺寸：" + size.Height + "X" + size.Width);
                    temp = Related_functions.Icon.ConvertImageToIcon(Number_file[i].ToString(), out_file + @"\" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + "_" + size.Height + "X" + size.Width + ".ico", size);
                    if (temp) { Assignment("文件：" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + " 执行成功！( •  ω •  )✧"); }
                    else { Assignment("文件：" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + " 执行失败！(っ °Д °;)っ"); }
                }

                if (((bool[])e.Argument)[5])
                {
                    Assignment("开始执行第 " + (i + 1) + " 个文件" + "尺寸：" + siz_customize.Width + "X" + siz_customize.Height);
                    temp = Related_functions.Icon.ConvertImageToIcon(Number_file[i].ToString(), out_file + @"\" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + "_" + siz_customize.Width + "X" + siz_customize.Height + ".ico", siz_customize);
                    if (temp) { Assignment("文件：" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + " 执行成功！( •  ω •  )✧"); }
                    else { Assignment("文件：" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()) + " 执行失败！(っ °Д °;)っ"); }
                }
            }
            Assignment("一共" + Number_file.Count.ToString() + "个文件执行完毕！(✿◕‿◕✿)");
            Dispatcher.Invoke(new Action(delegate{ 进度条.IsIndeterminate = false;}));
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (高.Text != string.Empty && 宽.Text != string.Empty)
                {
                    if (int.Parse(高.Text) > 5120 || int.Parse(宽.Text) > 5120)
                    {
                        警告.Content = "高宽不能大于 5120";
                        警告.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        siz_customize.Height = int.Parse(高.Text);
                        siz_customize.Width = int.Parse(宽.Text);
                        自定义.IsChecked = true;
                        自定义.Content = 高.Text + " X " + 宽.Text;
                        BeginStoryboard((Storyboard)FindResource("自定义关闭"));
                    }
                }
                else
                {
                    警告.Content = "高或宽不能为空！";
                    警告.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                警告.Content = "非法字符！";
                警告.Visibility = Visibility.Visible;
            }
        }

        private void 拖入文件_Drop(object sender, DragEventArgs e)
        {
            拖入文件.Visibility = Visibility.Collapsed;
            string[] filePath = (string[])e.Data.GetData(DataFormats.FileDrop);
            for (int i = 0; i < filePath.Length; i++)
            {
                if (Number_file.Contains(filePath[i]) == false)//排除同类项
                {
                    string temp = System.IO.Path.GetExtension(filePath[i]);
                    if (temp == ".jpg" || temp == ".png" || temp == ".bmp" || temp == ".jpeg")
                    {
                        Number_file.Add(filePath[i]);//将元素添加到数组末尾
                        文件列表.Items.Add(filePath[i]);
                        Assignment("已添加文件：" + filePath[i]);
                    }
                    else
                    {
                        Assignment("请添加格式为 jpg、png、bmp、jpeg 格式文件！");
                    }
                }
                else
                {
                    Assignment("添加失败!原因：已存在此文件");
                }
            }
            文件下拉框.Header = "已选择" + Number_file.Count + "个对象";
        }
    }
}

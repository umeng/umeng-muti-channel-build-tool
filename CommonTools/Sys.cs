using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace CommonTools
{
    /// <summary>
    /// utils facade !
    /// </summary>
    public class Sys
    {

        public static void Run(string cmd)
        {
            new SyncCmd().run(cmd);
        }

        public static void Run(string cmd, DataReceivedEventHandler handler)
        {
            new SyncCmd(handler).run(cmd);
        }
    }

    public class SyncCmd
    {
        private Process p = new Process();
     
        public SyncCmd(DataReceivedEventHandler hander = null)
        {
            p.OutputDataReceived += hander ?? new DataReceivedEventHandler(p_OutputDataReceived);
            p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);

            p.StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
            //设定程序名

            p.StartInfo.FileName = "cmd.exe";

            //关闭Shell的使用

            p.StartInfo.UseShellExecute = false;

            //重定向标准输入

            p.StartInfo.RedirectStandardInput = true;

            //重定向标准输出

            p.StartInfo.RedirectStandardOutput = true;

            //重定向错误输出

            p.StartInfo.RedirectStandardError = true;

            //设置不显示窗口

            p.StartInfo.CreateNoWindow = true;

        }

        public void run(string cmd)
        {
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.StandardInput.WriteLine(cmd + " > log\\i.txt 2> log\\e.txt");
            p.StandardInput.WriteLine("exit");
            p.WaitForExit();
        }

        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Log.i( e.Data);
        }

        void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Log.e( e.Data);
        }
    }
}
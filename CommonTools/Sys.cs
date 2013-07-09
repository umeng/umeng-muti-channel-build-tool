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

        public static void Run(string cmd, OnCmdExitHandler handler)
        {
            new LightSyncCmd(handler).run(cmd);
        }

        /// <summary>
        /// A sys method to judge wheather java has been installed!
        /// Use 'cmd: where java' to test any error output, if no error throws out,
        /// it proves that java exist.
        /// </summary>
        public static bool isJavaInstalled()
        {
            string cmd = "where java";
            bool hasJava = true;
            new LightSyncCmd((I,E) =>
            {
                if (!string.IsNullOrWhiteSpace(E))
                {
                    hasJava = false;
                }
            })
            .run( cmd );

            return hasJava;
        }
    }

    public class SyncCmd
    {
        private Process p = new Process();
     
        public SyncCmd(DataReceivedEventHandler hander = null)
        {
            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            p.ErrorDataReceived += hander ?? new DataReceivedEventHandler(p_ErrorDataReceived);

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
            Log.StaticCreateDirectory();

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
    public delegate void OnCmdExitHandler(string info, string error);
    public class LightSyncCmd
    {
        private Process p = new Process();

       
        private OnCmdExitHandler exitHandler;
       
        private StringBuilder info = new StringBuilder();
        private StringBuilder error = new StringBuilder();

        public LightSyncCmd(OnCmdExitHandler handler1)
        {
            exitHandler = handler1;

            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
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

        void p_Exited()
        {
            exitHandler( info.ToString() , error.ToString());
        }

        public void run(string cmd)
        {
            Log.StaticCreateDirectory();

            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.StandardInput.WriteLine( cmd );
            p.StandardInput.WriteLine("exit");
            p.WaitForExit();

            p_Exited();
        }

        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            info.Append(e.Data);
        }

        void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            error.Append(e.Data);
        }
    }

}
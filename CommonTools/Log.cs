/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2012/7/17
 * Time: 17:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.IO;

namespace CommonTools
{
	/// <summary>
	/// Description of Log.
	/// </summary>
	public class Log
	{
        public static bool LOG = true;

		private static readonly string ii = null;
        private static readonly string ee = null;
		
		static Log()
		{
			if(!Directory.Exists(Path.Combine(System.Environment.CurrentDirectory, "log")))
			{
				Directory.CreateDirectory(Path.Combine(System.Environment.CurrentDirectory, "log"));
			}
			
			ii = Path.Combine(System.Environment.CurrentDirectory,
                Path.Combine("log" ,"i.txt"));
            ee = Path.Combine(System.Environment.CurrentDirectory,
                Path.Combine("log", "e.txt"));
			
			if(File.Exists(ii) && (new FileInfo(ii).Length > 1024*1024))
			{
				File.Delete(ii);
			}

            if (File.Exists(ee) && (new FileInfo(ee).Length > 1024 * 1024))
            {
                File.Delete(ii);
            }
		}
		
		public static void d(string debug){
            if (LOG)
            {
                Debug.WriteLine("DEBUG:" + debug);
            }
		}
		public static void i(string info){
            if (LOG)
            {
                Debug.WriteLine("INFO:" + info);
            }
		}
		
		public static void w(string warning){
            if (LOG)
            {
                Debug.WriteLine("WARNING:" + warning);
            }
		}
		
		public static void e(string error){
            if (LOG)
            {
                Debug.WriteLine("ERROR:" + error);
            }
		}
		
		public static void e(Exception e){
            if (LOG)
            {
                Debug.WriteLine("ERROR:" + e.Message);
                Debug.WriteLine("ERROR:" + e.StackTrace);
            }
		}
	}
}

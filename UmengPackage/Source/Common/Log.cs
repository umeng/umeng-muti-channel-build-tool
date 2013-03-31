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

namespace UmengPackage.Source.Common
{
	/// <summary>
	/// Description of Log.
	/// </summary>
	public class Log
	{
		private static readonly string ii = null;
		//private static readonly string ee = "e.txt";
		
		private static StreamWriter sw = null;
		
		static Log()
		{
			if(!Directory.Exists(Path.Combine(System.Environment.CurrentDirectory, "log")))
			{
				Directory.CreateDirectory(Path.Combine(System.Environment.CurrentDirectory, "log"));
			}
			
			ii = Path.Combine(System.Environment.CurrentDirectory,
                Path.Combine("log" ,"i.txt"));
			
			if(File.Exists(ii) && (new FileInfo(ii).Length > 1024*1024))
			{
				File.Delete(ii);
			}
			sw = File.AppendText(ii);
		}
		
		public static void d(int debug){
			Debug.WriteLine("DEBUG:"+ debug);
		}
		public static void i(string info){
			Debug.WriteLine("INFO:" + info);
			
			sw.WriteLine("INFO:" + info);
			sw.Flush();
		}
		
		public static void w(string warning){
			Debug.WriteLine("WARNING:" + warning);
		}
		
		public static void e(string error){
			Debug.WriteLine("ERROR:" + error);
			
			sw.WriteLine("ERROR" + error);
			sw.Flush();
		}
		
		public static void e(Exception e){
			Debug.WriteLine("ERROR:" + e.Message);
		}
	}
}

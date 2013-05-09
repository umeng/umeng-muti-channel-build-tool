/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2012/7/18
 * Time: 13:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text.RegularExpressions;
using System.IO;

namespace UmengChannel
{
	/// <summary>
	/// Description of Utils.
	/// </summary>
	public class Utils
	{
		public Utils()
		{
		}
		
		public static bool isValidChannal(string channel){
			return (channel.Length < 128);
		}
		
		public static bool isValidAndroidProject(string path){
			string [] files = {"AndroidManifest.xml"};
			string file_path;
			foreach (string file in files){
				file_path = System.IO.Path.Combine(path, file);
				if(!System.IO.File.Exists(file_path)) 
					return false;
			}
			return true;
		}
		
		public static bool isValidAndroidSDKPath(string path){
			string [] folders = {"tools","platforms"};
			string folder_path;
			
			foreach(string folder in folders){
				folder_path  = Path.Combine(path, folder);
				
				if(!Directory.Exists( folder_path)){
				 	return false;  	
				}
			}
			
			return true;
		}
		
		
		public static bool isValidJavaSDKPath(string path){
			string bin = System.IO.Path.Combine(path,"bin");
			string lib = System.IO.Path.Combine(path,"lib");
			
			if(Directory.Exists(bin) && Directory.Exists(lib)){
				return true;
			}
			
			return false;
		}
		
		public static string generateSettingFileName(string project)
		{
			return string.Format("setting@{0}.xml",project);
		}	
	}
}

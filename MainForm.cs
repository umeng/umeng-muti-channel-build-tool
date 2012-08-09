/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2012/7/17
 * Time: 13:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.IO;

namespace UmengChannel
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		ProjectConfigration project = null;
		BackgroundWorker bw = new BackgroundWorker();
		//private string currentProject = null;
		
		public MainForm()
		{
            Application.ApplicationExit += new EventHandler(this.Application_ApplicationExit);
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
	
			refreshProjects();
			
			project = Configration.Instanse().getDefaultProject();
			
			bindProjectConfig();
			bindGeneralConfig();
			
			initBackgroundWorker();
    
		}
		//set backgroundworker for background task!
		private void initBackgroundWorker(){
			bw.WorkerReportsProgress = true;
			bw.WorkerSupportsCancellation = true;
			bw.DoWork += new DoWorkEventHandler(doWork);
			bw.ProgressChanged +=  new ProgressChangedEventHandler(bw_ProgressChanged);
			bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
		}
		
		private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.progressBar1.Visible = false;
			
		    if ((e.Cancelled == true))
		    {
		    	MessageBox.Show("任务已经取消");
		    }
		
		    else if (!(e.Error == null))
		    {
		    	System.Text.StringBuilder sb = new System.Text.StringBuilder();
		    	sb.Append(e.Error.Message);
		    	sb.Append("\n\n");
		    	sb.Append("查看 /log/i.txt 详细错误信息");
		    	MessageBox.Show(sb.ToString());
		    }
		
		    else
		    {
		    	MessageBox.Show( "渠道打包完成" );
		    }
		}
		
		private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.progressBar1.Value = e.ProgressPercentage;
			Log.i("progress:"+ e.ProgressPercentage);
		}

		//
		private void bindGeneralConfig(){
			this.tb_java_path.DataBindings.Clear();
			this.tb_java_path.DataBindings.Add("Text",Configration.Instanse(),"java_home",false,DataSourceUpdateMode.OnPropertyChanged);
			this.tb_android_sdk_path.DataBindings.Clear();
			this.tb_android_sdk_path.DataBindings.Add("Text",Configration.Instanse(),"android_home",false,DataSourceUpdateMode.OnPropertyChanged);
		}
		//projects -> view
		private void refreshProjects(){
			if(Configration.Instanse().projects.Count <= 0){
				this.projects.Items.Clear();
				return;
			}
			
			this.projects.BeginUpdate();
			
			this.projects.Items.Clear();
			foreach(string key in Configration.Instanse().projects.Keys){
				this.projects.Items.Add(key);
			}
			
			this.projects.EndUpdate();
		}
		//project -> view
		private void bindProjectConfig(){

			//if(this)
			this.tb_project.DataBindings.Clear();
			this.tb_project.DataBindings.Add("Text", project,"project_path",false,DataSourceUpdateMode.OnPropertyChanged);
			this.checkBox1.DataBindings.Clear();
			this.checkBox1.DataBindings.Add("Checked",project,"setProguard",false,DataSourceUpdateMode.OnPropertyChanged);
			this.tb_alias.DataBindings.Clear();
			this.tb_alias.DataBindings.Add("Text", project,"alias",false,DataSourceUpdateMode.OnPropertyChanged);
			this.tb_keystore.DataBindings.Clear();
			this.tb_keystore.DataBindings.Add("Text",project,"keystore_file_path",false,DataSourceUpdateMode.OnPropertyChanged);
			this.tb_keystore_pw.DataBindings.Clear();
			this.tb_keystore_pw.DataBindings.Add("Text",project,"keystore_pw",false,DataSourceUpdateMode.OnPropertyChanged);
			this.tb_key_pw.DataBindings.Clear();
			this.tb_key_pw.DataBindings.Add("Text",project,"key_pw",false,DataSourceUpdateMode.OnPropertyChanged);
			this.channels.DataBindings.Clear();
			this.channels.DataBindings.Add("DataSource",project,"channels",false,DataSourceUpdateMode.OnPropertyChanged);
		}

		//view to object
		private void setOrUpdateConfig(){
			string project_name = Utils.getAndroidProjectName(this.tb_project.Text);
	
			ProjectConfigration config = Configration.Instanse().getOrCreateProject(project_name);
			config.project_path = this.tb_project.Text;
			config.keystore_file_path = this.tb_keystore.Text;
			config.keystore_pw = this.tb_keystore_pw.Text;
			config.key_pw = this.tb_key_pw.Text;
			config.alias = this.tb_alias.Text;
			
		}
		
		private void addChannel(string channel){	
			// Insert the string at the front of the List.
			if(!project.addChannel(channel)){
				MessageBox.Show("The channel already exit!");
				return;
			}
    		// Force a refresh of the ListBox.
    		((CurrencyManager)channels.BindingContext[project.channels]).Refresh();
		}
		
		private void deleteChannel(string channel){
			if(project.removeChannle(channel)){
				// Force a refresh of the ListBox.
    			((CurrencyManager)channels.BindingContext[project.channels]).Refresh();
			}
		}
		
		//generate apk
		void Button3Click(object sender, EventArgs e)
		{
			//loadConfig();
			if( !isEnviromentReady(project) ){
				return;
			}
			
			if( bw.IsBusy ){
				MessageBox.Show("正在打包，稍后再试");
				return;
			}
			
			progressBar1.Visible = true;
			
			bw.RunWorkerAsync();
		}
		
		private void doWork(object sender, DoWorkEventArgs e){
			try{
				BackgroundWorker worker = sender as BackgroundWorker;
				Worker.setProject(project, worker);
				Worker.start();
			}catch(Exception ex){
				throw ex;
			}
		}
		
		//delete channel
		void Button1Click(object sender, EventArgs e)
		{
			if(channels.SelectedIndex < 0 || channels.SelectedIndex >= project.channels.Count ) 
			{
				return;
			}
			deleteChannel(project.channels[channels.SelectedIndex]);
		}
		//input channel(enter!)
		void TextBox5KeyDown(object sender, KeyEventArgs e)
		{
			TextBox tb_channel = sender as TextBox;
			
			if (e.KeyCode == Keys.Enter ){
				string channel = tb_channel.Text;
				if(!string.IsNullOrEmpty(channel) && Utils.isValidChannal(channel)){
					addChannel(channel);
					this.tb_input_channel_area.Text = string.Empty;
				}else{
					Log.w("invalid channel ");
					MessageBox.Show("invalid channel" + channel);
				}
  			}
			
		}
		
		void Tb_input_channel_areaEnter(object sender, EventArgs e)
		{
			this.label_hint.Visible = false;
		}
		
		void Tb_input_channel_areaLeave(object sender, EventArgs e)
		{
			if(string.IsNullOrEmpty(tb_input_channel_area.Text))
			{
				this.label_hint.Visible = true;
			}
		}
		
		//open the apks folder
		void Button2Click(object sender, EventArgs e)
		{
			string path = System.IO.Path.Combine(System.Environment.CurrentDirectory,
                Path.Combine("output", Utils.getAndroidProjectName(project.project_path)));
			System.Diagnostics.Process.Start("explorer.exe", path);
		}
		//open project path
		void Bt_open_projectClick(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK){
				string path = null;
				if(Utils.isValidAndroidProject(path = folderBrowserDialog1.SelectedPath)){	
					OnGenerateProject(path);
				}else{
					MessageBox.Show("请选择 Android 工程根目录");
				}
			}
		}
		//open keystore file
		void Bt_keystore_pathClick(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			
			openFileDialog1.DefaultExt = "keystore";
        	openFileDialog1.Filter = "keystore files (*.*)|*.*";
        	
        	if( openFileDialog1.ShowDialog() == DialogResult.OK)
        	{
        		this.tb_keystore.Text = openFileDialog1.FileName;
        	}
        	
		}
		
		//open java sdk path
		void Bt_java_pathClick(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK){
				string path = null;
				if(Utils.isValidJavaSDKPath(path = folderBrowserDialog1.SelectedPath)){	
					this.tb_java_path.Text = path;
				}else{
					MessageBox.Show("请选择 JDK 工程根目录( 包涵 lin ,bin 等子目录)");
				}
			}
		}
		
		void Bt_android_sdk_pathClick(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK){
				string path = null;
				if(Utils.isValidAndroidSDKPath(path = folderBrowserDialog1.SelectedPath)){	
					this.tb_android_sdk_path.Text = path;
				}else{
					MessageBox.Show("请选择 Android SDK 目录( 包涵 platform , tools 等子目录)");
				}
			}
		}
		
		void ProjectsSelectedIndexChanged(object sender, EventArgs e)
		{
			if(projects.SelectedIndex < 0) return;
			
			string project_name = projects.Items[projects.SelectedIndex] as string;
			
			project = Configration.Instanse().updateCurrentProject(project_name);
			
			bindProjectConfig();
		}
		
		private void OnGenerateProject(string path){
			string new_project = Utils.getAndroidProjectName(path);
			project = Configration.Instanse().addProject(new_project);
			project.project_path = path;
			
			refreshProjects();
			bindProjectConfig();
		}
	
		private void Application_ApplicationExit(object sender, EventArgs e) {
		    try {
				Configration.Instanse().saveProjects();
				Configration.Instanse().saveSysConfig();
		    } catch {}
		}
		
		public bool isEnviromentReady(ProjectConfigration project){
			string error = null;
			if(project.project_path == null){
				Log.e("Please set the project path");
				error = "工程目录没有设置";
			
			}
			
			if(!Directory.Exists(project.project_path)){
				Log.e("The input project path does't exit");
				error = "工程目录不存在";
				
			}else{
				Log.i("Target project is OK ... ");
			}
			
			if(project.keystore_file_path == null){
				Log.e("Please set the keystore file path");
				error = "密钥文件没设置";
				
			}
			if(!File.Exists(project.keystore_file_path)){
				Log.e("The input keystore file doesn't exit");
				error = "密钥文件不存在";
				
			}else{
				Log.i("Target keystore file is OK ...");
			}
			
			if(project.keystore_pw == null){
				error = "密钥库密码没设置";
				Log.e("The input keystore password is null");
				
			}
			
			if(project.key_pw == null){
				error = "密钥密码没设置";
				Log.e("The input key password is null");
				
			}
			
			if(project.alias == null){
				error = "密钥别名没有设置";
				Log.e("The input alias is null");
				
			}
			
			if(project.channels == null || project.channels.Count <=0){
				error = "渠道没有设置";
				Log.w("Please input channels !");
				
			}

			
			if(string.IsNullOrEmpty(Configration.Instanse().java_home))
			{
				error = "JDK 路径没有设置";
			}
			
			if(string.IsNullOrEmpty(Configration.Instanse().android_home))
			{
				error = "Android SDK 路径没有设置";
			}
			
			if(error != null)
			{
				MessageBox.Show(error);
				return false;
			}
			
			Configration.Instanse().setEnvironment();
			
			//if(android_sdk_path == null)
			//if(java sdk path !
			return true;
		}
		
		
		void Bt_delete_projectClick(object sender, EventArgs e)
		{
			deleteProject(projects.SelectedIndex);
		}
		
		private void deleteProject(int index)
		{
			if(index < 0) return;
			try{
				project = Configration.Instanse().deleteProject(index);
			}catch{}
			
			refreshProjects();
			bindProjectConfig();
		}
		
		void Label8Click(object sender, EventArgs e)
		{
			
		}
		
		void Label6Click(object sender, EventArgs e)
		{
			
		}
		
		void Label_hintClick(object sender, EventArgs e)
		{
			
		}
		
		void Label9Click(object sender, EventArgs e)
		{
			
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			
		}
		
		void FontDialog1Apply(object sender, EventArgs e)
		{
			
		}
	}
}

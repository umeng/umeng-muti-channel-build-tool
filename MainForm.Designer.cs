/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2012/7/17
 * Time: 13:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace UmengChannel
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.projects = new System.Windows.Forms.ListBox();
			this.tb_project = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.keystore_file_path = new System.Windows.Forms.Label();
			this.tb_alias = new System.Windows.Forms.TextBox();
			this.keystore_pw = new System.Windows.Forms.Label();
			this.tb_keystore = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tb_keystore_pw = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.channels = new System.Windows.Forms.ListBox();
			this.button3 = new System.Windows.Forms.Button();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.fontDialog1 = new System.Windows.Forms.FontDialog();
			this.label7 = new System.Windows.Forms.Label();
			this.key_pw = new System.Windows.Forms.Label();
			this.tb_key_pw = new System.Windows.Forms.TextBox();
			this.bt_open_project = new System.Windows.Forms.Button();
			this.bt_keystore_path = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.javahome = new System.Windows.Forms.Label();
			this.tb_java_path = new System.Windows.Forms.TextBox();
			this.bt_java_path = new System.Windows.Forms.Button();
			this.bt_delete_project = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.tb_android_sdk_path = new System.Windows.Forms.TextBox();
			this.bt_android_sdk_path = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// projects
			// 
			this.projects.FormattingEnabled = true;
			this.projects.ItemHeight = 12;
			this.projects.Location = new System.Drawing.Point(1, 29);
			this.projects.Name = "projects";
			this.projects.Size = new System.Drawing.Size(137, 400);
			this.projects.TabIndex = 0;
			this.projects.SelectedIndexChanged += new System.EventHandler(this.ProjectsSelectedIndexChanged);
			// 
			// tb_project
			// 
			this.tb_project.Location = new System.Drawing.Point(253, 53);
			this.tb_project.Name = "tb_project";
			this.tb_project.ReadOnly = true;
			this.tb_project.Size = new System.Drawing.Size(168, 21);
			this.tb_project.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(163, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(73, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "工程目录：";
			// 
			// keystore_file_path
			// 
			this.keystore_file_path.Location = new System.Drawing.Point(163, 128);
			this.keystore_file_path.Name = "keystore_file_path";
			this.keystore_file_path.Size = new System.Drawing.Size(74, 23);
			this.keystore_file_path.TabIndex = 3;
			this.keystore_file_path.Text = "alias";
			// 
			// tb_alias
			// 
			this.tb_alias.Location = new System.Drawing.Point(253, 125);
			this.tb_alias.Name = "tb_alias";
			this.tb_alias.Size = new System.Drawing.Size(193, 21);
			this.tb_alias.TabIndex = 4;
			// 
			// keystore_pw
			// 
			this.keystore_pw.Location = new System.Drawing.Point(164, 156);
			this.keystore_pw.Name = "keystore_pw";
			this.keystore_pw.Size = new System.Drawing.Size(73, 23);
			this.keystore_pw.TabIndex = 5;
			this.keystore_pw.Text = "keystore";
			// 
			// tb_keystore
			// 
			this.tb_keystore.Location = new System.Drawing.Point(253, 153);
			this.tb_keystore.Name = "tb_keystore";
			this.tb_keystore.ReadOnly = true;
			this.tb_keystore.Size = new System.Drawing.Size(168, 21);
			this.tb_keystore.TabIndex = 6;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(164, 194);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(73, 23);
			this.label4.TabIndex = 7;
			this.label4.Text = "keystore_pw";
			this.label4.Click += new System.EventHandler(this.Label4Click);
			// 
			// tb_keystore_pw
			// 
			this.tb_keystore_pw.Location = new System.Drawing.Point(253, 191);
			this.tb_keystore_pw.Name = "tb_keystore_pw";
			this.tb_keystore_pw.Size = new System.Drawing.Size(193, 21);
			this.tb_keystore_pw.TabIndex = 8;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(145, 17);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(148, 24);
			this.label5.TabIndex = 9;
			this.label5.Text = "工程配置";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(481, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 10;
			this.button1.Text = "删除渠道";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(562, 12);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(134, 23);
			this.button2.TabIndex = 11;
			this.button2.Text = "打开文件夹";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// channels
			// 
			this.channels.FormattingEnabled = true;
			this.channels.ItemHeight = 12;
			this.channels.Location = new System.Drawing.Point(487, 53);
			this.channels.Name = "channels";
			this.channels.Size = new System.Drawing.Size(209, 328);
			this.channels.TabIndex = 12;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(164, 367);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(282, 59);
			this.button3.TabIndex = 13;
			this.button3.Text = "开始打包";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.Button3Click);
			// 
			// textBox5
			// 
			this.textBox5.Location = new System.Drawing.Point(487, 390);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(209, 21);
			this.textBox5.TabIndex = 14;
			this.textBox5.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox5KeyDown);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(1, 9);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 17);
			this.label6.TabIndex = 15;
			this.label6.Text = "最近";
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(145, 7);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(330, 5);
			this.progressBar1.TabIndex = 16;
			this.progressBar1.Visible = false;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(145, 104);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(100, 23);
			this.label7.TabIndex = 17;
			this.label7.Text = "一般配置";
			// 
			// key_pw
			// 
			this.key_pw.Location = new System.Drawing.Point(164, 221);
			this.key_pw.Name = "key_pw";
			this.key_pw.Size = new System.Drawing.Size(86, 23);
			this.key_pw.TabIndex = 18;
			this.key_pw.Text = "key_pw";
			// 
			// tb_key_pw
			// 
			this.tb_key_pw.Location = new System.Drawing.Point(253, 222);
			this.tb_key_pw.Name = "tb_key_pw";
			this.tb_key_pw.Size = new System.Drawing.Size(193, 21);
			this.tb_key_pw.TabIndex = 19;
			// 
			// bt_open_project
			// 
			this.bt_open_project.Location = new System.Drawing.Point(428, 53);
			this.bt_open_project.Name = "bt_open_project";
			this.bt_open_project.Size = new System.Drawing.Size(35, 23);
			this.bt_open_project.TabIndex = 20;
			this.bt_open_project.Text = "....";
			this.bt_open_project.UseVisualStyleBackColor = true;
			this.bt_open_project.Click += new System.EventHandler(this.Bt_open_projectClick);
			// 
			// bt_keystore_path
			// 
			this.bt_keystore_path.Location = new System.Drawing.Point(428, 156);
			this.bt_keystore_path.Name = "bt_keystore_path";
			this.bt_keystore_path.Size = new System.Drawing.Size(35, 23);
			this.bt_keystore_path.TabIndex = 21;
			this.bt_keystore_path.Text = "....";
			this.bt_keystore_path.UseVisualStyleBackColor = true;
			this.bt_keystore_path.Click += new System.EventHandler(this.Bt_keystore_pathClick);
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(385, 80);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(61, 24);
			this.checkBox1.TabIndex = 22;
			this.checkBox1.Text = "混淆";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(145, 270);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 23;
			this.label2.Text = "系统配置";
			// 
			// javahome
			// 
			this.javahome.Location = new System.Drawing.Point(164, 297);
			this.javahome.Name = "javahome";
			this.javahome.Size = new System.Drawing.Size(86, 23);
			this.javahome.TabIndex = 24;
			this.javahome.Text = "Java 路径";
			// 
			// tb_java_path
			// 
			this.tb_java_path.Location = new System.Drawing.Point(253, 297);
			this.tb_java_path.Name = "tb_java_path";
			this.tb_java_path.ReadOnly = true;
			this.tb_java_path.Size = new System.Drawing.Size(168, 21);
			this.tb_java_path.TabIndex = 25;
			// 
			// bt_java_path
			// 
			this.bt_java_path.Location = new System.Drawing.Point(428, 297);
			this.bt_java_path.Name = "bt_java_path";
			this.bt_java_path.Size = new System.Drawing.Size(35, 23);
			this.bt_java_path.TabIndex = 26;
			this.bt_java_path.Text = "...";
			this.bt_java_path.UseVisualStyleBackColor = true;
			this.bt_java_path.Click += new System.EventHandler(this.Bt_java_pathClick);
			// 
			// bt_delete_project
			// 
			this.bt_delete_project.Location = new System.Drawing.Point(63, 5);
			this.bt_delete_project.Name = "bt_delete_project";
			this.bt_delete_project.Size = new System.Drawing.Size(75, 20);
			this.bt_delete_project.TabIndex = 27;
			this.bt_delete_project.Text = "删除工程";
			this.bt_delete_project.UseVisualStyleBackColor = true;
			this.bt_delete_project.Click += new System.EventHandler(this.Bt_delete_projectClick);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(164, 324);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(73, 23);
			this.label3.TabIndex = 28;
			this.label3.Text = "Android SDK 路径";
			// 
			// tb_android_sdk_path
			// 
			this.tb_android_sdk_path.Location = new System.Drawing.Point(253, 325);
			this.tb_android_sdk_path.Name = "tb_android_sdk_path";
			this.tb_android_sdk_path.ReadOnly = true;
			this.tb_android_sdk_path.Size = new System.Drawing.Size(168, 21);
			this.tb_android_sdk_path.TabIndex = 29;
			// 
			// bt_android_sdk_path
			// 
			this.bt_android_sdk_path.Location = new System.Drawing.Point(428, 323);
			this.bt_android_sdk_path.Name = "bt_android_sdk_path";
			this.bt_android_sdk_path.Size = new System.Drawing.Size(35, 23);
			this.bt_android_sdk_path.TabIndex = 30;
			this.bt_android_sdk_path.Text = "...";
			this.bt_android_sdk_path.UseVisualStyleBackColor = true;
			this.bt_android_sdk_path.Click += new System.EventHandler(this.Bt_android_sdk_pathClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(707, 438);
			this.Controls.Add(this.bt_android_sdk_path);
			this.Controls.Add(this.tb_android_sdk_path);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.bt_delete_project);
			this.Controls.Add(this.bt_java_path);
			this.Controls.Add(this.tb_java_path);
			this.Controls.Add(this.javahome);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.bt_keystore_path);
			this.Controls.Add(this.bt_open_project);
			this.Controls.Add(this.tb_key_pw);
			this.Controls.Add(this.key_pw);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textBox5);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.channels);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.tb_keystore_pw);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.tb_keystore);
			this.Controls.Add(this.keystore_pw);
			this.Controls.Add(this.tb_alias);
			this.Controls.Add(this.keystore_file_path);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tb_project);
			this.Controls.Add(this.projects);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "MainForm";
			this.Text = "UmengChannel";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TextBox tb_android_sdk_path;
		private System.Windows.Forms.Button bt_android_sdk_path;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button bt_delete_project;
		private System.Windows.Forms.Button bt_java_path;
		private System.Windows.Forms.TextBox tb_java_path;
		private System.Windows.Forms.Label javahome;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Button bt_keystore_path;
		private System.Windows.Forms.Button bt_open_project;
		private System.Windows.Forms.TextBox tb_key_pw;
		private System.Windows.Forms.Label key_pw;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.FontDialog fontDialog1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.ListBox channels;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tb_keystore_pw;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tb_keystore;
		private System.Windows.Forms.Label keystore_pw;
		private System.Windows.Forms.TextBox tb_alias;
		private System.Windows.Forms.Label keystore_file_path;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tb_project;
		private System.Windows.Forms.ListBox projects;
		
		void Label4Click(object sender, System.EventArgs e)
		{
			
		}
	}
}

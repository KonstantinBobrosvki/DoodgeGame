/*
 * Created by SharpDevelop.
 * User: USER
 * Date: 30.01.2019
 * Time: 16:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Miss
{
	partial class StartForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TextBox name;
		private System.Windows.Forms.ComboBox ColorChoose;
		private System.Windows.Forms.Button Connect;
		private System.Windows.Forms.Button Host;
		
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
            this.name = new System.Windows.Forms.TextBox();
            this.ColorChoose = new System.Windows.Forms.ComboBox();
            this.Connect = new System.Windows.Forms.Button();
            this.Host = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // name
            // 
            this.name.Font = new System.Drawing.Font("Microsoft YaHei", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.name.Location = new System.Drawing.Point(42, 71);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(243, 46);
            this.name.TabIndex = 0;
            this.name.Text = "Твоето име";
            this.name.TextChanged += new System.EventHandler(this.NameTextChanged);
            // 
            // ColorChoose
            // 
            this.ColorChoose.FormattingEnabled = true;
            this.ColorChoose.Location = new System.Drawing.Point(42, 176);
            this.ColorChoose.Name = "ColorChoose";
            this.ColorChoose.Size = new System.Drawing.Size(130, 21);
            this.ColorChoose.TabIndex = 1;
            this.ColorChoose.Text = "Твоят цвят";
            // 
            // Connect
            // 
            this.Connect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Connect.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Connect.Location = new System.Drawing.Point(13, 483);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(329, 173);
            this.Connect.TabIndex = 2;
            this.Connect.Text = "Свържи се към игра\r\n";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.ConnectClick);
            // 
            // Host
            // 
            this.Host.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Host.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Host.Location = new System.Drawing.Point(1040, 483);
            this.Host.Name = "Host";
            this.Host.Size = new System.Drawing.Size(329, 173);
            this.Host.TabIndex = 3;
            this.Host.Text = "Стартирай игра\r\n";
            this.Host.UseVisualStyleBackColor = true;
            this.Host.Click += new System.EventHandler(this.HostClick);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1381, 668);
            this.Controls.Add(this.Host);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.ColorChoose);
            this.Controls.Add(this.name);
            this.Name = "StartForm";
            this.Text = "StartForm";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}

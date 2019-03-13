/*
 * Created by SharpDevelop.
 * User: USER
 * Date: 30.01.2019
 * Time: 16:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Input;
using System.Collections.Generic;

namespace Miss
{
	/// <summary>
	/// Description of StartForm.
	/// </summary>
	public partial class StartForm : Form
	{
		
	
		public StartForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			this.WindowState=FormWindowState.Maximized;
	
		this.FormBorderStyle=FormBorderStyle.None;
	//Fill Chosse color for player
    foreach (System.Reflection.PropertyInfo prop in typeof(Color).GetProperties())  
    {     
         if (prop.PropertyType.FullName == "System.Drawing.Color")  
                 ColorChoose.Items.Add(prop.Name);  
    }  

		}
		
		void NameTextChanged(object sender, EventArgs e)
		{
	
		}
		void HostClick(object sender, EventArgs e)
		{
			DialogResult local=MessageBox.Show("Тази игра ще се играе на един компютър ?","Тип игра",MessageBoxButtons.YesNo);
	if (local==DialogResult.Yes) {
				LocalClick();
	}

			else
			{
                try
                {   
                    if(ColorChoose.SelectedItem!=null)
                    Controller.Web.MainPlayer=new Player(Color.FromName(ColorChoose.SelectedItem.ToString() ), name.Text);
                    else
                    {
                        MessageBox.Show("Choose Color");
                        return;
                    }
                    Controller.Web.Hoster = true;
                    Controller.Web.Start();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.StackTrace);
                }
				this.Hide();
			}
		}
		void ConnectClick(object sender, EventArgs e)
		{
            if (ColorChoose.SelectedItem != null)
                Controller.Web.MainPlayer = new Player(Color.FromName(ColorChoose.SelectedItem.ToString()), name.Text);
            else
            {
                MessageBox.Show("Избери цвят");
                return;
            }
            Controller.Web.Hoster = false;
            Controller.Web.Start();

            
            this.Hide();
		}
		
		
		//For Local game part go to StartFormLocal.cs
		
	}
}

/*
 * Created by SharpDevelop.
 * User: USER
 * Date: 23.01.2019
 * Time: 18:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
namespace Miss
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();


        

         this.DoubleBuffered=true;

          this.FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

           
	
		}
	
		
	
	}
}

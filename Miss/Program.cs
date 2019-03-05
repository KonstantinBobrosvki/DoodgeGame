/*
 * Created by SharpDevelop.
 * User: USER
 * Date: 23.01.2019
 * Time: 18:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;

namespace Miss
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
        /// <summary>
        /// Program entry point.
        /// </summary>
        ///  
         public static StartForm z;
        [STAThread]
     
		private static void Main(string[] args)
		{
            

            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            z = new StartForm();
            Application.Run(z);
		}
		
	}
}

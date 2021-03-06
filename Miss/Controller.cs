﻿/*
 * Created by SharpDevelop.
 * User: USER
 * Date: 23.01.2019
 * Time: 18:35
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
    /// Description of Controller.
    /// </summary>
    public  static partial class Controller
    {
       
        //on this tick we draw and update Screen
        static Timer Frame;
        
        //On this tick we add new ball
        static Timer BallAdd;

        //Form where we draw
        static Form screen;

        static Random rnd;
      
        //Elemets what we will draw
        static List<IDrawer> ToDraw;

        static Controller()
        {
            Frame = new Timer();
            Frame.Interval = 1000 / 24;
           
            BallAdd = new Timer();
            BallAdd.Interval = 4000;
           
            rnd = new Random();
            ToDraw = new List<IDrawer>();
          
        }

        
        //set form where we draw
		public static void SetScreen(Form f)
		{
			screen=f;
            screen.FormClosed += CloseAplication;
            screen.Paint += Draw;

            // this is not working I dont know why
            // Balls.SetBounds(screen.Width, screen.Height);

            //this is working perfect
            Balls.SetBounds(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

        }

        //Close all windows of Aplication
        private static void CloseAplication(object sender,EventArgs e)
        {
            if (Web.MainPlayer != null)
                Web.Exit();
            Program.z.Close();
        }

        //Drawing on Form
        private static void Draw(object Sender, PaintEventArgs e)
        {
            var g = e.Graphics;
           

            foreach (var item in ToDraw)
            {
                item.Draw(g);
            }
        }

       
        
        private static bool IsOpenedDialogForExit = true;

        //Call drawing on form
        //It must be called on every tick without event
        private static void FrameTick()
		{
            #region Check for esc for aplication
            if (Keyboard.IsKeyDown(Key.Escape))
            {
               
                if (IsOpenedDialogForExit)
                {
                    IsOpenedDialogForExit = false;
                    DialogResult d = MessageBox.Show("Искате ли да излезете?", "Сигурни ли сте, че искате да излезете?", MessageBoxButtons.YesNo);
                    if (d == DialogResult.Yes)
                    {
                        Program.z.Close();
                        IsOpenedDialogForExit = true;
                    }
                    else
                    {
                        IsOpenedDialogForExit = true ;
                    }
                }
            }
            #endregion
            screen.Invalidate();
		}
        public static void Clear()
        {
            ToDraw.Clear();
        }
		
	   
		
	}
}

/*
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
        static Player p;
        static Timer Frame;
        static Timer BallAdd;
        static Form screen;
        static Random rnd;
      public  static bool Hoster;
        static List<IDrawer> ToDraw;
        static Controller()
        {
            Frame = new Timer();
            Frame.Interval = 1000 / 24;
            Frame.Tick += FrameTick;
            BallAdd = new Timer();
            BallAdd.Interval = 4000;
            BallAdd.Tick += NewBall;
            rnd = new Random();
            ToDraw = new List<IDrawer>();
            //Balls.SetBounds(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

        }
        public static Player MainPlayer
        {
            get
            {
                return p;
            }
            set
            {
               
                p = value;
                ToDraw.Add(p);
            }
        }
        public static void Start()
        {
            MainForm fom = new MainForm();
            SetScreen(fom);
            Balls.SetBounds(fom.Width, fom.Height);
            screen.Paint += Draw;
            Player.NewRound();
            screen.Show();
            Frame.Start();
            if (Hoster) {
                BallAdd.Start();
            }
           
            Balls.Clear();

        }
       
        public static void NewRound()
		{
            
            for (int i = 0; i < ToDraw.Count; i++)
            {
                if(ToDraw[i]is Balls ||ToDraw[i] is FakePlayer)
                {
                    ToDraw.RemoveAt(i--);
                }
            }
        Player.NewRound();
            Balls.Clear();
            
            
        }
		
		public static void SetScreen(Form f)
		{
			screen=f;
		}
        private static void Draw(object Sender, PaintEventArgs e)
        {
            var g = e.Graphics;
           



            foreach (var item in ToDraw)
            {
                item.Draw(g);
            }
        }
		private static void FrameTick(object Sender,EventArgs e)
		{
            try
            {
                Controller.SendPlayer(MainPlayer);
                screen.Invalidate();
                if (!Player.Live())
                {
                    NewRound();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
     
            }
		}
		private static void NewBall(object sender,EventArgs e)
		{
			var t= new Balls( new Point(rnd.Next(0,screen.Width-80),rnd.Next(0,screen.Height-80)),Color.FromArgb(rnd.Next(0,255),rnd.Next(0,255),rnd.Next(0,255)));
            ToDraw.Add(t);
          //  SendBall(t);
		}
	   
		
	}
}

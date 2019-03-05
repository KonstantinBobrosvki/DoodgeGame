/*
 * Created by SharpDevelop.
 * User: USER
 * Date: 27.01.2019
 * Time: 20:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
namespace Miss
{
	/// <summary>
	/// Description of Player.
	/// </summary>
	public class Player : IDrawer

	{
        static List<Player> all;
		
        //Area size is 50,50 and it is in New round()
		Rectangle area;
		readonly string name;
	    readonly Pen pen;
	    bool alive;
	    
	    int hightscore;
	    int currentscore;
		int speed=30;
		Key[] controls;
		/* елемент controls 
         * 0вверх
		 * 1 вправо
		 * 2 вниз
		 * 3 влево
		 * 4 буст
		*/
		
			static Font font;
	static	Brush brush;
	
	
	static Player()
	{
		all=new List<Player>(4);
		brush=new SolidBrush(Color.Black);
		    font = new Font("Times New Roman", 12.0f);
		
		 
	}
    public static int CountOfPlayres()
        {
            return all.Count;
        }

	public Player(Key[]control,string name)
	{
		if (control.Length!=5) {
			throw new Exception("Controls must be 5");
		}
		Random rnd=new Random(Guid.NewGuid().ToByteArray().Sum(x=> x));
		
		controls=control;
		pen=new Pen(Color.FromArgb(rnd.Next(0,255),rnd.Next(0,255),rnd.Next(0,255)));
            alive = false;
            this.name=name;

		all.Add(this);
	}
        public static void Clear()
        {
            all.Clear();
        }
	public Player(Color col,string name, Key[] control)
	{
		if (control.Length!=5) {
			throw new Exception("Controls must be 5");
		}
		pen=new Pen(col);
		controls=control;
		this.name=name;
            alive = false;
            all.Add(this);
		
	}


        public Player(Color col, string name)
        {
            
            pen = new Pen(col);
            controls = new Key[] { Key.W, Key.D, Key.S, Key.A, Key.LeftShift };
            this.name = name;
            alive = false;
            all.Add(this);
           

        }


        public Rectangle GetBounds()	{
		return this.area;
	}
	
	
	public static void NewRound()
	{
		foreach (var element in all) {
               
			element.currentscore=0;
			element.alive=true;
            Random rnd = new Random(Guid.NewGuid().ToByteArray().Sum(x => x));
            Point p = new Point(rnd.Next(0, Screen.PrimaryScreen.Bounds.Width - 70), rnd.Next(0, Screen.PrimaryScreen.Bounds.Height - 70));
            element.area = new Rectangle(p, new Size(50, 50));
            }
	}
	
	private void CheckMove()
	{

            if (!alive)
                return;

		if (Keyboard.IsKeyDown(controls[0])) {
					for (int i = 0; i < speed; i++) {
		    		if (area.Y==0) {
		    			break;
		    		}
				area.Y--;
		    		
					}
				}
		if (Keyboard.IsKeyDown(controls[1])) {
					for (int i = 0; i < speed; i++) {
		    		if (area.X==Screen.PrimaryScreen.Bounds.Width-75) {
		    			break;
		    		}
				area.X++;
		    		
					}
				}
				if (Keyboard.IsKeyDown(controls[2])) {
					for (int i = 0; i < speed; i++) {
		    		if (area.Y==Screen.PrimaryScreen.Bounds.Height-75) {
		    			break;
		    		}
				area.Y++;
		    		
			}
				}
				if (Keyboard.IsKeyDown(controls[3])) {
					
			for (int i = 0; i < speed; i++) {
				if (area.X==0) {
		    			break;
		    		}
		    		area.X--;
		    		
				}
				}
				if (Keyboard.IsKeyDown(controls[4])) {
			    speed=50;
		}
				
		if(	Balls.CheckCollusion(this))
		{

                this.alive = false;
			if(all.Count==0)
				Controller.Start();
			
		}
			
		if (speed>20) {
			speed--;
		}
	}
	
	
	public void Draw(Graphics g)
	{

            if (!alive)
                return;
            g.DrawRectangle(pen,this.area);
		g.DrawString(name,font,brush,this.area.X,this.area.Y+16.0f);
            CheckMove();
    }
	public static bool Live()
	{int i=0;
		foreach (var element in all) {
			if(element.alive)
				i++;
		}
		if (i==0) {
			return false;
			
		}
		return true;
	}

        public override string ToString()
        {
            string forret = area.X.ToString() + ',' + area.Y + ',' + name+',' + pen.Color.ToArgb()+';';
            return forret;
        }
        
        public static string[] GetCode()
        {
            string[] forret = new string[all.Count];
            int i = 0;
            foreach (var item in all)
            {
                forret[i++] = item.ToString();
            }
            return forret;
        }
    }
    
}

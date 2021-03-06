﻿/*
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
       
		
        
		Rectangle area;
		readonly string name;
	    readonly Pen pen;
        bool alive;

        //EventArgs are not used
        public event Action<object,EventArgs> Dying;

        int hightscore;
        int currentscore;

        public int CurrentScore
        {
            get
            {
                return currentscore;
            }
            set
            {
                currentscore = value;
                if(HightScore <= value)
                {
                    HightScore = value;
                }


            }
        }
        public int HightScore
        {

            get
            {
                return hightscore;
            }

            private set
            {
                hightscore = value;
            }

        }
        public string Name { get { return name; } }

        int speed;
		Key[] controls;
		/* елемент controls 
         * 0вверх
		 * 1 вправо
		 * 2 вниз
		 * 3 влево
		 * 4 буст
		*/
		
			static Font font;
	        static Brush brush;
            static List<Player> all;

    static Player()
	{
		all=new List<Player>(4);
		brush=new SolidBrush(Color.Black);
		    font = new Font("Times New Roman", 12.0f);
		
		 
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
            speed = 30;
        alive = false;
        all.Add(this);
            Random rnd = new Random(Guid.NewGuid().ToByteArray().Sum(x => x));
            try
            {
                Point p = new Point(rnd.Next(40, Balls.BoundsForReflect().Item1 - 70), rnd.Next(40, Balls.BoundsForReflect().Item2 - 70));
                area = new Rectangle(p, new Size(50, 50));
            }
            catch(ArgumentOutOfRangeException)
            {
                Point p = new Point(rnd.Next(40, Screen.PrimaryScreen.Bounds.Width - 70), rnd.Next(40, Screen.PrimaryScreen.Bounds.Height - 70));
                area = new Rectangle(p, new Size(50, 50));
            }
      
		
	    }

    public Player(Color col, string name)
        {
            
            pen = new Pen(col);
            controls = new Key[] { Key.W, Key.D, Key.S, Key.A, Key.LeftShift };
            this.name = name;
            alive = false;
            speed = 30;
            Random rnd = new Random(Guid.NewGuid().ToByteArray().Sum(x => x));
            Point p;
            try
            {

                p = new Point(rnd.Next(0, Balls.BoundsForReflect().Item1 - 70), rnd.Next(0, Balls.BoundsForReflect().Item2 - 70));
            }
            catch(ArgumentOutOfRangeException)
            {
                p= new Point(rnd.Next(0, Screen.PrimaryScreen.Bounds.Width - 70), rnd.Next(0, Screen.PrimaryScreen.Bounds.Height - 70));
            }
            catch (Exception)
            {
                p = new Point(rnd.Next(0, Screen.PrimaryScreen.Bounds.Width - 70), rnd.Next(0, Screen.PrimaryScreen.Bounds.Height - 70));

            }
            area = new Rectangle(p, new Size(50, 50));
            all.Add(this);
           

        }


    public Rectangle GetBounds(){
		return this.area;
	}
	
	public static void NewRound()
	{
		foreach (var element in all) {
               
			element.currentscore=0;
			element.alive=true;
            Random rnd = new Random(Guid.NewGuid().ToByteArray().Sum(x => x));
            Point p = new Point(rnd.Next(0, Balls.BoundsForReflect().Item1 - 70), rnd.Next(0, Balls.BoundsForReflect().Item2 - 70));
            element.area = new Rectangle(p, new Size(50, 50));
        
        }
            
	}

    public bool Alive
        {
            get
            {
                return alive;
            }
            private set
            {
                if (Dying!=null)
                    if(value==false)
                        Dying(this,new EventArgs());

               


                alive = value;
            }
        }

    private void CheckMove()
	{

           

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

                this.Alive = false;
			
		}
			
		if (speed>20) {
			speed--;
		}
	}
	
	public void Draw(Graphics g)
	{

            if (!Alive)
                return;
            g.DrawRectangle(pen,this.area);
		g.DrawString(name,font,brush,this.area.X,this.area.Y+16.0f);
            
    }

    // Is there any player who wasnt hitted
	public static bool Live()
	{int i=0;
		foreach (var element in all) {
			if(element.Alive)
				i++;
		}
		if (i==0) {
			return false;
			
		}
		return true;
	}

    public override string ToString()
        {
            string forret = area.X.ToString() + ',' + area.Y + ',' + name+',' + pen.Color.ToArgb() + ';';
            return forret;
        }
        
    public static int CountOfPlayres()
        {
            return all.Count;
        }

    public static void MoveChecker()
    {
            
                foreach (var item in all)
                {
                    if (item.Alive)
                    item.CheckMove();
               
                }
           
    }

        public static void NewScore()
        {
            foreach (var item in all)
            {
                if (item.Alive)
                    item.CurrentScore++;
            }
        }

        

    }

}

﻿/*
 * Created by SharpDevelop.
 * User: USER
 * Date: 23.01.2019
 * Time: 18:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
namespace Miss
{
	/// <summary>
	/// Description of Balls.
	/// </summary>
	public class Balls : IDrawer
	{
		static int Bottom;
		static int left;
		static List<Balls> all=new List<Balls>();
			
		Rectangle area;
		SolidBrush color;
		int VectorX;
		int VectorY;
	
		public Balls(Point Location,Color c)
		{
			
			this.area=new Rectangle(Location.X-25,Location.Y-25,25,25);
			color=new SolidBrush(c);
			#region vector
			Random rnd=new Random();
		
			int x=20;
			while(x==0)
			x=rnd.Next(-10,10);
			VectorX=x;
			x=0;
			while(x==0)
			x=rnd.Next(-10,10);
			VectorY=x;
				#endregion
				all.Add(this);
		}
		//Ограничение для отражения от границ
		public static void SetBounds(int x,int y)
		{
			left=x;
			Bottom=y;
		}
		
		
		public void Draw(Graphics g)
		{
			
			g.FillEllipse(color,area);
			this.Move();
		}
		private void Move()
		{
			#region X
			if(VectorX>0){
			for (int i = 0; i < VectorX; i++) {
				area.X++;
				if(area.X>=left+area.Width)
				{
					VectorX=(VectorX*-1)-1;
					
				
					break;
				}
			}
			}
			if(VectorX<0){
			for (int i = 0; i < VectorX*-1; i++) {
				area.X--;
				if(area.X<=0)
				{
					VectorX=(VectorX*-1)+1;
					
					break;
				}
			
			}
			}
			#endregion
			#region Y
			if(VectorY>0){
			for (int i = 0; i < VectorY; i++) {
				area.Y++;
				if(area.Y+area.Height>=Bottom)
				{
					VectorY=(VectorY*-1)-1;
				
					break;
				}
			}
			}
			if(VectorY<0){
			for (int i = 0; i < VectorY*-1; i++) {
				area.Y--;
				if(area.Y<=0)
				{
					VectorY=(VectorY*-1)+1;
					
					break;
				}
			}
			}
			#endregion
			
		}
        public override string ToString()
        {
            string forret = "";
            forret = area.X.ToString() + ',' + area.Y + ',' + VectorX + ',' + VectorY+','+color.Color.ToArgb();
            return forret;
        }
        public static Balls FromCode(string code)
        {
            string[] codes = code.Split(',');
            Point p = new Point(int.Parse(codes[0]), int.Parse(codes[1]));
            var z= new Balls(p, int.Parse(codes[2]), int.Parse(codes[3]), Color.FromArgb(int.Parse(codes[4])));
            all.Add(z);
            return z;
        }
        private Balls(Point p,int vectorX,int vectorY,Color c)
        {
            this.area.Location = p;
            this.area.Size = new Size(25, 25);
            VectorX = vectorX;
            VectorY = vectorY;
            this.color = new SolidBrush(c);
        }


        public static bool CheckCollusion(Player p)
		{
			Rectangle bounds=p.GetBounds();
			foreach (var element in all) {
				if(element.area.IntersectsWith(bounds))
				   {
				   	return true;
				   }
		}
			return false;
		}
		public static void Clear()
		{
			all.Clear();
		}
		
	}
}

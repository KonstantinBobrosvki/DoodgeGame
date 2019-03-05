using System;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Input;
using System.Collections.Generic;


namespace Miss
{
    public class FakePlayer : IDrawer
    {
      
        Pen pen;
        Rectangle area;
        string name;
       static List<FakePlayer> all;
        

        #region Comments

        //This is a class only for drawing player on other computer


        //Player.ToString()
        //public override string ToString()
        //{
        //    string forret = area.X.ToString() + ',' + area.Y + ',' + name + ',' + pen.Color.ToArgb()+';';
        //    return forret;
        //}
        #endregion
        public FakePlayer(string code)
        {//code is Player.ToString()
            var things = code.Split(',');
            area = new Rectangle(new Point(int.Parse(things[0]), int.Parse(things[1])), new Size(50, 50));
            name = things[2];
            pen = new Pen(Color.FromArgb(int.Parse(things[3].TrimEnd(';'))));

        }
        
        static Font font;
        static Brush brush;
        public void SetPosition(Point p)
        {
            area.Location = p;
        }
        public static void Clear()
        {
            all.Clear();
        }
        
        static FakePlayer()
        {
           
            brush = new SolidBrush(Color.Black);
            font = new Font("Times New Roman", 12.0f);
            all = new List<FakePlayer>(2);

        }
       
        //I check is this new or new position
        public static bool NewFake(FakePlayer f)
        {
            foreach (var item in all)
            {
                if (item.name == f.name)
                {
                    item.area = f.area;
                    return false;
                }
            }
            all.Add(f);
            return true;
        }
        
       
        public void Draw(Graphics g)
        {

            if (Controller.Web.AnotherDied)
                return;
            g.DrawRectangle(pen, this.area);
            g.DrawString(name, font, brush, this.area.X, this.area.Y + 16.0f);
        }

    }
}

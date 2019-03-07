using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace Miss
{
    public static partial class Controller
    {
        public static class Local
        {
            //Create players from StartFormLocal.cs information
            public static void ForLocalGame(Dictionary<int, (TextBox, ComboBox)> RegisterInfo)
            {
                int i = 0;
                foreach (var item in RegisterInfo)
                {

                    if (i == 0)
                    {
                        ToDraw.Add(new Player(Color.FromName(item.Value.Item2.SelectedItem.ToString()), item.Value.Item1.Text));
                    }
                    if (i == 1)
                    {

                        ToDraw.Add(new Player(Color.FromName(item.Value.Item2.SelectedItem.ToString()), item.Value.Item1.Text, new Key[] { Key.NumPad8, Key.NumPad6, Key.NumPad5, Key.NumPad4, Key.Add }));
                       
                    }
                    if (i == 2)
                    {
                        ToDraw.Add(new Player(Color.FromName(item.Value.Item2.SelectedItem.ToString()), item.Value.Item1.Text, new Key[] { Key.Up, Key.Right, Key.Down, Key.Left, Key.Enter }));

                    }
                    if (i == 3)
                    {
                        ToDraw.Add(new Player(Color.FromName(item.Value.Item2.SelectedItem.ToString()), item.Value.Item1.Text, new Key[] { Key.Y, Key.J, Key.H, Key.G, Key.Space }));

                    }


                    i++;
                }
            }

            static Local()
            {
                BallAdd.Tick += NewBall;
            }

            
            public async static void Start()
            {
                SetScreen(new MainForm());
                screen.Show();
                Frame.Start();
                BallAdd.Start();
                Frame.Tick += LocalTick;
                NewRound();

              
            }

            //This is FrameTick and we check is there any alive player
            private static void LocalTick(object sender, EventArgs e)
            {
                Player.MoveChecker();
                FrameTick();
               
                if (!Player.Live())
                { 
                    NewRound();
                }
            }

            public static void NewRound()
            {
                Player.NewRound();
                Balls.Clear();
                for (int i = 0; i < ToDraw.Count; i++)
                {
                    if(ToDraw[i]is Balls)
                    {
                        ToDraw.RemoveAt(i--);
                    }
                }
            }

            private static void NewBall(object sender, EventArgs e)
            {
               ToDraw.Add(new Balls(screen));
            }

        }

    }
    

}

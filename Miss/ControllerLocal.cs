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
                        var sd = new Player(Color.FromName(item.Value.Item2.SelectedItem.ToString()), item.Value.Item1.Text);
                        ToDraw.Add(sd);
                        sd.Dying += PlayerDying;
                    }
                    if (i == 1)
                    {
                        var sd = new Player(Color.FromName(item.Value.Item2.SelectedItem.ToString()), item.Value.Item1.Text, new Key[] { Key.NumPad8, Key.NumPad6, Key.NumPad5, Key.NumPad4, Key.Add });
                        ToDraw.Add(sd);
                        sd.Dying += PlayerDying;
                    }
                    if (i == 2)
                    {
                        var sd = new Player(Color.FromName(item.Value.Item2.SelectedItem.ToString()), item.Value.Item1.Text, new Key[] { Key.Up, Key.Right, Key.Down, Key.Left, Key.Enter });
                        ToDraw.Add(sd);
                        sd.Dying += PlayerDying;
                    }
                    if (i == 3)
                    {
                        var sd = new Player(Color.FromName(item.Value.Item2.SelectedItem.ToString()), item.Value.Item1.Text, new Key[] { Key.Y, Key.J, Key.H, Key.G, Key.Space });
                        ToDraw.Add(sd);
                        sd.Dying += PlayerDying;
                    }


                    i++;
                }
            }

            static Local()
            {
                BallAdd.Tick += NewBall;
                logs = new Dictionary<string, (int, int)>();
            }

            static Dictionary<String, (int, int)> logs;
            private static void PlayerDying(object sender,EventArgs e)
            {
                int i = 0;
                Player p = sender as Player;
                try
                {
                    logs.Add(p.Name, (p.HightScore, p.CurrentScore));
                }
                catch(ArgumentException)
                {
                    try
                    {
                        logs.Add(p.Name + ++i, (p.HightScore, p.CurrentScore));
                    }
                    catch(ArgumentException)
                    {
                        try
                        {
                            logs.Add(p.Name + ++i, (p.HightScore, p.CurrentScore));
                        }
                        catch (ArgumentException)
                        {
                            try
                            {
                                logs.Add(p.Name + ++i, (p.HightScore, p.CurrentScore));
                            }
                            catch (ArgumentException)
                            {
                                try
                                {
                                    logs.Add(p.Name + ++i, (p.HightScore, p.CurrentScore));
                                }
                                catch(ArgumentException)
                                {
                                    
                                        logs.Add(p.Name + ++i, (p.HightScore, p.CurrentScore));
                                   
                                }


                        }
                            }

                    }
                }
                p.CurrentScore = 0;
            }

            
            public  static void Start()
            {
                SetScreen(new MainForm());
                screen.Show();
                Frame.Start();
                BallAdd.Start();
                Frame.Tick += LocalTick;
                NewRound();

                Timer forscore = new Timer();
                forscore.Tick += (sender, e) => { Player.NewScore(); };
                forscore.Interval = 1000;
                forscore.Start();
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
                var formess = new List<string>(5);
                foreach (var item in logs)
                {
                    formess.Add(item.Key + " Най-добър резултат " + item.Value.Item1 + ". Текущ резултат " + item.Value.Item2);
                }
                logs.Clear();
                BallAdd.Stop();
                foreach (var item in formess)
                {
                    MessageBox.Show(item);


                }
                
                Player.NewRound();
                Balls.Clear();
                for (int i = 0; i < ToDraw.Count; i++)
                {
                    if(ToDraw[i]is Balls)
                    {
                        ToDraw.RemoveAt(i--);
                    }
                }
               
                Timer shit = new Timer();
                shit.Interval = 10000;

                shit.Tick += (sender, e) => {
                BallAdd.Start();
                    shit.Dispose();
                };
                shit.Start();   
                    


            }

            private static void NewBall(object sender, EventArgs e)
            {
               ToDraw.Add(new Balls(screen));
            }

        }

    }
    

}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Runtime.Serialization.Formatters.Binary;

namespace Miss
{
    public static partial class Controller
    {

        public static class Web
        {
            static Player p;
            
            public static bool Hoster;

            static bool anotherdied; //Is dead another player

            private static bool gameisstartedonboth;

            public static bool GameIsStartedonBoth { get => gameisstartedonboth; set => gameisstartedonboth = value; }

            static System.Windows.Forms.Label score;

            static System.Windows.Forms.Timer forscore;

            //Initialization of some things
            static Web()
            {
                GameIsStartedonBoth = false;

                score = new System.Windows.Forms.Label();
                score.Bounds = new System.Drawing.Rectangle(0, 0, 100, 50);
                score.BackColor = System.Drawing.Color.White;

                forscore = new System.Windows.Forms.Timer();
                forscore.Tick += (sender, even) => Player.NewScore();
                forscore.Interval = 1000;
            }

            //Initialization of all things 
            public async static void Start()
            {
                //Opening new form for playground
                MainForm form = new MainForm();
                SetScreen(form);
                screen.Controls.Add(score);
                //Showing playground
                screen.Show();

               
               
                forscore.Start();

                Frame.Start();
                Frame.Tick += ScreenUpdate;

                if (Hoster)
                {
                    BallAdd.Start();
                    BallAdd.Tick += NewBall;
                }
                else
                {
                    SendToStart();
                    Controller.Web.NewRound();
                    GameIsStartedonBoth = true;
                    AnotherDied = false;
                   
                }

                //Start listening for signals
                await Task.Run(() => Reciever());

               
                //For closing of program
                //I dont known what shit is going on
                screen.FormClosed += Exit;
                Program.z.FormClosed += Exit;
                screen.FormClosing += Exit;
                Program.z.FormClosing += Exit;

            }//Like main

            public static bool AnotherDied
            {

                get
                {
                    return anotherdied;

                }
                private set
                {
                    anotherdied = value;
                }
            }//anotherdied public version

            //Recieve info from another computer
            private static void Reciever()
            {

                //hoster port is 8006 client 8005

                int port = 8005;
                if (Hoster)
                    port = 8006;

                string address = "127.0.0.1";

                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            
                
                // создаем сокет
                Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {

                    // связываем сокет с локальной точкой, по которой будем принимать данные
                    listenSocket.Bind(ipPoint);

                    // начинаем прослушивание
                    listenSocket.Listen(10);

                    while (true)
                    {
                        Socket handler = listenSocket.Accept();
                        // получаем сообщение
                        StringBuilder builder = new StringBuilder();
                        int bytes = 0; // количество полученных байтов
                        byte[] data = new byte[256]; // буфер для получаемых данных

                        do
                        {
                            bytes = handler.Receive(data);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (handler.Available > 0);

                        if (builder.ToString().Contains(";"))
                        {
                            var z = new FakePlayer(builder.ToString());

                            if (FakePlayer.NewFake(z))
                            {
                                ToDraw.Add(z);
                            }

                        }
                        else if (builder.ToString() == "Start")
                        {
                            if (Hoster)
                            { 
                                GameIsStartedonBoth = true;
                                BallAdd.Start();
                                NewRound();
                            }

                        }
                        else if (builder.ToString().Contains(","))
                        {
                            ToDraw.Add(Balls.FromCode(builder.ToString()));

                        }
                        else if (builder.ToString().Contains("MeDied"))
                        {

                            AnotherDied = true;
                        }
                        else if (builder.ToString()== "Exit")
                        {
                            GameIsStartedonBoth = false;
                            System.Windows.Forms.MessageBox.Show("Другия играч излезе");
                            NewRound();
                            
                        }
                        else  if(builder.ToString().Contains("Exit") || builder.ToString().Contains("Exit".ToLower()))
                        {
                            System.Windows.Forms.MessageBox.Show("Другия играч излезе");

                            GameIsStartedonBoth = false;
                            NewRound();

                        }





                        // закрываем сокет
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    System.Windows.Forms.MessageBox.Show("Reciever Error");


                }

            }

            #region Sender methods for another computer

            //Sending signal to start
            private static void SendToStart()
            {
                int port = 8006;
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    sender.Connect(ipPoint);

                    var forsend = Encoding.Unicode.GetBytes("Start");
                    sender.Send(forsend);
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Грешка при свързване");
                }
                GameIsStartedonBoth = true;
            }

            //Sending ball
            private static void SendBall(Balls b)
            {
                if (!GameIsStartedonBoth)
                    return;
                
                int port = 8005;
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    sender.Connect(ipPoint);

                    var forsend = Encoding.Unicode.GetBytes(b.ToString());
                    sender.Send(forsend);
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (Exception e)
                {
                    return;
                }
            }

            //Sending FakePlayer to another computer
            private static void SendPlayer(Player p)
            {
                if (!GameIsStartedonBoth)
                    return;
                if (!p.Alive)
                    return;
                int port = 8006;
                if (Hoster)
                    port = 8005;

                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    sender.Connect(ipPoint);

                    var forsend = Encoding.Unicode.GetBytes(p.ToString());
                    sender.Send(forsend);
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (Exception e)
                {

                    return;
                }
            }

            //Sending signal of dying of MainPlayer
            private static void MeDied()
            {

                if (!GameIsStartedonBoth)
                    return;


                int port = 8006;
                if (Hoster)
                    port = 8005;
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    sender.Connect(ipPoint);

                    var forsend = Encoding.Unicode.GetBytes("MeDied");
                    sender.Send(forsend);
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Грешка при пращане");

                    return;
                }
            }

            //Send signal that i left
             static void Exit(object caller,System.Windows.Forms.FormClosedEventArgs e)
             {
                Exit();
             }
            //Send signal that i left
            static void Exit(object caller,System.Windows.Forms.FormClosingEventArgs e)
            {
                
                Exit(); 
            }
            //Signal exit Method
            public static void Exit()
            {
                int port = 8006;
                if (Hoster)
                    port = 8005;
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    sender.Connect(ipPoint);
                    

                    var forsend = Encoding.Unicode.GetBytes("Exit");
                    sender.Send(forsend);
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (Exception ex)
                {

                    System.Windows.Forms.MessageBox.Show("Грешка при излизане");
                }
            }
            #endregion

            //Player of this computer
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
                    p.Dying += MainDying;

                }
            }

          

            //Event of MainPlayer Dying
            private static void MainDying(object sender,EventArgs e)
            {
                forscore.Stop();
                //Send to other comp that i died

                if (GameIsStartedonBoth)
                MeDied();

                

                
                score.Text = "Твоят най-добър резултат " + MainPlayer.HightScore + " Текущ резултат" + MainPlayer.CurrentScore;
                MainPlayer.CurrentScore = 0;
                    

            }

            //Sending information about your MainPlayer to other computer
            static void ScreenUpdate(object sender,EventArgs e)
            {
                //Drawing elemets
                FrameTick();

                Player.MoveChecker();

                if (!GameIsStartedonBoth)
                    return;
               
                Controller.Web.SendPlayer(MainPlayer);
                if (!Player.Live() && AnotherDied)
                {
                    Web.NewRound();
                }
            }

            //Starting new round
            static void NewRound()
            {
                forscore.Start();
                Player.NewRound();
                AnotherDied = false;
                Balls.Clear();
                for (int i = 0; i < ToDraw.Count; i++)
                {
                    if(ToDraw[i] is Balls)
                    ToDraw.RemoveAt(i--);
                }
            }

            //Create new ball and send it
            static void NewBall(object sender,EventArgs e)
            {

                if (!GameIsStartedonBoth)
                    return;
                    
                    var t = new Balls(screen);
                    ToDraw.Add(t);
                    Web.SendBall(t);

                
               

            }

        }
      
    }

}

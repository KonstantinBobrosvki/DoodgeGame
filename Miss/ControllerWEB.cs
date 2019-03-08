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
            //8006 is hoster recivier 8005 is for client

            private static bool GameIsStartedonBoth;

            static Player p;
            
            private static bool hoster;

            static bool anotherdied; //Is dead another player

            //My port reciever
            static int MyPort;

            //Enemy reciever port (I send thing there)
            static int EnemyPort;

            //private of EnemyIp
            static string enemyip;

            //Initialization of some things
            static Web()
            {
                GameIsStartedonBoth = false;
            }

            public static bool Hoster
            {
                get
                {
                    return hoster;
                }
                set
                {
                    hoster = value;
                    if (value)
                    {
                        MyPort = 8006;
                        EnemyPort = 8005;
                    }
                    else
                    {
                        MyPort = 8005;
                        EnemyPort = 8006;
                    }

                }
            }

            public static string MyIPAdress
            {
                get
                {
                    //Download info from this site
                    //My public ip

                    var forret = new WebClient().DownloadString("http://icanhazip.com");
                    return forret.Remove(forret.Length-1,1);
                }
            }

            public static string EnemyIp
            {
                get
                {
                    return enemyip;
                }
                set
                {
                    try
                    {
                        IPAddress.Parse(value);
                    }
                    catch (FormatException ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Въввединия IP не е верен.");
                        return;
                    }
                    catch(ArgumentNullException ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Въвведeтe IP ");
                        return;
                    }
                    enemyip = value;
                }
            }


            //Initialization of all things 
            public async static void Start()
            {
                //Opening new form for playground
                MainForm form = new MainForm();
                SetScreen(form);

                //Showing playground
                screen.Show();

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
                  
                   
                }

                //Start listening for signals
                await Task.Run(() => Reciever());

                //For closing of program
                screen.FormClosed += Exit;


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
                TcpListener tcpListener = new TcpListener(IPAddress.Any,MyPort);

               //TODO: Improve External IP

                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(MyIPAdress), MyPort);


                // создаем сокет
                Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {

                    // связываем сокет с локальной точкой, по которой будем принимать данные
                    listenSocket.Bind(ipPoint);
                }
                catch(SocketException)
                {
                    System.Windows.Forms.MessageBox.Show("Грешка при свързване за слушане");
                }
                try
                {
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
                                BallAdd.Start();
                                NewRound();
                                SendToStart();
                            }
                            else
                            {
                                Controller.Web.NewRound();
                                AnotherDied = false;
                            }
                            GameIsStartedonBoth = true;

                        }
                        else if (builder.ToString().Contains(","))
                        {
                            ToDraw.Add(Balls.FromCode(builder.ToString()));

                        }
                        else if (builder.ToString().Contains("MeDied"))
                        {

                            AnotherDied = true;
                        }
                        else if (builder.ToString().Equals("STOP"))
                        {
                            GameIsStartedonBoth = false;
                        }





                        // закрываем сокет
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                
                }
               
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show("Грешка при приемане");
                   // System.Windows.Forms.MessageBox.Show(ex.);
                    return;
                }

            }

            #region Sender methods for another computer

            //Sending signal to start
            private static void SendToStart()
            {
                
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(EnemyIp), EnemyPort);
                
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
                    System.Windows.Forms.MessageBox.Show("Грешка при старт");
                }
                GameIsStartedonBoth = true;
            }

            //Sending ball
            private static void SendBall(Balls b)
            {
                if (!GameIsStartedonBoth)
                    return;
                
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(EnemyIp), EnemyPort);
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
                    System.Windows.Forms.MessageBox.Show("Грешка при пращане на топката");

                }
            }

            //Sending FakePlayer to another computer
            private static void SendPlayer(Player p)
            {
                if (!GameIsStartedonBoth)
                    return;
                if (!p.Alive)
                    return;


                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(EnemyIp), EnemyPort);
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

                    System.Windows.Forms.MessageBox.Show("Грешка при пращане на играч");

                }
            }

            //Sending signal of dying of MainPlayer
            private static void MeDied()
            {
                if (!GameIsStartedonBoth)
                    return;
               
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(EnemyIp), EnemyPort);
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

                    System.Windows.Forms.MessageBox.Show("Грешка при смърт");

                }
            }

            //Send signal that i left
            static void Exit(object caller,System.Windows.Forms.FormClosedEventArgs e)
            {
                if (!GameIsStartedonBoth)
                    return;

                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(EnemyIp), EnemyPort);
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    sender.Connect(ipPoint);

                    var forsend = Encoding.Unicode.GetBytes("STOP");
                    sender.Send(forsend);
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (Exception ex)
                {

                    System.Windows.Forms.MessageBox.Show("Грешка при затваряне");

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
            private static void MainDying()
            {
                //Send to other comp that i died
                if(GameIsStartedonBoth)
                MeDied();
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

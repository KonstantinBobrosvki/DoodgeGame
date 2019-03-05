﻿using System;
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
            
            public static bool Hoster;

            static bool anotherdied; //Is dead another player




            //Initialization of some things
            static Web()
            {
                GameIsStartedonBoth = false;
            }

            //Initialization of all things 
            public async static void Start()
            {
                //Opening new form for playground
                SetScreen(new MainForm());

                //Showing playground
                screen.Show();

               

               

                System.Windows.Forms.MessageBox.Show("sd");
                
                Frame.Start();
                Frame.Tick += ScreenUpdate;

                if (Hoster)
                {
                    BallAdd.Start();
                }
                else
                {
                    SendToStart();
                    Controller.Web.NewRound();
                    GameIsStartedonBoth = true;
                }

                //Start listening for signals
                await Task.Run(() => Reciever());
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
                    System.Windows.Forms.MessageBox.Show(e.StackTrace);
                }
                GameIsStartedonBoth = true;
            }

            //Sending ball
            private static void SendBall(Balls b)
            {
                if (!GameIsStartedonBoth)
                {
                    return;
                }
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
                    System.Windows.Forms.MessageBox.Show(e.StackTrace);
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

                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
            }

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

            //Sending signal of dying of MainPlayer
            private static void MeDied()
            {
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

                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
            }

            //Sending information about your MainPlayer to other computer
            static void ScreenUpdate(object sender,EventArgs e)
            {

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
                    ToDraw.RemoveAt(i--);
                }
            }

            //Create new ball and send it
            static void NewBall(object sender,EventArgs e)
            {
                if(!GameIsStartedonBoth)
                {
                    return;
                }

                var t = new Balls(screen);
                ToDraw.Add(t);
                Web.SendBall(t);

            }

        }
      
    }

}
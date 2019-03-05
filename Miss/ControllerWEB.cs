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
      static List<FakePlayer> fakes = new List<FakePlayer>();
        public static async void InternetStart()
        {
          
           
            await Task.Run(() => Reciever());
        }
        private static void Reciever()
        {
            // порт сервера

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

                   if(builder.ToString().Contains(";"))
                    {
                       
                       var z=new FakePlayer(builder.ToString());
                        if(FakePlayer.NewFake(z))
                        {
                            ToDraw.Add(z);
                        }
                    }
                   else
                    {
                        if(!Hoster)
                    ToDraw.Add(Balls.FromCode(builder.ToString()));

                    }

                    // отправляем ответ
                   
               
                   
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                System.Windows.Forms.MessageBox.Show("Reciever Error");
                return;

            }

        }
        
        private static void SendBall(Balls b)
        {
           
            int port = 8005;
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Connect(ipPoint);
              
                var forsend = Encoding.Unicode.GetBytes(b.ToString());
                listenSocket.Send(forsend);
               listenSocket.Shutdown(SocketShutdown.Both);
                listenSocket.Close();
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.StackTrace);
            }
        }
        private static void SendPlayer(Player p)
        {
            Thread.Sleep(10000);
            int port = 8006;
            if (Hoster)
                port = 8005;
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Connect(ipPoint);

                var forsend = Encoding.Unicode.GetBytes(p.ToString());
                listenSocket.Send(forsend);
                listenSocket.Shutdown(SocketShutdown.Both);
                listenSocket.Close();
            }
            catch (Exception e)
            {

                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

    }

}

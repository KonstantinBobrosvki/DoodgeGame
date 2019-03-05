using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Input;

namespace Miss
{
    public partial class StartForm : Form
    {
      //For playrs construction
        private Dictionary<int, (TextBox, ComboBox)> RegisterInfo;
        void LocalClick()
        {
            
            Controls.Clear();
            {
                NumericUpDown CountOfPlayers = new NumericUpDown();
                CountOfPlayers.SetBounds(30, 30, 150, 200);
                CountOfPlayers.Font = new Font("Times New Roman", 24.0f);
                CountOfPlayers.BorderStyle = BorderStyle.FixedSingle;
                CountOfPlayers.ValueChanged += PlayerCountChanged;
                CountOfPlayers.Maximum = 4;
                CountOfPlayers.Minimum = 1;
                Controls.Add(CountOfPlayers);

            }

        }
        void PlayerCountChanged(object sender, EventArgs e)
        {
            DrawPlayersAccaunts(int.Parse((sender as NumericUpDown).Value.ToString()));
        }
        void DrawPlayersAccaunts(int HowMuch)
        {

            //Clear from past chanching
            RegisterInfo = new Dictionary<int, (TextBox, ComboBox)>();
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] is TextBox || Controls[i] is ComboBox || Controls[i] is Button)
                    Controls.RemoveAt(i--);
            }
            //Draw NewFields
            for (int i = 0; i < HowMuch; i++)
            {

                TextBox namefield = new TextBox();
                namefield.Bounds = new Rectangle(new Point(i * 210, Screen.PrimaryScreen.WorkingArea.Height / 2 - 60), new Size(200, 100));
                namefield.Font = new Font("Times New Roman", 24.0f);

                namefield.Text = "Your name";
                Controls.Add(namefield);
                #region Color chooser
                ComboBox color = new ComboBox();
                color.Text = "Your color";
                //Fill Chosse color for player
                foreach (System.Reflection.PropertyInfo prop in typeof(Color).GetProperties())
                {
                    if (prop.PropertyType.FullName == "System.Drawing.Color")
                        color.Items.Add(prop.Name);
                }
                color.Bounds = new Rectangle(namefield.Location.X, namefield.Location.Y + 130, namefield.Width, namefield.Height);
                Controls.Add(color);
                #endregion
                RegisterInfo.Add(i, (namefield, color));
                
            }
            Button StartGame = new Button();
            StartGame.Click += StartLocalGame;
            StartGame.Bounds = new Rectangle(new Point(Screen.PrimaryScreen.Bounds.Width / 2 - 150, Screen.PrimaryScreen.Bounds.Height - 200), new Size(200, 150));
            StartGame.Text = "Start";
            Controls.Add(StartGame);

        }
        void StartLocalGame(object sender, EventArgs e)
        {
            int i = 0;
            Controller.Hoster = true;
            
            foreach (var item in RegisterInfo)
            {
                try
                {
                   if(i==0)
                    {
                        new Player(Color.FromName(item.Value.Item2.SelectedItem.ToString()), item.Value.Item1.Text);
                        

                    }
                    if (i == 1)
                    {

                        new Player(Color.FromName(item.Value.Item2.SelectedItem.ToString()), item.Value.Item1.Text, new Key[] { Key.NumPad8, Key.NumPad6, Key.NumPad5, Key.NumPad4, Key.Add });

                    }
                    if (i == 2)
                    {
                        new Player(Color.FromName(item.Value.Item2.SelectedItem.ToString()), item.Value.Item1.Text, new Key[] { Key.Up, Key.Right, Key.Down, Key.Left, Key.Enter });
                        
                    }
                    if (i == 3)
                    {
                        new Player(Color.FromName(item.Value.Item2.SelectedItem.ToString()), item.Value.Item1.Text, new Key[] { Key.Y, Key.J, Key.H, Key.G, Key.Space });

                    }

                }
                catch(NullReferenceException)
                {
                    MessageBox.Show("Everyone must choose his color");
                    Player.Clear();
                    return;
                }
                ++i;
            }
            Controller.Start();
            this.Hide();
        }



    }
}

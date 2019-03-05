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

        //Main of this Form
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

        //When we change players count we redraw fields
        void PlayerCountChanged(object sender, EventArgs e)
        {
            DrawPlayersAccaunts(int.Parse((sender as NumericUpDown).Value.ToString()));
        }

        //Create fields for players constructor
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

        //Start game
        void StartLocalGame(object sender, EventArgs e)
        {
            
            
            try
            {
                Controller.Local.Start();
                Controller.Local.ForLocalGame(RegisterInfo);


            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Everyone must choose his color");
                Player.Clear();
                return;
            }
           
           
            this.Hide();
        }



    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Input;

namespace Miss
{
    public partial class StartFormLocal : Form
    {
        private Button ExitLocal;
        private NumericUpDown numericUpDown1;

        //For playrs construction
        private Dictionary<int, (TextBox, ComboBox)> RegisterInfo;

        //Main of this Form
       
            
            
           

       

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
                if (Controls[i] is TextBox || Controls[i] is ComboBox )
                    Controls.RemoveAt(i--);
            }
            //Draw NewFields
            for (int i = 0; i < HowMuch; i++)
            {

                TextBox namefield = new TextBox();
                namefield.Bounds = new Rectangle(new Point(i * 210, Screen.PrimaryScreen.WorkingArea.Height / 2 - 60), new Size(200, 100));
                namefield.Font = new Font("Times New Roman", 24.0f);

                namefield.Text = "Име на играч";
                Controls.Add(namefield);
                #region Color chooser
                ComboBox color = new ComboBox();
                color.Text = "Цветът на играч";
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
            StartGame.Text = "Започни";
            Controls.Add(StartGame);

        }

        //Start game
        void StartLocalGame(object sender, EventArgs e)
        {


            try
            {
                Controller.Local.ForLocalGame(RegisterInfo);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Всеки трябва да си избере цвят и име");
                Player.Clear();
                Controller.Clear();
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Всеки трябва да си избере цвят и име");
                Player.Clear();
                Controller.Clear();
                return;
            }
            this.Hide();

            Controller.Local.Start();
            
            
           
           
        }

        private void InitializeComponent()
        {
            this.ExitLocal = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // ExitLocal
            // 
            this.ExitLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitLocal.Location = new System.Drawing.Point(1336, -1);
            this.ExitLocal.Name = "ExitLocal";
            this.ExitLocal.Size = new System.Drawing.Size(68, 61);
            this.ExitLocal.TabIndex = 5;
            this.ExitLocal.Text = "Изход";
            this.ExitLocal.UseVisualStyleBackColor = true;
            this.ExitLocal.Click += new System.EventHandler(this.ExitLocal_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(13, 24);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(112, 20);
            this.numericUpDown1.TabIndex = 6;
            // 
            // StartFormLocal
            // 
            this.ClientSize = new System.Drawing.Size(1405, 640);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.ExitLocal);
            this.Name = "StartFormLocal";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
          
            this.ResumeLayout(false);

        }

        public StartFormLocal()
        {
            InitializeComponent();
            this.Icon = new Icon(System.AppDomain.CurrentDomain.BaseDirectory + "favicon.ico");
            this.FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;


            for (int i = 0;true; i++)
            {
                if(Controls[i] is NumericUpDown)
                {
                    Controls.RemoveAt(i);
                    break;
                }
            }

            numericUpDown1 = new NumericUpDown();
            numericUpDown1.SetBounds(30, 30, 150, 200);
            numericUpDown1.Font = new Font("Times New Roman", 24.0f);

            numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            numericUpDown1.ValueChanged += PlayerCountChanged;
            numericUpDown1.Maximum = 4;
            numericUpDown1.Minimum = 1;

            Controls.Add(numericUpDown1);
        }


        private bool IsOpenedDialogForExit = true;

        private void ExitLocal_Click(object sender, EventArgs e)
        {

        
            if (IsOpenedDialogForExit)
            {
                IsOpenedDialogForExit = false;
                DialogResult d = MessageBox.Show("Искате ли да излезете?", "Сигурни ли сте, че искате да излезете?", MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                    Program.z.Close();
                    IsOpenedDialogForExit = true;
                }
                else
                {
                    IsOpenedDialogForExit = true;
                }
            }
        
        }
    }
}

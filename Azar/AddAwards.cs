using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Azar
{
    public partial class AddAwards : Form
    {
        #region Zmienne
        private int togMove;
        private int mValX;
        private int mValY;
        MainWindow mainWindow;
        #endregion

        public AddAwards(int lastAwardValue, MainWindow mainWindowParam = null)
        {
            InitializeComponent();
            mainWindow = mainWindowParam;
            numericUpDown_AwardValue.Value = lastAwardValue;
        }

        protected override bool ProcessDialogKey(Keys keyData) // Zatwierdzenie danych klawiszem enter, lub zamknięcie okna klawiszem escape
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Dispose();
            }
            else if (Form.ModifierKeys == Keys.None && keyData == Keys.Enter)
            {
                AddAward(); // Dodaj nagrodę
            }
            return base.ProcessDialogKey(keyData);
        }

        // PRZESUWANIE OKNA >>>>>
        private void topBorder_MouseDown(object sender, MouseEventArgs e)
        {
            togMove = 1;
            mValX = e.X;
            mValY = e.Y;
        }

        private void topBorder_MouseUp(object sender, MouseEventArgs e)
        {
            togMove = 0;
        }

        private void topBorder_MouseMove(object sender, MouseEventArgs e)
        {
            if (togMove == 1)
            {
                this.SetDesktopLocation(MousePosition.X - mValX, MousePosition.Y - mValY);
            }
        }
        // <<<<< PRZESUWANIE OKNA

        // ZARZĄDZANIE OKNEM >>>>>
        private void button_Mini_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_Mini.Image = global::Azar.Properties.Resources.mini2;
        }
        //
        private void button_Mini_MouseUp(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            this.button_Mini.Image = global::Azar.Properties.Resources.mini1;
        }
        //
        private void button_Exit_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_Exit.Image = global::Azar.Properties.Resources.exit2;
        }
        //
        private void button_Exit_MouseUp(object sender, MouseEventArgs e)
        {
            this.Dispose();
        }
        // <<<<< ZARZĄDZANIE OKNEM

        private void button_AddAward_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_AddAward.Image = global::Azar.Properties.Resources.addAward2;
        }

        private void button_AddAward_MouseUp(object sender, MouseEventArgs e)
        {
            this.button_AddAward.Image = global::Azar.Properties.Resources.addAward;
            AddAward(); // Dodaj nagrodę
        }

        // ########################## DODAWANIE NAGRODY ##########################
        private void AddAward()
        {
            mainWindow.lastAwardWeight = (int)numericUpDown_AwardValue.Value;
            try
            {
                if (textBox_AwardName.Text != "")
                {
                    mainWindow.listBox_Awards.DataSource = null;
                    mainWindow.listBox_Awards.Items.Clear();

                    mainWindow.awards.Add(new Award(textBox_AwardName.Text, (int)numericUpDown_AwardAmount.Value, (int)numericUpDown_AwardValue.Value));

                    mainWindow.listBox_Awards.DataSource = mainWindow.awards;
                    mainWindow.listBox_Awards.DisplayMember = "displayAll";
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

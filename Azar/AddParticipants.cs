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
    public partial class AddParticipants : Form
    {
        #region Zmienne i obiekty
        private int togMove;
        private int mValX;
        private int mValY;

        MainWindow mainWindow;
        #endregion

        public AddParticipants(MainWindow mainWindowParam = null)
        {
            InitializeComponent();
            mainWindow = mainWindowParam;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Dispose();
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

        private void button_AddParticipants_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_AddParticipants.Image = global::Azar.Properties.Resources.buttonAddParticipants2;
        }

        // ########################## DODAWANIE UCZESTNIKÓW ##########################
        private void button_AddParticipants_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (richTextBox_participants.Text != "")
                {
                    mainWindow.listBox_Participants.DataSource = null;
                    mainWindow.listBox_Participants.Items.Clear();

                    string[] splittedParticipants = richTextBox_participants.Text.Split('\n');

                    foreach (var eachParticipant in splittedParticipants)
                    {
                        if (!string.IsNullOrEmpty(eachParticipant))
                        {
                            int index = mainWindow.participants.FindIndex(item => item.name == eachParticipant);

                            if (index < 0)
                            {
                                mainWindow.participants.Add(new Participant(eachParticipant));
                            }
                            else
                            {
                                DialogResult dialogResult = MessageBox.Show(this, "Na liście uczestników znajduje się już uczestnik o nazwie \"" + eachParticipant + "\".\nCzy mimo to chcesz dodać tego uczestnika?", "Dodawanie uczestnika - Azar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    for (int i = 1; i <= 100; i++)
                                    {
                                        index = mainWindow.participants.FindIndex(item => item.name == eachParticipant + "_" + i.ToString());

                                        if (index < 0)
                                        {
                                            mainWindow.participants.Add(new Participant(eachParticipant + "_" + i.ToString()));
                                            break;
                                        }
                                            
                                    }
                                }
                            }
                        }
                    }

                    mainWindow.listBox_Participants.DataSource = mainWindow.participants;
                    mainWindow.listBox_Participants.DisplayMember = "name";
                    mainWindow.numericUpDown_NumberOfWinners_Click(null, null);
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.button_AddParticipants.Image = global::Azar.Properties.Resources.buttonAddParticipants;
        }
    }
}

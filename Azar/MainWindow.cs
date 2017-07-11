using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;

namespace Azar
{
    public partial class MainWindow : Form
    {
        #region Zmienne i obiekty
        private int togMove; // Poruszanie oknem
        private int mValX;
        private int mValY;

        private RandMethod randMetods; // Metody odpowiadające za losowanie
        private ListManager listManager; // Metody do zarządzania listami
        private MenuMethod menuMethods; // Metody odpowiadające za wczytywanie i zapisywanie stanu progrmau
        private Thread randProcess_Thread; // Losowanie odbywa się w osobnym wątku
        private Stopwatch sw; // Odmierza czas między naciśnięciem, a puszczeniem przycisku losowania, wartość odmierzonego czasu używana przy inicjalizacji obiektu Random.

        public List<Participant> participants = new List<Participant> { };
        public List<Award> awards = new List<Award> { };
        public List<Winner> winners = new List<Winner> { };

        public bool radioChangeWasCanceled = false, skipWinnersAutoChange = false; // Żeby standardowe komunikaty nie były wyświetlane podczas wczytywania stanu programu
        public int lastAwardWeight = 1; // Przy dodawaniu nagrody w polu waga znajduje się ostatnio używana wartość

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            // Przekazanie obecnej instancji do obiektów:
            randMetods = new RandMethod(this);
            listManager = new ListManager(this);
            menuMethods = new MenuMethod(this);
        }

        private void Azar_Load(object sender, EventArgs e)
        {

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
        private void button_mini_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_Mini.Image = global::Azar.Properties.Resources.mini2;
        }
        //
        private void button_mini_MouseUp(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            this.button_Mini.Image = global::Azar.Properties.Resources.mini1;
        }
        //
        private void button_exit_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_Exit.Image = global::Azar.Properties.Resources.exit2;
        }
        //
        private void button_exit_MouseUp(object sender, MouseEventArgs e)
        {
            this.button_Exit.Image = global::Azar.Properties.Resources.exit1;
            DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz zamknąć aplikację?", "Zamkaniej aplikacji - Azar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                System.Environment.Exit(0);
            }
        }
        // <<<<< ZARZĄDZANIE OKNEM

        // GRUPA UCZESTNICY >>>>>
        private void button_addParticipants_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_AddParticipants.Image = global::Azar.Properties.Resources.buttonAdd2;
        }

        private void button_addParticipants_MouseUp(object sender, MouseEventArgs e)
        {
            this.button_AddParticipants.Image = global::Azar.Properties.Resources.buttonAdd;
            if (!Application.OpenForms.OfType<AddParticipants>().Any())
            {
                new AddParticipants(this).Show();
            }
        }

        private void button_removeParticipants_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_RemoveParticipants.Image = global::Azar.Properties.Resources.buttonRemove2;
        }

        private void button_removeParticipants_MouseUp(object sender, MouseEventArgs e)
        {
            this.button_RemoveParticipants.Image = global::Azar.Properties.Resources.buttonRemove;
            listManager.DeleteParticipant();
        }

        private void button_clearParticipantsList_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_ClearParticipantsList.Image = global::Azar.Properties.Resources.buttonClearList2;
        }

        private void button_clearParticipantsList_MouseUp(object sender, MouseEventArgs e)
        {
            this.button_ClearParticipantsList.Image = global::Azar.Properties.Resources.buttonClearList;
            listManager.clearParticipantsList();
        }
        // <<<<< GRUPA UCZESTNICY

        // GRUPA NAGRODY >>>>>
        private void button_addAwards_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_AddAwards.Image = global::Azar.Properties.Resources.buttonAdd2;
        }

        private void button_addAwards_MouseUp(object sender, MouseEventArgs e)
        {
            this.button_AddAwards.Image = global::Azar.Properties.Resources.buttonAdd;
            if (!Application.OpenForms.OfType<AddAwards>().Any())
            {
                new AddAwards(lastAwardWeight, this).Show();
            }
        }

        private void button_removeAwards_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_RemoveAwards.Image = global::Azar.Properties.Resources.buttonRemove2;
        }

        private void button_removeAwards_MouseUp(object sender, MouseEventArgs e)
        {
            this.button_RemoveAwards.Image = global::Azar.Properties.Resources.buttonRemove;
            listManager.DeleteAward();
        }

        private void button_clearAwardsList_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_ClearAwardsList.Image = global::Azar.Properties.Resources.buttonClearList2;
        }

        private void button_clearAwardsList_MouseUp(object sender, MouseEventArgs e)
        {
            this.button_ClearAwardsList.Image = global::Azar.Properties.Resources.buttonClearList;
            listManager.ClearAwardsList();
        }
        // <<<<< GRUPA NAGRODY

        // GRUPA ZWYCIĘZCY >>>>>
        private void button_startRand_MouseDown(object sender, MouseEventArgs e)
        {
            sw = new Stopwatch();

            if (radioButton_RandAward.Checked == true)
                this.button_StartRand.Image = global::Azar.Properties.Resources.buttonRandAward2;
            else
                this.button_StartRand.Image = global::Azar.Properties.Resources.buttonStartRand2;

            sw.Start();
        }

        // ########################### ROZPOCZĘCIE LOSOWANIA ###########################
        private void button_startRand_MouseUp(object sender, MouseEventArgs e)
        {
            sw.Stop();

            if (radioButton_RandAward.Checked == true) // Zmiana wyglądu przycisku
                this.button_StartRand.Image = global::Azar.Properties.Resources.buttonRandAward;
            else
                this.button_StartRand.Image = global::Azar.Properties.Resources.buttonStartRand;

            DialogResult dialogResult = DialogResult.Yes;

            if (!awards.Any() && radioButton_RandAward.Checked)
            {
                MessageBox.Show("Lista nagród jest pusta.", "Brak nagród - Azar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!participants.Any() && !radioButton_RandAward.Checked)
            {
                MessageBox.Show("Lista uczestników jest pusta.", "Brak uczestników - Azar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (winners.Any())
                    dialogResult = MessageBox.Show("Rozpoczęcie losowania spowoduje wyczyszczenie listy wygranych.\nCzy chcesz kontynuować?", "Uwaga - Azar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    this.button_StartRand.Enabled = false;

                    numericUpDown_NumberOfWinners_Click(null, null);
                    
                    listBox_Winners.DataSource = null;
                    listBox_Winners.Items.Clear();
                    winners.Clear();
                    winners = new List<Winner> { };

                    Random rnd = new Random();
                    int elapsedTime = rnd.Next(100000);

                    try
                    {
                        string elapsedTimeString = (sw.Elapsed.TotalMilliseconds.ToString()).Replace(",", String.Empty);
                        elapsedTime = int.Parse(elapsedTimeString);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    randProcess_Thread = new Thread(() => randMetods.Start(elapsedTime));
                    randProcess_Thread.Start();
                }
            }
        }

        private void button_clearWinnersList_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_ClearWinnersList.Image = global::Azar.Properties.Resources.buttonClearList2;
        }

        private void button_clearWinnersList_MouseUp(object sender, MouseEventArgs e)
        {
            this.button_ClearWinnersList.Image = global::Azar.Properties.Resources.buttonClearList;
            if (winners.Any())
            {
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz wyczyścić listę zwycięzców?", "Czyszczenie listy - Azar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    listBox_Winners.DataSource = null;
                    listBox_Winners.Items.Clear();
                    winners.Clear();
                    winners = new List<Winner> { };
                }
            }
        }
        // <<<<< ZWYCIĘZCY

        // O PROGRAMIE >>>>>
        private void button_About_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_About.Image = global::Azar.Properties.Resources.buttonAbout2;
        }

        private void button_About_MouseUp(object sender, MouseEventArgs e)
        {
            this.button_About.Image = global::Azar.Properties.Resources.buttonAbout;
            if (!Application.OpenForms.OfType<About>().Any())
            {
                new About().Show();
            }
        }
        // <<<<< O PROGRAMIE

        // Menu kontekstowe
        private void iconMenu_Click(object sender, EventArgs e) 
        {
            contextMenu.Show(this.Left + 6, this.Top + 31);
        }

        // Zmiana wyglądu groupBoxa z listą wygranych przy przełaczaniu między losowaniem uczestników i nagród
        private void radioButton_RandAward_CheckedChanged(object sender, EventArgs e) 
        {
            if (radioButton_RandAward.Checked == true)
                saveWinnersListAsParticipants.Enabled = false;
            else
                saveWinnersListAsParticipants.Enabled = true;

            if (radioChangeWasCanceled)
            {
                radioChangeWasCanceled = false;
            }
            else
            {
                DialogResult dialogResult = DialogResult.No;
                if (winners.Any())
                    dialogResult = MessageBox.Show("Zmiana tej konfiguracji spowoduje wyczyszczenie listy.\nCzy chcesz kontynuować?", "Zmiana konfiguracji - Azar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes || !winners.Any())
                {
                    listBox_Winners.DataSource = null;
                    listBox_Winners.Items.Clear();
                    winners.Clear();
                    winners = new List<Winner> { };

                    if (radioButton_RandAward.Checked == true)
                    {
                        groupBox_Winners.Text = "Wylosowana nagroda";
                        this.button_StartRand.Image = global::Azar.Properties.Resources.buttonRandAward;
                    }
                    else
                    {
                        groupBox_Winners.Text = "Wylosowani zwycięzcy";
                        this.button_StartRand.Image = global::Azar.Properties.Resources.buttonStartRand;
                    }
                }
                else
                {
                    radioChangeWasCanceled = true;

                    if (radioButton_RandAward.Checked == false)
                        radioButton_RandAward.Checked = true;
                    else
                        radioButton_PickNumberOfWinners.Checked = true;
                }
            }
        }

        // Otwarcie okna dodawania uczestników przy wciśnięciu enter, gdy lista uczestników aktywna
        private void listBox_Participants_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!Application.OpenForms.OfType<AddParticipants>().Any())
                {
                    new AddParticipants(this).Show();
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                listManager.DeleteParticipant();
            }
        }

        // Otwarcie okna dodawania nagrody przy wciśnięciu enter, gdy lista nagród aktywna
        private void listBox_Awards_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!Application.OpenForms.OfType<AddAwards>().Any())
                {
                    new AddAwards(lastAwardWeight, this).Show();
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                listManager.DeleteAward();
            }
        }

        private void numericUpDown_NumberOfWinners_ValueChanged(object sender, EventArgs e)
        {
            if ((int)numericUpDown_NumberOfWinners.Value == 1)
                label_PickNumberOfWinners.Text = "zwycięzcę";
            else
                label_PickNumberOfWinners.Text = "zwycięzców";
        }

        public void numericUpDown_NumberOfWinners_Click(object sender, EventArgs e)
        {
            numericUpDown_NumberOfWinners.Maximum = participants.Count();
            if (numericUpDown_NumberOfWinners.Maximum > 0 && numericUpDown_NumberOfWinners.Value < 2)
                numericUpDown_NumberOfWinners.Value = 1;
        }

        // Żeby żaden przedmiot na liście wygranych nie był zaznaczony
        private void listBox_Winners_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox_Winners.SelectedIndex = -1;
        }

        private void checkBox_SkipWinners_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_SkipWinners.Checked && !skipWinnersAutoChange)
            {
                skipWinnersAutoChange = false;
                MessageBox.Show("Wylosowani zwycięzcy będą usuwani z listy uczestników.", "Informacja - Azar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Poniżej metody odpowiadające za menu kontekstowe
        private void loadProgramState_Click(object sender, EventArgs e)
        {
            menuMethods.LoadProgramState();
        }

        private void loadParticipantsList_Click(object sender, EventArgs e)
        {
            menuMethods.LoadParticipants();
        }

        private void loadAwardsList_Click(object sender, EventArgs e)
        {
            menuMethods.LoadAwards();
        }

        private void loadWinnersList_Click(object sender, EventArgs e)
        {
            menuMethods.LoadWinners();
        }

        private void saveProgramState_Click(object sender, EventArgs e)
        {
            menuMethods.SaveProgramState();
        }

        private void saveParticipantsList_Click(object sender, EventArgs e)
        {
            menuMethods.SaveParticipants();
        }

        private void saveParticipantsListAsTxt_Click(object sender, EventArgs e)
        {
            menuMethods.SaveParticipantsAsTxt();
        }

        private void saveAwardsList_Click(object sender, EventArgs e)
        {
            menuMethods.SaveAwards();
        }

        private void saveAwardsListAsTxt_Click(object sender, EventArgs e)
        {
            menuMethods.SaveAwardsAsTxt();
        }

        private void saveWinnersList_Click(object sender, EventArgs e)
        {
            menuMethods.SaveWinners();
        }

        private void saveWinnersListAsParticipants_Click(object sender, EventArgs e)
        {
            menuMethods.SaveWinnersAsParticipants();
        }

        private void saveWinnersListAsTxt_Click(object sender, EventArgs e)
        {
            menuMethods.SaveWinnersAsTxt();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.Xml.Linq;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Azar
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }

    public class RandMethod
    {
        MainWindow mainWindow;

        public RandMethod(MainWindow mainWindowP)
        {
            mainWindow = mainWindowP;
        }

        public void Start(int seed) // Rozpoczęcie sekwencji losowania //
        {
            SoundPlayer simpleSound;
            int delay = 30, delay2 = 700;

            // Efekty dźwiękowe
            if (mainWindow.checkBox_Mute.Checked == false)
            {
                if (mainWindow.radioButton_RandAward.Checked)
                {
                    simpleSound = new SoundPlayer(global::Azar.Properties.Resources.ring);
                    delay = 65;
                }
                else
                {
                    simpleSound = new SoundPlayer(global::Azar.Properties.Resources.thewinner);
                }
                simpleSound.Load();
                simpleSound.Play();

                for (int i = 10; i <= 100; i++)
                {
                    Thread.Sleep(delay);
                    mainWindow.progressBar_winners.Value = i;
                }
                delay2 = 1000;
            }

            mainWindow.progressBar_winners.Value = 100;
            Thread.Sleep(delay2);

            // Decyzja o rodzaju losowania
            if (mainWindow.radioButton_RandAward.Checked)
                RandAward(seed); // Losuj nagrodę
            else
                RandWinners(seed); // Losuj z zwycięzcow z nagrodami lub bez

            // Efekty dźwiękowe
            if (mainWindow.checkBox_Mute.Checked == false)
            {
                simpleSound = new SoundPlayer(global::Azar.Properties.Resources.tada);
                simpleSound.PlaySync();
            }

            mainWindow.progressBar_winners.Value = 0;
            mainWindow.button_StartRand.Enabled = true;
        }

        // Losowanie zwycięzców z nagrodami lub bez
        private void RandWinners(int seed)
        {
            int numberOfWinners = 1;

            if (mainWindow.radioButton_PickNumberOfWinners.Checked)
            {
                numberOfWinners = (int)mainWindow.numericUpDown_NumberOfWinners.Value;
            }

            if (mainWindow.awards.Count() > 0 && mainWindow.participants.Count() > 0) // Losuj uczestników i nagrody
            {
                try
                {
                    mainWindow.listBox_Winners.Font = new System.Drawing.Font("Microsoft Sans Serif", (float)8.25, System.Drawing.FontStyle.Bold);

                    List<Award> selectedAwards = new List<Award> { }; // Lista na której odbędzie się losowanie nagrod

                    // Dodawanie nagród o określonej wadze do listy na której odbędzie się losowanie
                    if (mainWindow.checkBox_OnlyAwardsWithSpecificWeight.Checked)
                    {
                        int awardsSelectedValue = (int)mainWindow.numericUpDown_AwardsWeight.Value;
                        for (int i = 0; i < mainWindow.awards.Count(); i++)
                        {
                            if (mainWindow.awards[i].weight == awardsSelectedValue)
                                selectedAwards.Add(new Award(mainWindow.awards[i].name, mainWindow.awards[i].amount, mainWindow.awards[i].weight));
                        }
                    }
                    else // Jeżeli nie określono wagi losowanych nagród
                    {
                        selectedAwards = mainWindow.awards;
                    }

                    Random rnd = new Random(seed);
                    int rndParticipantIndex = 0, rndAwardIndex = 0;

                    for (int i = 1; i <= numberOfWinners; i++)
                    {
                        rndParticipantIndex = rnd.Next(mainWindow.participants.Count());
                        rndAwardIndex = rnd.Next(selectedAwards.Count());

                        // Znajduje index przedmiotu na wlaściwej liście nagród
                        int awardIndex = mainWindow.awards.FindIndex(item => item.name == selectedAwards[rndAwardIndex].name);

                        // Jeżeli nagrody się skończyły to zakończ losowanie
                        if (awardIndex < 0)
                            break;

                        // Dodaj zwycięzcow i wylosowane dla nich nagrody do listy
                        mainWindow.winners.Add(new Winner(i, mainWindow.participants[rndParticipantIndex].name, mainWindow.awards[awardIndex].name));

                        if (mainWindow.checkBox_SkipWinners.Checked) // Usuwa wylosowanych zwycięzców z listy uczestników
                        {
                            mainWindow.listBox_Participants.DataSource = null;
                            mainWindow.listBox_Participants.Items.Clear();
                            mainWindow.participants.RemoveAt(rndParticipantIndex);
                            mainWindow.listBox_Participants.DataSource = mainWindow.participants;
                            mainWindow.listBox_Participants.DisplayMember = "name"; // Wyświetlaj na liście uczestników nazwę uczestnika
                        }

                        if (mainWindow.checkBox_AwardsAmountReduce.Checked) // Usuwa wylosowane nagrody, zmniejszając ich ilość
                        {
                            if (awardIndex >= 0)
                            {
                                mainWindow.listBox_Awards.DataSource = null;
                                mainWindow.listBox_Awards.Items.Clear();

                                if (mainWindow.awards[awardIndex].amount <= 1)
                                    mainWindow.awards.RemoveAt(awardIndex);
                                else
                                    mainWindow.awards[awardIndex].amount -= 1;

                                mainWindow.listBox_Awards.DataSource = mainWindow.awards;
                                mainWindow.listBox_Awards.DisplayMember = "displayAll"; // Wyświetlaj na liście nagród wczystkie dostepne informacje
                            }
                        }

                    }

                    mainWindow.listBox_Winners.DataSource = mainWindow.winners;
                    mainWindow.listBox_Winners.DisplayMember = "displayWinnerAndAward"; // Wyświetlaj na liście wygranych nazwę zwycięzcy i wylosowaną dla niego nagrodę
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else if (mainWindow.participants.Count() > 0) // Jeżeli lista nagród jest pusta to losuj samych uczestników
            {
                try
                {
                    mainWindow.listBox_Winners.Font = new System.Drawing.Font("Microsoft Sans Serif", 14, System.Drawing.FontStyle.Bold);

                    Random rnd = new Random(seed);
                    int rndIndex = 0;

                    for (int i = 1; i <= numberOfWinners; i++)
                    {
                        rndIndex = rnd.Next(mainWindow.participants.Count());

                        // Dodaje wylosowanego uczestnika do listy zwyciezców
                        mainWindow.winners.Add(new Winner(i, mainWindow.participants[rndIndex].name, ""));

                        // Usuwa wylosowanych zwycięzców z listy uczestników
                        if (mainWindow.checkBox_SkipWinners.Checked)
                        {
                            mainWindow.listBox_Participants.DataSource = null;
                            mainWindow.listBox_Participants.Items.Clear();
                            mainWindow.participants.RemoveAt(rndIndex);
                            mainWindow.listBox_Participants.DataSource = mainWindow.participants;
                            mainWindow.listBox_Participants.DisplayMember = "name"; // Wyświetlaj na liście uczestników nazwę uczestnika
                        }

                    }

                    mainWindow.listBox_Winners.DataSource = mainWindow.winners;
                    mainWindow.listBox_Winners.DisplayMember = "displayWinner"; // Wyświetlaj na liście wygranych nazwę zwycięzcy
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        // Losowanie samej nagrody
        private void RandAward(int seed)
        {
            try
            {
                mainWindow.listBox_Winners.Font = new System.Drawing.Font("Microsoft Sans Serif", 14, System.Drawing.FontStyle.Bold);
                List<Award> selectedAwards = new List<Award> { }; // Lista na której odbędzie się losowanie

                // Dodanie do listy na ktorej odbędzie się losowanie nagród o wybranej wadze
                if (mainWindow.checkBox_OnlyAwardsWithSpecificWeight.Checked)
                {
                    int awardsSelectedValue = (int)mainWindow.numericUpDown_AwardsWeight.Value;
                    for (int i = 0; i < mainWindow.awards.Count(); i++)
                    {
                        if (mainWindow.awards[i].weight == awardsSelectedValue)
                            selectedAwards.Add(new Award(mainWindow.awards[i].name, -1, -1));
                    }
                }
                else // Jeżeli nie wybrano wagi losowanych nagród
                {
                    selectedAwards = mainWindow.awards;
                }

                Random rnd = new Random(seed);
                int rndIndex = 0;
                rndIndex = rnd.Next(selectedAwards.Count());
                // Dodanie do listy wygranych wylosowanej nagrody
                mainWindow.winners.Add(new Winner(1, "", selectedAwards[rndIndex].name));

                // Zmniejszenie ilości dostepnych nagród
                if (mainWindow.checkBox_AwardsAmountReduce.Checked)
                {
                    int index = mainWindow.awards.FindIndex(item => item.name == selectedAwards[rndIndex].name);
                    if (index >= 0)
                    {
                        mainWindow.listBox_Awards.DataSource = null;
                        mainWindow.listBox_Awards.Items.Clear();

                        if (mainWindow.awards[index].amount <= 1)
                            mainWindow.awards.RemoveAt(index);
                        else
                            mainWindow.awards[index].amount -= 1;

                        mainWindow.listBox_Awards.DataSource = mainWindow.awards;
                        mainWindow.listBox_Awards.DisplayMember = "displayAll";
                    }
                }

                mainWindow.listBox_Winners.DataSource = mainWindow.winners;
                mainWindow.listBox_Winners.DisplayMember = "award"; // Wyświetla na liście wygranych nazwę nagrody
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }

    public class ListManager
    {
        MainWindow mainWindow;

        public ListManager(MainWindow mainWindowP)
        {
            mainWindow = mainWindowP;
        }

        // Usuwa zaznaczonych uczestników
        public void DeleteParticipant()
        {
            if (mainWindow.listBox_Participants.SelectedIndex != -1)
            {
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć zaznaczonych uczestników?", "Usuwanie uczestników - Azar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (mainWindow.listBox_Participants.SelectedIndex != -1)
                    {
                        for (int x = mainWindow.listBox_Participants.SelectedIndices.Count - 1; x >= 0; x--)
                        {
                            int idx = mainWindow.listBox_Participants.SelectedIndices[x];
                            mainWindow.participants.RemoveAt(idx);
                        }
                        mainWindow.listBox_Participants.DataSource = null;
                        mainWindow.listBox_Participants.Items.Clear();
                        mainWindow.listBox_Participants.DataSource = mainWindow.participants;
                        mainWindow.listBox_Participants.DisplayMember = "name";
                    }
                }
                mainWindow.numericUpDown_NumberOfWinners_Click(null, null);
            }
        }

        // Usuwa zaznaczone nagrody
        public void DeleteAward()
        {
            if (mainWindow.listBox_Awards.SelectedIndex != -1)
            {
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć zaznaczone nagrody?", "Usuwanie nagród - Azar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (mainWindow.listBox_Awards.SelectedIndex != -1)
                    {
                        for (int x = mainWindow.listBox_Awards.SelectedIndices.Count - 1; x >= 0; x--)
                        {
                            int idx = mainWindow.listBox_Awards.SelectedIndices[x];
                            mainWindow.awards.RemoveAt(idx);
                        }
                        mainWindow.listBox_Awards.DataSource = null;
                        mainWindow.listBox_Awards.Items.Clear();
                        mainWindow.listBox_Awards.DataSource = mainWindow.awards;
                        mainWindow.listBox_Awards.DisplayMember = "displayAll";
                    }
                }
            }
        }

        // Czyści listę uczestników
        public void clearParticipantsList()
        {
            if (mainWindow.participants.Any())
            {
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz wyczyścić listę uczestników?", "Czyszczenie listy - Azar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    mainWindow.listBox_Participants.DataSource = null;
                    mainWindow.listBox_Participants.Items.Clear();
                    mainWindow.participants.Clear();
                    mainWindow.participants = new List<Participant> { };
                }
                mainWindow.numericUpDown_NumberOfWinners_Click(null, null);
            }
        }

        // Czyści listę nagród
        public void ClearAwardsList()
        {
            if (mainWindow.awards.Any())
            {
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz wyczyścić listę nagród?", "Czyszczenie listy - Azar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    mainWindow.listBox_Awards.DataSource = null;
                    mainWindow.listBox_Awards.Items.Clear();
                    mainWindow.awards.Clear();
                    mainWindow.awards = new List<Award> { };
                }
            }
        }
    }

    public class MenuMethod
    {
        MainWindow mainWindow;

        XmlDocument xmlDocument;
        XmlNode node;
        XmlNode childNode;
        XmlAttribute xmlAttribute;

        public MenuMethod(MainWindow mainWindowP)
        {
            mainWindow = mainWindowP;
        }

        public void LoadProgramState()
        {
            xmlDocument = new XmlDocument();
            int total;
            DialogResult dialogResult = DialogResult.Yes;

            if (mainWindow.awards.Any())
                dialogResult = MessageBox.Show("Wczytanie stanu programu spowoduje utratę obecnych danych.\nCzy chcesz kontynuować?", "Uwaga - Azar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Plik XML (*.xml)|*.xml";
                openFileDialog.Title = "Wczytaj stan programu";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        xmlDocument.Load(openFileDialog.FileName);
                        node = xmlDocument.SelectSingleNode("Azar");

                        if (node.Attributes["listType"].Value.ToString() == "programState")
                        {
                            // Wcztywanie informcji o stanie kontrolek

                            ////////////////////////////////////////////////////////////////////////
                            mainWindow.radioChangeWasCanceled = true;
                            if (node.Attributes["radioButton_Rand"].Value.ToString() == "awards")
                                mainWindow.radioButton_RandAward.Checked = true;
                            else
                                mainWindow.radioButton_PickNumberOfWinners.Checked = true;
                            ////////////////////////////////////////////////////////////////////////
                            mainWindow.numericUpDown_NumberOfWinners.Maximum = int.Parse(node.Attributes["numericUpDown_NumberOfWinners"].Value);
                            mainWindow.numericUpDown_NumberOfWinners.Value = int.Parse(node.Attributes["numericUpDown_NumberOfWinners"].Value);
                            ////////////////////////////////////////////////////////////////////////
                            if (node.Attributes["checkBox_AwardsAmountReduce"].Value.ToString() == "true")
                                mainWindow.checkBox_AwardsAmountReduce.Checked = true;
                            else
                                mainWindow.checkBox_AwardsAmountReduce.Checked = false;
                            ////////////////////////////////////////////////////////////////////////
                            if (node.Attributes["checkBox_SkipWinners"].Value.ToString() == "true")
                            {
                                mainWindow.skipWinnersAutoChange = true;
                                mainWindow.checkBox_SkipWinners.Checked = true;
                            }
                            else
                            {
                                mainWindow.checkBox_SkipWinners.Checked = false;
                            }
                            ////////////////////////////////////////////////////////////////////////
                            if (node.Attributes["checkBox_OnlyAwardsWithSpecificWeight"].Value.ToString() == "true")
                                mainWindow.checkBox_OnlyAwardsWithSpecificWeight.Checked = true;
                            else
                                mainWindow.checkBox_OnlyAwardsWithSpecificWeight.Checked = false;
                            ////////////////////////////////////////////////////////////////////////
                            mainWindow.numericUpDown_AwardsWeight.Maximum = int.Parse(node.Attributes["numericUpDown_NumberOfWinners"].Value);
                            mainWindow.numericUpDown_AwardsWeight.Value = int.Parse(node.Attributes["numericUpDown_AwardsWeight"].Value);
                            ////////////////////////////////////////////////////////////////////////
                            if (node.Attributes["checkBox_Mute"].Value.ToString() == "true")
                                mainWindow.checkBox_Mute.Checked = true;
                            else
                                mainWindow.checkBox_Mute.Checked = false;
                            ////////////////////////////////////////////////////////////////////////

                            // Wczytywanie listy uczestników z pliku XML
                            node = xmlDocument.SelectSingleNode("Azar/Participants");
                            total = int.Parse(node.Attributes["total"].Value);

                            mainWindow.listBox_Participants.DataSource = null;
                            mainWindow.listBox_Participants.Items.Clear();
                            mainWindow.participants.Clear();
                            mainWindow.participants = new List<Participant> { };

                            for (int id = 1; id <= total; id++)
                            {
                                node = xmlDocument.SelectSingleNode("Azar/Participants/" + "ID_" + id.ToString());
                                mainWindow.participants.Add(new Participant(node.Attributes["name"].Value.ToString()));
                            }

                            mainWindow.listBox_Participants.DataSource = mainWindow.participants;
                            mainWindow.listBox_Participants.DisplayMember = "name";

                            // Wczytywanie listy nagród z pliku XML
                            node = xmlDocument.SelectSingleNode("Azar/Awards");
                            total = int.Parse(node.Attributes["total"].Value);

                            mainWindow.listBox_Awards.DataSource = null;
                            mainWindow.listBox_Awards.Items.Clear();
                            mainWindow.awards.Clear();
                            mainWindow.awards = new List<Award> { };

                            for (int id = 1; id <= total; id++)
                            {
                                node = xmlDocument.SelectSingleNode("Azar/Awards/" + "ID_" + id.ToString());
                                mainWindow.awards.Add(new Award(node.Attributes["name"].Value.ToString(), int.Parse(node.Attributes["amount"].Value), int.Parse(node.Attributes["weight"].Value)));
                            }

                            mainWindow.listBox_Awards.DataSource = mainWindow.awards;
                            mainWindow.listBox_Awards.DisplayMember = "displayAll";

                            // Wczytywanie listy wygranych z pliku XML
                            node = xmlDocument.SelectSingleNode("Azar/Winners");
                            total = int.Parse(node.Attributes["total"].Value);

                            mainWindow.listBox_Winners.DataSource = null;
                            mainWindow.listBox_Winners.Items.Clear();
                            mainWindow.winners.Clear();
                            mainWindow.winners = new List<Winner> { };

                            for (int id = 1; id <= total; id++)
                            {
                                node = xmlDocument.SelectSingleNode("Azar/Winners/" + "ID_" + id.ToString());
                                mainWindow.winners.Add(new Winner(int.Parse(node.Attributes["id"].Value), node.Attributes["winner"].Value.ToString(), node.Attributes["award"].Value.ToString()));
                            }

                            mainWindow.listBox_Winners.DataSource = mainWindow.winners;
                            node = xmlDocument.SelectSingleNode("Azar/Winners");
                            mainWindow.listBox_Winners.DisplayMember = node.Attributes["display"].Value.ToString();

                            mainWindow.radioChangeWasCanceled = false;
                        }
                        else
                        {
                            MessageBox.Show("Podany plik nie zawiera stanu programu.", "Nieprawidłowy plik - Azar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        public void LoadParticipants()
        {
            xmlDocument = new XmlDocument();
            int total;
            DialogResult dialogResult = DialogResult.Yes;

            if (mainWindow.participants.Any())
                dialogResult = MessageBox.Show("Wczytanie listy uczestników spowoduje utratę obecnej.\nCzy chcesz kontynuować?", "Uwaga - Azar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Plik XML (*.xml)|*.xml";
                openFileDialog.Title = "Wczytaj listę uczestników";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        xmlDocument.Load(openFileDialog.FileName);
                        node = xmlDocument.SelectSingleNode("Azar");


                        if (node.Attributes["listType"].Value.ToString() == "participant")
                        {
                            total = int.Parse(node.Attributes["total"].Value);

                            mainWindow.listBox_Participants.DataSource = null;
                            mainWindow.listBox_Participants.Items.Clear();
                            mainWindow.participants.Clear();
                            mainWindow.participants = new List<Participant> { };

                            for (int id = 1; id <= total; id++)
                            {
                                node = xmlDocument.SelectSingleNode("Azar/" + "ID_" + id.ToString());
                                mainWindow.participants.Add(new Participant(node.Attributes["name"].Value.ToString()));
                            }

                            mainWindow.listBox_Participants.DataSource = mainWindow.participants;
                            mainWindow.listBox_Participants.DisplayMember = "name";
                        }
                        else
                        {
                            MessageBox.Show("Podany plik nie zawiera listy uczestników.", "Nieprawidłowy plik - Azar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        public void LoadAwards()
        {
            xmlDocument = new XmlDocument();
            int total;
            DialogResult dialogResult = DialogResult.Yes;

            if (mainWindow.awards.Any())
                dialogResult = MessageBox.Show("Wczytanie listy nagród spowoduje utratę obecnej.\nCzy chcesz kontynuować?", "Uwaga - Azar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Plik XML (*.xml)|*.xml";
                openFileDialog.Title = "Wczytaj listę nagród";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        xmlDocument.Load(openFileDialog.FileName);
                        node = xmlDocument.SelectSingleNode("Azar");

                        if (node.Attributes["listType"].Value.ToString() == "award")
                        {
                            total = int.Parse(node.Attributes["total"].Value);

                            mainWindow.listBox_Awards.DataSource = null;
                            mainWindow.listBox_Awards.Items.Clear();
                            mainWindow.awards.Clear();
                            mainWindow.awards = new List<Award> { };

                            for (int id = 1; id <= total; id++)
                            {
                                node = xmlDocument.SelectSingleNode("Azar/" + "ID_" + id.ToString());
                                mainWindow.awards.Add(new Award(node.Attributes["name"].Value.ToString(), int.Parse(node.Attributes["amount"].Value), int.Parse(node.Attributes["weight"].Value)));
                            }

                            mainWindow.listBox_Awards.DataSource = mainWindow.awards;
                            mainWindow.listBox_Awards.DisplayMember = "displayAll";
                        }
                        else
                        {
                            MessageBox.Show("Podany plik nie zawiera listy nagród.", "Nieprawidłowy plik - Azar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        public void LoadWinners()
        {
            xmlDocument = new XmlDocument();
            int total;
            DialogResult dialogResult = DialogResult.Yes;

            if (mainWindow.winners.Any())
                dialogResult = MessageBox.Show("Wczytanie listy wygranych spowoduje utratę obecnej.\nCzy chcesz kontynuować?", "Uwaga - Azar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Plik XML (*.xml)|*.xml";
                openFileDialog.Title = "Wczytaj listę wygranych";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        xmlDocument.Load(openFileDialog.FileName);
                        node = xmlDocument.SelectSingleNode("Azar");

                        if (node.Attributes["listType"].Value.ToString() == "winner")
                        {
                            total = int.Parse(node.Attributes["total"].Value);

                            mainWindow.listBox_Winners.DataSource = null;
                            mainWindow.listBox_Winners.Items.Clear();
                            mainWindow.winners.Clear();
                            mainWindow.winners = new List<Winner> { };

                            for (int id = 1; id <= total; id++)
                            {
                                node = xmlDocument.SelectSingleNode("Azar/" + "ID_" + id.ToString());
                                mainWindow.winners.Add(new Winner(int.Parse(node.Attributes["id"].Value), node.Attributes["winner"].Value.ToString(), node.Attributes["award"].Value.ToString()));
                            }

                            mainWindow.listBox_Winners.DataSource = mainWindow.winners;
                            node = xmlDocument.SelectSingleNode("Azar");

                            mainWindow.radioChangeWasCanceled = true;

                            if (node.Attributes["display"].Value.ToString() == "award")
                                mainWindow.radioButton_RandAward.Checked = true;
                            else
                                mainWindow.radioButton_PickNumberOfWinners.Checked = true;

                            mainWindow.listBox_Winners.DisplayMember = node.Attributes["display"].Value.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Podany plik nie zawiera listy wygranych.", "Nieprawidłowy plik - Azar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        public void SaveProgramState()
        {
            xmlDocument = new XmlDocument();
            int id = 0;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Plik XML (*.xml)|*.xml";
            saveFileDialog.Title = "Zapisz stan programu";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlNode secondChildNode;
                    xmlDocument.AppendChild(xmlDeclaration);

                    ////////////////////////////////////////////////////////////////////////
                    node = xmlDocument.CreateElement("Azar");
                    xmlAttribute = xmlDocument.CreateAttribute("listType");
                    xmlAttribute.Value = "programState";
                    node.Attributes.Append(xmlAttribute);
                    ////////////////////////////////////////////////////////////////////////
                    xmlAttribute = xmlDocument.CreateAttribute("radioButton_Rand");
                    if (mainWindow.radioButton_RandAward.Checked)
                        xmlAttribute.Value = "awards";
                    else
                        xmlAttribute.Value = "participants";
                    node.Attributes.Append(xmlAttribute);
                    ////////////////////////////////////////////////////////////////////////
                    xmlAttribute = xmlDocument.CreateAttribute("numericUpDown_NumberOfWinners");
                    xmlAttribute.Value = mainWindow.numericUpDown_NumberOfWinners.Value.ToString();
                    node.Attributes.Append(xmlAttribute);
                    ////////////////////////////////////////////////////////////////////////
                    xmlAttribute = xmlDocument.CreateAttribute("checkBox_AwardsAmountReduce");
                    if (mainWindow.checkBox_AwardsAmountReduce.Checked)
                        xmlAttribute.Value = "true";
                    else
                        xmlAttribute.Value = "false";
                    node.Attributes.Append(xmlAttribute);
                    ////////////////////////////////////////////////////////////////////////
                    xmlAttribute = xmlDocument.CreateAttribute("checkBox_SkipWinners");
                    if (mainWindow.checkBox_SkipWinners.Checked)
                        xmlAttribute.Value = "true";
                    else
                        xmlAttribute.Value = "false";
                    node.Attributes.Append(xmlAttribute);
                    ////////////////////////////////////////////////////////////////////////
                    xmlAttribute = xmlDocument.CreateAttribute("checkBox_OnlyAwardsWithSpecificWeight");
                    if (mainWindow.checkBox_OnlyAwardsWithSpecificWeight.Checked)
                        xmlAttribute.Value = "true";
                    else
                        xmlAttribute.Value = "false";
                    node.Attributes.Append(xmlAttribute);
                    ////////////////////////////////////////////////////////////////////////
                    xmlAttribute = xmlDocument.CreateAttribute("numericUpDown_AwardsWeight");
                    xmlAttribute.Value = mainWindow.numericUpDown_AwardsWeight.Value.ToString();
                    node.Attributes.Append(xmlAttribute);
                    ////////////////////////////////////////////////////////////////////////
                    xmlAttribute = xmlDocument.CreateAttribute("checkBox_Mute");
                    if (mainWindow.checkBox_Mute.Checked)
                        xmlAttribute.Value = "true";
                    else
                        xmlAttribute.Value = "false";
                    node.Attributes.Append(xmlAttribute);
                    ////////////////////////////////////////////////////////////////////////

                    // Zapisywanie listy uczestnikow do pliku XML
                    id = 0;
                    childNode = xmlDocument.CreateElement("Participants");

                    foreach (var item in mainWindow.participants)
                    {
                        id++;
                        secondChildNode = xmlDocument.CreateElement("ID_" + id.ToString());

                        xmlAttribute = xmlDocument.CreateAttribute("name");
                        xmlAttribute.Value = item.name.ToString();
                        secondChildNode.Attributes.Append(xmlAttribute);

                        childNode.AppendChild(secondChildNode);
                    }

                    xmlAttribute = xmlDocument.CreateAttribute("total");
                    xmlAttribute.Value = id.ToString();
                    childNode.Attributes.Append(xmlAttribute);

                    node.AppendChild(childNode);

                    // Zapisywanie listy nagród do pliku XML
                    id = 0;
                    childNode = xmlDocument.CreateElement("Awards");

                    foreach (var item in mainWindow.awards)
                    {
                        id++;
                        secondChildNode = xmlDocument.CreateElement("ID_" + id.ToString());

                        xmlAttribute = xmlDocument.CreateAttribute("name");
                        xmlAttribute.Value = item.name.ToString();
                        secondChildNode.Attributes.Append(xmlAttribute);

                        xmlAttribute = xmlDocument.CreateAttribute("amount");
                        xmlAttribute.Value = item.amount.ToString();
                        secondChildNode.Attributes.Append(xmlAttribute);

                        xmlAttribute = xmlDocument.CreateAttribute("weight");
                        xmlAttribute.Value = item.weight.ToString();
                        secondChildNode.Attributes.Append(xmlAttribute);

                        childNode.AppendChild(secondChildNode);
                    }

                    xmlAttribute = xmlDocument.CreateAttribute("total");
                    xmlAttribute.Value = id.ToString();
                    childNode.Attributes.Append(xmlAttribute);

                    node.AppendChild(childNode);

                    // Zapisywanie listy wygranych do pliku XML
                    id = 0;
                    childNode = xmlDocument.CreateElement("Winners");

                    foreach (var item in mainWindow.winners)
                    {
                        id++;
                        secondChildNode = xmlDocument.CreateElement("ID_" + id.ToString());

                        xmlAttribute = xmlDocument.CreateAttribute("id");
                        xmlAttribute.Value = item.id.ToString();
                        secondChildNode.Attributes.Append(xmlAttribute);

                        xmlAttribute = xmlDocument.CreateAttribute("winner");
                        xmlAttribute.Value = item.winner.ToString();
                        secondChildNode.Attributes.Append(xmlAttribute);

                        xmlAttribute = xmlDocument.CreateAttribute("award");
                        xmlAttribute.Value = item.award.ToString();
                        secondChildNode.Attributes.Append(xmlAttribute);

                        childNode.AppendChild(secondChildNode);
                    }

                    xmlAttribute = xmlDocument.CreateAttribute("total");
                    xmlAttribute.Value = id.ToString();
                    childNode.Attributes.Append(xmlAttribute);

                    xmlAttribute = xmlDocument.CreateAttribute("display");
                    xmlAttribute.Value = mainWindow.listBox_Winners.DisplayMember.ToString();
                    childNode.Attributes.Append(xmlAttribute);

                    node.AppendChild(childNode);

                    xmlDocument.AppendChild(node);
                    xmlDocument.Save(saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public void SaveParticipants()
        {
            xmlDocument = new XmlDocument();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Plik XML (*.xml)|*.xml";
            saveFileDialog.Title = "Zapisz listę uczestników";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                    xmlDocument.AppendChild(xmlDeclaration);

                    node = xmlDocument.CreateElement("Azar");
                    xmlAttribute = xmlDocument.CreateAttribute("listType");
                    xmlAttribute.Value = "participant";
                    node.Attributes.Append(xmlAttribute);

                    int id = 0;

                    foreach (var item in mainWindow.participants)
                    {
                        id++;
                        childNode = xmlDocument.CreateElement("ID_" + id.ToString());
                        xmlAttribute = xmlDocument.CreateAttribute("name");
                        xmlAttribute.Value = item.name.ToString();
                        childNode.Attributes.Append(xmlAttribute);
                        node.AppendChild(childNode);
                    }

                    xmlAttribute = xmlDocument.CreateAttribute("total");
                    xmlAttribute.Value = id.ToString();
                    node.Attributes.Append(xmlAttribute);

                    xmlDocument.AppendChild(node);
                    xmlDocument.Save(saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public void SaveParticipantsAsTxt()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Plik tekstowy (*.txt)|*.txt";
            saveFileDialog.Title = "Zapisz listę uczestników jako plik tekstowy";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter file = new StreamWriter(saveFileDialog.FileName);

                    foreach (var item in mainWindow.participants)
                    {
                        file.WriteLine(item.name);
                    }

                    file.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public void SaveAwards()
        {
            xmlDocument = new XmlDocument();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki XML (*.xml)|*.xml";
            saveFileDialog.Title = "Zapisz listę nagród";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                    xmlDocument.AppendChild(xmlDeclaration);

                    node = xmlDocument.CreateElement("Azar");
                    xmlAttribute = xmlDocument.CreateAttribute("listType");
                    xmlAttribute.Value = "award";
                    node.Attributes.Append(xmlAttribute);

                    int id = 0;

                    foreach (var item in mainWindow.awards)
                    {
                        id++;
                        childNode = xmlDocument.CreateElement("ID_" + id.ToString());

                        xmlAttribute = xmlDocument.CreateAttribute("name");
                        xmlAttribute.Value = item.name.ToString();
                        childNode.Attributes.Append(xmlAttribute);

                        xmlAttribute = xmlDocument.CreateAttribute("amount");
                        xmlAttribute.Value = item.amount.ToString();
                        childNode.Attributes.Append(xmlAttribute);

                        xmlAttribute = xmlDocument.CreateAttribute("weight");
                        xmlAttribute.Value = item.weight.ToString();
                        childNode.Attributes.Append(xmlAttribute);

                        node.AppendChild(childNode);
                    }

                    xmlAttribute = xmlDocument.CreateAttribute("total");
                    xmlAttribute.Value = id.ToString();
                    node.Attributes.Append(xmlAttribute);

                    xmlDocument.AppendChild(node);
                    xmlDocument.Save(saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public void SaveAwardsAsTxt()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Plik tekstowy (*.txt)|*.txt";
            saveFileDialog.Title = "Zapisz listę nagród jako plik tekstowy";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter file = new StreamWriter(saveFileDialog.FileName);

                    foreach (var item in mainWindow.awards)
                    {
                        file.WriteLine(item.amount + "x " + item.name + "[" + item.weight + "]");
                    }

                    file.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public void SaveWinners()
        {
            xmlDocument = new XmlDocument();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki XML (*.xml)|*.xml";
            saveFileDialog.Title = "Zapisz listę wygranych";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                    xmlDocument.AppendChild(xmlDeclaration);

                    node = xmlDocument.CreateElement("Azar");
                    xmlAttribute = xmlDocument.CreateAttribute("listType");
                    xmlAttribute.Value = "winner";
                    node.Attributes.Append(xmlAttribute);

                    xmlAttribute = xmlDocument.CreateAttribute("display");
                    xmlAttribute.Value = mainWindow.listBox_Winners.DisplayMember.ToString();
                    node.Attributes.Append(xmlAttribute);


                    int id = 0;

                    foreach (var item in mainWindow.winners)
                    {
                        id++;
                        childNode = xmlDocument.CreateElement("ID_" + id.ToString());

                        xmlAttribute = xmlDocument.CreateAttribute("id");
                        xmlAttribute.Value = item.id.ToString();
                        childNode.Attributes.Append(xmlAttribute);

                        xmlAttribute = xmlDocument.CreateAttribute("winner");
                        xmlAttribute.Value = item.winner.ToString();
                        childNode.Attributes.Append(xmlAttribute);

                        xmlAttribute = xmlDocument.CreateAttribute("award");
                        xmlAttribute.Value = item.award.ToString();
                        childNode.Attributes.Append(xmlAttribute);

                        node.AppendChild(childNode);
                    }

                    xmlAttribute = xmlDocument.CreateAttribute("total");
                    xmlAttribute.Value = id.ToString();
                    node.Attributes.Append(xmlAttribute);

                    xmlDocument.AppendChild(node);
                    xmlDocument.Save(saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public void SaveWinnersAsParticipants()
        {
            xmlDocument = new XmlDocument();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki XML (*.xml)|*.xml";
            saveFileDialog.Title = "Zapisz listę wygranych jako uczestników";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                    xmlDocument.AppendChild(xmlDeclaration);

                    node = xmlDocument.CreateElement("Azar");
                    xmlAttribute = xmlDocument.CreateAttribute("listType");
                    xmlAttribute.Value = "participant";
                    node.Attributes.Append(xmlAttribute);

                    int id = 0;

                    foreach (var item in mainWindow.winners)
                    {
                        id++;
                        childNode = xmlDocument.CreateElement("ID_" + id.ToString());
                        xmlAttribute = xmlDocument.CreateAttribute("name");
                        xmlAttribute.Value = item.winner.ToString();
                        childNode.Attributes.Append(xmlAttribute);
                        node.AppendChild(childNode);
                    }

                    xmlAttribute = xmlDocument.CreateAttribute("total");
                    xmlAttribute.Value = id.ToString();
                    node.Attributes.Append(xmlAttribute);

                    xmlDocument.AppendChild(node);
                    xmlDocument.Save(saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public void SaveWinnersAsTxt()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Plik tekstowy (*.txt)|*.txt";
            saveFileDialog.Title = "Zapisz listę wygranych jako plik tekstowy";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter file = new StreamWriter(saveFileDialog.FileName);

                    foreach (var item in mainWindow.winners)
                    {
                        if (mainWindow.listBox_Winners.DisplayMember.ToString() == "award")
                            file.WriteLine(item.award);
                        else if (mainWindow.listBox_Winners.DisplayMember.ToString() == "displayWinner")
                            file.WriteLine(item.id.ToString() + ". " + item.winner);
                        else
                            file.WriteLine(item.winner + " - " + item.award);
                    }

                    file.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }

    public class Participant
    {
        public string name { get; set; } // Nazwa uczestnika

        public Participant(string nameP)
        {
            name = nameP;
        }
    }

    public class Award
    {
        public string name { get; set; } // Nazwa nagrody
        public int amount { get; set; } // Ilość nagrody
        public int weight { get; set; } // Waga nagrody

        public Award(string nameP, int amountP, int weightP)
        {
            name = nameP;
            amount = amountP;
            weight = weightP;
        }

        public string displayAll
        {
            get
            {
                return (amount.ToString() + "x " + name + " - [" + weight + "]");
            }
        }
    }

    public class Winner
    {
        public int id { get; set; } // Numer porzadkowy
        public string winner { get; set; } // Nazwa zwycięzcy
        public string award { get; set; } // Nazwa nagrody

        public Winner(int idP, string nameP, string awardP)
        {
            id = idP;
            winner = nameP;
            award = awardP;
        }

        public string displayWinner
        {
            get
            {
                return (id.ToString() + ". " + winner);
            }
        }

        public string displayWinnerAndAward
        {
            get
            {
                return (winner + " - " + award);
            }
        }
    }
}

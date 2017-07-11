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
    public partial class About : Form
    {
        #region Zmienne
        private int togMove;
        private int mValX;
        private int mValY;
        #endregion

        public About()
        {
            InitializeComponent();
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

        private void button_Close_MouseDown(object sender, MouseEventArgs e)
        {
            this.button_Close.Image = global::Azar.Properties.Resources.buttonClose2;
        }

        private void button_Close_MouseUp(object sender, MouseEventArgs e)
        {
            this.Dispose();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("www.flaticon.com/authors/roundicons");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("www.dabuttonfactory.com");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("www.freesound.org");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DivisionOfLifeUpdater
{
    public partial class Menu : Form
    {
        private bool _mouseDown = false;
        private int _startX = 0;
        private int _startY = 0;

        public Menu() {
            InitializeComponent();

            this.picClose.MouseClick += picClose_MouseClick;
            this.picMinimize.MouseClick += picMinimize_MouseClick;
            this.picTab.MouseDown += picTab_MouseDown;
            this.picTab.MouseUp += picTab_MouseUp;
            this.picTab.MouseMove += picTab_MouseMove;
            this.lblPlay.MouseClick += lblPlay_MouseClick;
            this.lblRetry.MouseClick += lblRetry_MouseClick;

            lblDescription.Text = "Division Of Life is a 3d MORPG made in Unity. The game has been in the making for over 3 years, But only recently decided to go public with updates and content revisions. Now is the time where I can start showing stuff that I have been working on as I am nearly complete with the coding side of the game."
                + "\n\n The game it self is not a standard MMORPG, It's more like a dungeon crawler and MORPG hybrid.";
        }

        void lblRetry_MouseClick(object sender, MouseEventArgs e) {
            lblCurrent.Visible = true;
            lblTotal.Visible = true;
            prgTotal.Visible = true;
            prgCurrent.Visible = true;
            lblRetry.Visible = false;
            lblCurrent.Text = "";
            lblTotal.Text = "";

            new Patcher().Begin();
        }

        void lblPlay_MouseClick(object sender, MouseEventArgs e) {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Game\\Division of Life.exe");
            Environment.Exit(1);
        }

        void picTab_MouseUp(object sender, MouseEventArgs e) {
            _mouseDown = false;
        }

        void picTab_MouseDown(object sender, MouseEventArgs e) {
            _mouseDown = true;
            _startX = e.X;
            _startY = e.Y;
        }

        void picTab_MouseMove(object sender, MouseEventArgs e) {
            if (_mouseDown) {
                this.Location = new Point(e.X - _startX + this.Left, e.Y - _startY + this.Top);
            }
        }

        void picMinimize_MouseClick(object sender, MouseEventArgs e) {
            this.WindowState = FormWindowState.Minimized;
        }

        void picClose_MouseClick(object sender, MouseEventArgs e) {
            try {
                Environment.Exit(1);
            } catch {

            }
        }

        private void lblRetry_Click(object sender, EventArgs e)
        {

        }
    }
}

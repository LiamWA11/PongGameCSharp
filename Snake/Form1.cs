using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class GameArea : Form
    {
        const int SCREEN_HEIGHT = 600;
        const int SCREEN_WIDTH = 800;

        public GameArea()
        {
            InitializeComponent();

            this.Width = SCREEN_WIDTH;
            this.Height = SCREEN_HEIGHT;
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}

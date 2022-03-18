using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject_LevelEditor
{
    public partial class InitForm : Form
    {
        public InitForm()
        {
            InitializeComponent();
        }

        private void CreateLevelButton_Click(object sender, EventArgs e)
        {
            LevelEditor newLevel = new LevelEditor(Convert.ToInt32(WidthTextBox.Text), Convert.ToInt32(HeightTextBox.Text));
            newLevel.Show();
        }
    }
}

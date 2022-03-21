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

    public enum TileType
    {
        Wall,
        Enemy,
        Spawn,
        Objective,
        Exit
    }
    public partial class LevelEditor : Form
    {
        //Fields
        private TileType currentTile;

        private List<Component> mapContents;

        private int bWidth;
        private int bHeight;

        //Properties

        //Constructors
        public LevelEditor(int width, int height)
        {
            InitializeComponent();
            int minWidth = PlaceMenu.Width + EditMenu.Width+37;
            float ratio = (float)width / (float)height;
            if(ratio*760 <= (1920-minWidth))
            {
                this.Width = (int)(ratio * 760) + minWidth;
                Level.Width = (int)(ratio * 760);
            }
            else
            {
                this.Width = 1920;
                Level.Width = 1920 - minWidth;
            }
            EditMenu.Location = new Point(this.Width - EditMenu.Width-22, 5);
            PlaceMenu.Location = new Point(5, 5);
            Level.Location = new Point(PlaceMenu.Location.X + PlaceMenu.Width + 5, 5);
            this.Height = 760;
            EditMenu.Height = this.Height - 10 - 40;
            PlaceMenu.Height = this.Height - 10 - 40;
            Level.Height = this.Height - 10 - 40;
            bWidth = Level.Width / width;
            bHeight = Level.Height / height;

            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    PictureBox p = new PictureBox();
                    p.BorderStyle = BorderStyle.None;
                    p.BackColor = Color.White;
                    p.Height = (int)bHeight;
                    p.Width = (int)bWidth;
                    p.Location = new Point((int)(i * bWidth), (int)(j * bHeight));
                    Level.Controls.Add(p);
                }
            }
            currentTile = TileType.Wall;
        }



        //Methods


        //Events
        private void WallButton_Click(object sender, EventArgs e)
        {
            currentTile = TileType.Wall;
        }

        private void EnemyButton_Click(object sender, EventArgs e)
        {
            currentTile = TileType.Enemy;
        }

        private void SpawnButton_Click(object sender, EventArgs e)
        {
            currentTile = TileType.Spawn;
        }

        private void ObjectiveButton_Click(object sender, EventArgs e)
        {
            currentTile = TileType.Objective;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            currentTile = TileType.Exit;
        }

        private void NewPieceButton_Click(object sender, EventArgs e)
        {
            Component comp = new Component(new Point(0, 0), currentTile, Convert.ToInt32(WidthTextBox.Text), Convert.ToInt32(HeightTextBox.Text), bWidth, bHeight);
            foreach(PictureBox p in comp.GetBoxes())
            {
                Level.Controls.Add(p);
            }
        }
    }
}

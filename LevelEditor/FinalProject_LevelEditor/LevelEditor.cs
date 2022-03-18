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
        private List<Enemy> enemies;
        private List<Wall> walls;
        private MapPoint playerSpawn;
        private MapPoint objective;
        private MapPoint exit;

        private TileType currentTile;

        //Properties

        //Constructors
        public LevelEditor(int width, int height)
        {
            InitializeComponent();
            float ratio = (float)width / (float)height;
            if(ratio*760 <= 1920)
            {
                this.Width = (int)(ratio * 760);
            }
            else
            {
                this.Width = 1920;
            }
            this.Height = 760;
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
    }
}

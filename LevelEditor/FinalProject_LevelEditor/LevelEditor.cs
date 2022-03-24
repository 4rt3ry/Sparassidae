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

        private int bWidth;
        private int bHeight;

        private Component selectedComp;

        private int prevSelectedIndex;

        //Properties

        //Constructors
        public LevelEditor(int width, int height)
        {
            InitializeComponent();
            //Size of window determining features
            int minWidth = PlaceMenu.Width + EditMenu.Width+37;
            float ratio = (float)width / (float)height;

            prevSelectedIndex = -1;
            selectedComp = null;

            //Detect ratio and determine width (capped at 1920)
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
            //Set positions of all menu items
            EditMenu.Location = new Point(this.Width - EditMenu.Width-22, 5);
            PlaceMenu.Location = new Point(5, 5);
            Level.Location = new Point(PlaceMenu.Location.X + PlaceMenu.Width + 5, 5);
            this.Height = 760;
            EditMenu.Height = this.Height - 10 - 40 - 65;
            PlaceMenu.Height = this.Height - 10 - 40;
            Level.Height = this.Height - 10 - 40;

            //Keep box size variables
            bWidth = Level.Width / width;
            bHeight = Level.Height / height;

            //Default tile is wall
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

        /// <summary>
        /// Handles creation of new piece and adds it to necessary features
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewPieceButton_Click(object sender, EventArgs e)
        {
            Color c = Color.White;
            switch (currentTile)
            {
                case TileType.Wall:
                    c = WallButton.BackColor;
                    break;
                case TileType.Enemy:
                    c = EnemyButton.BackColor;
                    break;
                case TileType.Spawn:
                    c = SpawnButton.BackColor;
                    break;
                case TileType.Objective:
                    c = ObjectiveButton.BackColor;
                    break;
                case TileType.Exit:
                    c = ExitButton.BackColor;
                    break;
            }
            Component comp = new Component(new Point(0, 0), currentTile, Convert.ToInt32(WidthTextBox.Text), Convert.ToInt32(HeightTextBox.Text), bWidth, bHeight, c);
            Level.Controls.Add(comp.GetBox());
            EditMenu.Items.Add(comp);
            EditMenu.Focus();
            EditMenu.SelectedItem = comp;
        }

        /// <summary>
        /// Handles the switching of the highlighted object, triggered by a change in selection of the EditMenu container
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (EditMenu.SelectedIndex != prevSelectedIndex)
            {
                selectedComp = null;
                if (prevSelectedIndex != -1)
                {
                    Component comp = ((Component)EditMenu.Items[prevSelectedIndex]);
                    comp.Select(false);
                    EditMenu.Items[prevSelectedIndex] = EditMenu.Items[prevSelectedIndex];
                    comp.GetBox().SendToBack();
                }
                    
                if (EditMenu.SelectedIndex != -1)
                {
                    Component comp = ((Component)EditMenu.Items[EditMenu.SelectedIndex]);
                    comp.Select(true);
                    comp.GetBox().BringToFront();
                    selectedComp = comp;
                }
                prevSelectedIndex = EditMenu.SelectedIndex;
            }
        }

        private void Level_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void Level_Enter(object sender, EventArgs e)
        {
        
        }

        /// <summary>
        /// Handles all keyboard input for moving and adjusting the level components
        /// WASD to move pieces around
        /// Shift WASD to extend pieces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditMenu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (selectedComp != null)
            {
                switch (e.KeyChar)
                {
                    case 'w':
                        selectedComp.Move(BoxMovement.Up, 1);
                        break;
                    case 'a':
                        selectedComp.Move(BoxMovement.Left, 1);
                        break;
                    case 's':
                        selectedComp.Move(BoxMovement.Down, 1);
                        break;
                    case 'd':
                        selectedComp.Move(BoxMovement.Right, 1);
                        break;
                    case 'W':
                        selectedComp.Extend(BoxMovement.Up, 1);
                        break;
                    case 'A':
                        selectedComp.Extend(BoxMovement.Left, 1);
                        break;
                    case 'S':
                        selectedComp.Extend(BoxMovement.Down, 1);
                        break;
                    case 'D':
                        selectedComp.Extend(BoxMovement.Right, 1);
                        break;
                }
                if(e.KeyChar == ' ')
                {
                    selectedComp = null;
                    EditMenu.SelectedIndex = -1;
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles key presses for keys that cannot be detected through key pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                Component compRemoved = (Component)EditMenu.Items[EditMenu.SelectedIndex];
                EditMenu.Items.RemoveAt(EditMenu.SelectedIndex);
                Level.Controls.Remove(compRemoved.GetBox());
            }
            if( EditMenu.SelectedIndex == -1)
            {
                if(e.KeyCode == Keys.Enter)
                    NewPieceButton.PerformClick();
                switch (e.KeyCode)
                {
                    case Keys.D1:
                        currentTile = TileType.Wall;
                        NewPieceButton.PerformClick();
                        break;
                    case Keys.D2:
                        currentTile = TileType.Enemy;
                        NewPieceButton.PerformClick();
                        break;
                    case Keys.D3:
                        currentTile = TileType.Spawn;
                        NewPieceButton.PerformClick();
                        break;
                    case Keys.D4:
                        currentTile = TileType.Objective;
                        NewPieceButton.PerformClick();
                        break;
                    case Keys.D5:
                        currentTile = TileType.Exit;
                        NewPieceButton.PerformClick();
                        break;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        Exit,
        RoamPoint,
        GlowStick,
        Arrow
    }
    public partial class LevelEditor : Form
    {
        //Fields
        private TileType currentTile;

        private PictureBox highlighter;

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

            //Set Width
            this.Width = 760 + minWidth;
            Level.Width = 760 - VBar.Width;

            //Set positions of all menu items
            EditMenu.Location = new Point(this.Width - EditMenu.Width-22, 5);
            PlaceMenu.Location = new Point(5, 5);
            Level.Location = new Point(PlaceMenu.Location.X + PlaceMenu.Width + 5, 5);
            this.Height = 760;
            EditMenu.Height = this.Height - SaveButton.Height - ZoomBar.Height - 70;
            PlaceMenu.Height = this.Height - 10 - 40;
            Level.Height = this.Height - 10 - 40 - HBar.Height;
            SaveButton.Location = new Point(EditMenu.Location.X + EditMenu.Width / 2 - SaveButton.Width / 2, EditMenu.Location.Y + EditMenu.Height + ZoomBar.Height + 10);
            ZoomBar.Location = new Point(EditMenu.Location.X, SaveButton.Location.Y - ZoomBar.Height - 5);
            HBar.Location = new Point(Level.Location.X, Level.Location.Y + Level.Height);
            VBar.Location = new Point(Level.Location.X + Level.Width, VBar.Location.Y);

            //Keep box size variables
            bWidth = Level.Width / width;
            bHeight = Level.Height / height;
            ZoomBar.Value = bWidth;


            //Highlighter setup
            highlighter = new PictureBox();
            highlighter.Width = bWidth;
            highlighter.Height = bHeight;
            highlighter.Location = new Point(0, 0);
            highlighter.BackColor = Color.White;
            highlighter.BorderStyle = BorderStyle.FixedSingle;

            //Default tile is wall
            currentTile = TileType.Wall;
        }

        public LevelEditor(int width, int height, String filePath) : this(width, height)
        {
            /// |tiletype,x,y,width,height,roampoints|
            /// enemy tiles will contain a collection of child roam nodes, others will just say "empty"
            /// Roam Point Notation:
            /// [roampoint,x,y,index[
            /// 
            StreamReader reader = new StreamReader(filePath);
            String data = reader.ReadToEnd();
            reader.Close();
            String[] a1 = data.Split('|');
            foreach(String s in a1)
            {
                if(s == "")
                {
                    continue;
                }
                String[] a2 = s.Split(',');
                TileType t = TileType.Wall;
                Color c = Color.White;
                switch (a2[0])
                {
                    case "wall":
                        t = TileType.Wall;
                        c = Color.Black;
                        break;
                    case "enemy":
                        t = TileType.Enemy;
                        c = Color.FromArgb(192, 0, 0);
                        break;
                    case "spawn":
                        t = TileType.Spawn;
                        c = Color.FromArgb(192, 192, 0);
                        break;
                    case "objective":
                        t = TileType.Objective;
                        c = Color.SteelBlue;
                        break;
                    case "exit":
                        t = TileType.Exit;
                        c = Color.Green;
                        break;
                    case "glow":
                        t = TileType.GlowStick;
                        c = Color.Lime;
                        break;
                    case "arrow":
                        t = TileType.Arrow;
                        c = Color.Gray;
                        break;
                }

                int x = Convert.ToInt32(a2[1]);
                int y = Convert.ToInt32(a2[2]);
                int w = Convert.ToInt32(a2[3]);
                int h = Convert.ToInt32(a2[4]);

                

                Component comp = new Component(new Point(x*bWidth, y*bHeight), t, w, h, bWidth, bHeight, c, new Point(-HBar.Value, -VBar.Value));

                Level.Controls.Add(comp.GetBox());
                EditMenu.Items.Add(comp);

                if (a2[5].Equals("empty"))
                {

                }
                else
                {
                    String[] b1 = a2[5].Split('[');
                    foreach(String rp in b1)
                    {
                        if(rp != "")
                        {
                            String[] b2 = rp.Split(']');
                            int rx = Convert.ToInt32(b2[1]);
                            int ry = Convert.ToInt32(b2[2]);
                            Component roampoint = new Component(new Point(rx*bWidth, ry*bHeight), TileType.RoamPoint, 1, 1, bWidth, bHeight, Color.FromArgb(200, 130, 130), new Point(-HBar.Value, -VBar.Value), comp);
                            Level.Controls.Add(roampoint.GetBox());
                            EditMenu.Items.Add(roampoint);
                        }
                    }
                }
            }
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

        private void GlowStickButton_Click(object sender, EventArgs e)
        {
            currentTile = TileType.GlowStick;
        }

        private void ArrowButton_Click(object sender, EventArgs e)
        {
            currentTile = TileType.Arrow;
        }

        /// <summary>
        /// Handles creation of new piece and adds it to necessary features
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewPieceButton_Click(object sender, EventArgs e)
        {
            Color c = Color.White;
            Point adjust = new Point(-HBar.Value, -VBar.Value);
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
                case TileType.RoamPoint:
                    c = Color.FromArgb(200, 130, 130);
                    break;
                case TileType.GlowStick:
                    c = Color.Lime;
                    break;
                case TileType.Arrow:
                    c = Color.Gray;
                    break;
            }
            if(currentTile == TileType.RoamPoint)
            {
                Component roamPointComp = new Component(new Point(0, 0), currentTile, Convert.ToInt32(WidthTextBox.Text), Convert.ToInt32(HeightTextBox.Text), bWidth, bHeight, c, adjust, (Component)EditMenu.SelectedItem);
                ((Component)EditMenu.Items[EditMenu.SelectedIndex]).AddRoamPoint(roamPointComp);
                Level.Controls.Add(roamPointComp.GetBox());
                
                EditMenu.Items.Add(roamPointComp);
                EditMenu.Focus();
                EditMenu.SelectedItem = roamPointComp;
                return;
            }
            Component comp = new Component(highlighter.Location, currentTile, Convert.ToInt32(WidthTextBox.Text), Convert.ToInt32(HeightTextBox.Text), bWidth, bHeight, c, adjust);
            Level.Controls.Add(comp.GetBox());
            if(comp.TileType == TileType.Arrow)
            {
                Level.Controls.Add(comp.GetDirectionIndicator());
            }
            EditMenu.Items.Add(comp);
            EditMenu.Focus();
            EditMenu.SelectedItem = comp;
            selectedComp.Select(false);
            selectedComp = comp;
            selectedComp.Select(true);
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
                if (prevSelectedIndex != -1 && EditMenu.Items.Count != 0)
                {
                    try
                    {
                        Component comp = ((Component)EditMenu.Items[prevSelectedIndex]);
                        comp.Select(false);
                        EditMenu.Items[prevSelectedIndex] = EditMenu.Items[prevSelectedIndex];
                        comp.GetBox().SendToBack();
                    }
                    catch(Exception error)
                    {

                    }
                }
                    
                if (EditMenu.SelectedIndex != -1)
                {
                    Component comp = ((Component)EditMenu.Items[EditMenu.SelectedIndex]);
                    comp.Select(true);
                    comp.GetBox().BringToFront();
                    selectedComp = comp;
                    highlighter.Location = comp.GetBox().Location;
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
                        highlighter.Location = new Point(highlighter.Location.X, highlighter.Location.Y - bHeight);
                        break;
                    case 'a':
                        selectedComp.Move(BoxMovement.Left, 1);
                        highlighter.Location = new Point(highlighter.Location.X - bWidth, highlighter.Location.Y);
                        break;
                    case 's':
                        selectedComp.Move(BoxMovement.Down, 1);
                        highlighter.Location = new Point(highlighter.Location.X, highlighter.Location.Y + bHeight);
                        break;
                    case 'd':
                        selectedComp.Move(BoxMovement.Right, 1);
                        highlighter.Location = new Point(highlighter.Location.X + bWidth, highlighter.Location.Y);
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
            if((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back) && EditMenu.SelectedIndex != -1)
            {
                Component compRemoved = (Component)EditMenu.Items[EditMenu.SelectedIndex];
                if(compRemoved.TileType == TileType.Enemy && compRemoved.RoamPoints != null)
                {
                    foreach(Component c in compRemoved.RoamPoints)
                    {
                        EditMenu.Items.Remove(c);
                        Level.Controls.Remove(c.GetBox());
                        
                    }
                }
                EditMenu.Items.RemoveAt(EditMenu.SelectedIndex);
                Level.Controls.Remove(compRemoved.GetBox());
                if(compRemoved.TileType == TileType.Arrow)
                {
                    Level.Controls.Remove(compRemoved.GetDirectionIndicator());
                }
            }
            if(EditMenu.SelectedIndex == -1)
            {
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
                    case Keys.D6:
                        currentTile = TileType.GlowStick;
                        NewPieceButton.PerformClick();
                        break;
                    case Keys.D7:
                        currentTile = TileType.Arrow;
                        NewPieceButton.PerformClick();
                        break;
                }
            }
            if(EditMenu.SelectedIndex != -1)
            {
                if (((Component)EditMenu.Items[EditMenu.SelectedIndex]).TileType == TileType.RoamPoint)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        EditMenu.SelectedItem = ((Component)EditMenu.SelectedItem).GetParentEnemy();
                        currentTile = TileType.RoamPoint;
                        NewPieceButton.PerformClick();
                    }
                }
                if (((Component)EditMenu.Items[EditMenu.SelectedIndex]).TileType == TileType.Enemy)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        currentTile = TileType.RoamPoint;
                        NewPieceButton.PerformClick();
                    }
                }
            }
        }

        /// <summary>
        /// Writes all data to a string, which will be exported to a text file
        /// ORDER:
        /// |tiletype,x,y,width,height,roampoints|
        /// enemy tiles will contain a collection of child roam nodes, others will just say "empty"
        /// Roam Point Notation:
        /// [roampoint,x,y,index[
        /// </summary>
        /// <returns></returns>
        public string WriteToString()
        {
            String final = "";
            foreach(Component comp in EditMenu.Items)
            {
                string addition = "";
                if (comp.TileType == TileType.RoamPoint)
                {
                    continue;
                }
                if (comp.TileType == TileType.Enemy)
                {
                    addition += comp.FileIOToString();
                    addition += ",";
                    addition += "";
                    for (int i = 0; i < comp.RoamPoints.Count; i ++)
                    {
                        Component rp = comp.RoamPoints[i];
                        addition += rp.FileIOToString();
                        addition += i;
                        addition += "[";
                    }
                }
                else
                {
                    addition += comp.FileIOToString();
                    addition += ",empty";
                }
                addition += "|";
                final += addition;
            }

            return final;
        }

        /// <summary>
        /// Handles the literaly writing of the data
        /// </summary>
        /// <param name="filePath">Path of the file to write to</param>
        public void WriteToFile(string filePath)
        {
            //try
            //{
                string data = WriteToString();
                File.WriteAllText(filePath, data);
            //}
            //catch(Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }

        /// <summary>
        /// Activates when the save button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog;
            dialog = new SaveFileDialog();
            dialog.FileName = "level";
            dialog.Title = "Save File";
            dialog.DefaultExt = "lvl";
            DialogResult result = dialog.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                WriteToFile(dialog.FileName);
            }
        }

        private void ZoomBar_Scroll(object sender, EventArgs e)
        {
            bWidth = ((TrackBar)sender).Value;
            bHeight = ((TrackBar)sender).Value;
            foreach (Component comp in EditMenu.Items)
            {
                comp.ReAdjust(bWidth, bHeight);
            }
        }

        private void VBar_DragDrop(object sender, DragEventArgs e)
        {
            
        }

        private void HBar_DragDrop(object sender, DragEventArgs e)
        {
            
        }

        private void HBar_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (Component comp in EditMenu.Items)
            {
                comp.ChangeScroll(-((HScrollBar)sender).Value, -VBar.Value);
            }
        }

        private void VBar_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (Component comp in EditMenu.Items)
            {
                comp.ChangeScroll(-HBar.Value, -((VScrollBar)sender).Value);
            }
        }

        
    }
}

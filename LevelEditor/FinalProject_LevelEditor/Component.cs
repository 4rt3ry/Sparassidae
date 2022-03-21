/*
 * Component class
 * Represents a placeable component within the level
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject_LevelEditor
{
    class Component 
    {
        //Fields
        private Point location;
        private TileType tileType;
        private List<PictureBox> boxes;

        //Constructors
        public Component(Point location, TileType tileType, int width, int height, int bWidth, int bHeight)
        {
            this.location = location;
            this.tileType = tileType;
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    PictureBox p = new PictureBox();
                    p.Location = new Point(location.X + i, location.Y + j);
                    p.Width = bWidth;
                    p.Height = bHeight;
                    boxes.Add(p);
                }
            }
        }

        public Component(Point location, TileType tileType, List<PictureBox> boxes)
        {

        }

        //Methods
        public List<PictureBox> GetBoxes()
        {
            return boxes;
        }
    }
}

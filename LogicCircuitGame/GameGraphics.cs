using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace LogicCircuitGame
{
    public class GameGraphics
    {
        public static PictureBox Canvas;

        public static void DrawBackground(Graphics g, int Width, int Height)
        {
            if (Game.BlockSize <= 5) return;
            Pen p = new Pen(Game.GridColor);
            for (int x = Game.OffsetX % Game.BlockSize; x < Width; x += Game.BlockSize)
            {
                g.DrawLine(p, x, 0, x, Height);
            }
            for (int y = Game.OffsetY % Game.BlockSize; y < Height; y += Game.BlockSize)
            {
                g.DrawLine(p, 0, y, Width, y);
            }
        }

        public static void DrawWires(Graphics g)
        {
            foreach (Wire w in Game.Wires)
            {
                DrawWire(w, g);
            }
        }

        public static void DrawElements(Graphics g)
        {
            Brush b;
            if (Game.DraggingElement)
            {
                b = new SolidBrush(Game.MoveCollisionColor);
                Element CollisionElement = Game.CollisionElement();
                if (CollisionElement != null)
                {
                    g.FillRectangle(b, CollisionElement.DisplayRectangle());
                }
            }
            if (Game.Routing)
            {
                DrawWire(Game.TempPath, g);
            }
            foreach (Element e in Game.Elements)
            {
                g.DrawImage(e.Symbol(), e.Location.DisplaySize().X + Game.OffsetX, e.Location.DisplaySize().Y + Game.OffsetY);
                b = new SolidBrush(Game.NodeColor);
                foreach (Vector NodePos in e.Inputs.Concat(e.Outputs))
                {
                    Vector Center = (e.Location + NodePos).DisplaySize();
                    g.FillEllipse(b, Center.X - Game.BlockSize * .25f + Game.OffsetX, Center.Y - Game.BlockSize * .25f + Game.OffsetY, Game.BlockSize * .5f, Game.BlockSize * .5f);
                }
                if (Game.Simulating)
                {
                    for (int i = 0; i < e.Inputs.Count; i++)
                    {
                        if (e.InputConnections[i] != null)
                        {
                            b = new SolidBrush(Game.BoolColor(e.InputValue(i)));
                            Vector Center = (e.Location + e.Inputs[i]).DisplaySize();
                            g.FillEllipse(b, Center.X - Game.BlockSize * .25f + Game.OffsetX, Center.Y - Game.BlockSize * .25f + Game.OffsetY, Game.BlockSize * .5f, Game.BlockSize * .5f);
                        }
                    }
                    for (int i = 0; i < e.Outputs.Count; i++)
                    {
                        b = new SolidBrush(Game.BoolColor(e.OutputValues[i]));
                        //MessageBox.Show((e.Location + e.Outputs[i]).X.ToString() + " " + (e.Location + e.Outputs[i]).Y.ToString());
                        Vector Center = (e.Location + e.Outputs[i]).DisplaySize();
                        g.FillEllipse(b, Center.X - Game.BlockSize * .25f + Game.OffsetX, Center.Y - Game.BlockSize * .25f + Game.OffsetY, Game.BlockSize * .5f, Game.BlockSize * .5f);
                    }

                }
            }
            Pen p = new Pen(Game.SelectedColor);
            if (Game.SelectedElement != null)
            {
                g.DrawRectangle(p, Game.SelectedElement.DisplayRectangle());
            }
            b = new SolidBrush(Game.SelectedColor);
            if (Game.SelectedNode != null)
            {
                Vector Location = Game.SelectedNode.DisplayLocation();
                g.FillEllipse(b, Location.X - Game.BlockSize * .25f, Location.Y - Game.BlockSize * .25f, Game.BlockSize * .5f, Game.BlockSize * .5f);
            }
            if (Game.DraggingElement)
            {
                g.DrawRectangle(p, Game.NewLocation.DisplaySize().X + Game.OffsetX, Game.NewLocation.DisplaySize().Y + Game.OffsetY, Game.SelectedElement.Size.DisplaySize().X, Game.SelectedElement.Size.DisplaySize().Y);
            }
        }

        public static void DrawWire(Wire w, Graphics g)
        {
            if (w == null) return;
            Pen p = new Pen(Game.WireColor, 2);
            Point[] points = new Point[w.Nodes.Count];
            for (int i = 0; i < w.Nodes.Count; i++)
            {
                points[i] = w.Nodes[i].DisplayLocation();
            }
            g.DrawLines(p, points);
        }

        public static void Refresh()
        {
            Canvas.Invalidate();
        }

        public static void InvalidateCache()
        {
            foreach (Element e in Game.Elements)
            {
                e.Invalidate();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicCircuitGame
{
    public partial class Form1 : Form
    {
        private int LastMouseX, LastMouseY;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GameGraphics.Canvas = this.pictureBox1;
            this.Resize += Form1_Resize;
            pictureBox1.Paint += PictureBox1_Paint;
            pictureBox1.MouseDown += PictureBox1_MouseDown;
            pictureBox1.MouseMove += PictureBox1_MouseMove;
            pictureBox1.MouseUp += PictureBox1_MouseUp;
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;

            //for test
            for (int i = 0; i < 10; i++) for (int j = 0; j < 10; j++)
                    Game.Elements.Add(new NANDGate(new Vector(i * 5, j * 5)));

            Form1_Resize(sender, e);
        }

        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                Game.BlockSize++;
            else
                Game.BlockSize--;

            if (Game.BlockSize <= 0)
                Game.BlockSize = 1;
            if (Game.BlockSize > 25)
                Game.BlockSize = 25;

            GameGraphics.InvalidateCache();
            GameGraphics.Refresh();
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (Game.DraggingElement)
            {
                if (Game.CollisionElement() == null)
                {
                    Game.SelectedElement.Location = Game.NewLocation;
                }
            }
            else if (Game.Routing)
            {
                Vector CurrentLocation = Vector.FromDisplaySize(e.X - Game.OffsetX + Game.BlockSize / 2, e.Y - Game.OffsetY + Game.BlockSize / 2);
                Game.TempPath = Game.Route(Game.SelectedNode.Location(), CurrentLocation);
                Node CurrentNode = Game.SearchNode(CurrentLocation);
                if (Game.TempPath != null && CurrentNode != null)
                {
                    Game.Wires.Add(Game.TempPath);
                    Game.SelectedNode = CurrentNode;
                    if (Game.Simulating) Game.Simulate();
                }
            }
            Game.DraggingBackground = false;
            Game.DraggingElement = false;
            Game.Routing = false;
            GameGraphics.Refresh();
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Game.DraggingBackground)
                {
                    Game.OffsetX += e.X - LastMouseX;
                    Game.OffsetY += e.Y - LastMouseY;
                    LastMouseX = e.X;
                    LastMouseY = e.Y;
                    GameGraphics.Refresh();
                }
                else if (Game.DraggingElement)
                {
                    Game.NewLocation = Vector.FromDisplaySize(e.X - LastMouseX + Game.SelectedElement.Location.DisplaySize().X, e.Y - LastMouseY + Game.SelectedElement.Location.DisplaySize().Y);
                    GameGraphics.Refresh();
                }
                else if (Game.Routing)
                {
                    Vector CurrentLocation = Vector.FromDisplaySize(e.X - Game.OffsetX + Game.BlockSize / 2, e.Y - Game.OffsetY + Game.BlockSize / 2);
                    //MessageBox.Show(CurrentLocation.X.ToString() + " " + CurrentLocation.Y.ToString());
                    Game.TempPath = Game.Route(Game.SelectedNode.Location(), CurrentLocation);
                    GameGraphics.Refresh();
                }
            }
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Game.SelectedElement = null;
                Game.SelectedNode = null;
                if (Game.SelectNode(e.X - Game.OffsetX, e.Y - Game.OffsetY)) //连线
                {
                    Game.Routing = true;
                }
                else if (Game.SelectElement(e.X - Game.OffsetX, e.Y - Game.OffsetY)) //拖动元素
                {
                    Game.DraggingElement = true;
                }
                else //拖动背景
                {
                    Game.DraggingBackground = true;
                }
                LastMouseX = e.X;
                LastMouseY = e.Y;
            }
            PictureBox1_MouseMove(sender, e);
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            GameGraphics.DrawBackground(e.Graphics, pictureBox1.Width, pictureBox1.Height);
            GameGraphics.DrawWires(e.Graphics);
            GameGraphics.DrawElements(e.Graphics);
        }

        private void 开始模拟ToolStripMenuItem_Click(object sender, EventArgs e0)
        {
            foreach (Element e in Game.Elements)
            {
                e.ClearInputConnections();
                e.ClearOutputValues();
            }
            foreach (Wire w in Game.Wires)
            {
                Node Node1 = Game.SearchNode(w.Nodes.First());
                Node Node2 = Game.SearchNode(w.Nodes.Last());
                if (Node1.Type == NodeType.Output)
                {
                    Node Temp = Node1;
                    Node1 = Node2;
                    Node2 = Temp;
                }
                if (Node1.Type == NodeType.Output || Node2.Type == NodeType.Input)
                {
                    MessageBox.Show("");
                    return;
                }
                if (Node1.Parent.InputConnections[Node1.Index] != null)
                {
                    MessageBox.Show("");
                    return;
                }
                Node1.Parent.InputConnections[Node1.Index] = Node2;
            }
            Game.Simulating = true;
            Game.Simulate();
        }

        private void 停止模拟ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Game.Simulating = false;
            GameGraphics.Refresh();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point(0, menuStrip1.Height);
            pictureBox1.Size = this.Size;
        }
    }
}

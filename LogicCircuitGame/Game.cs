using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuitGame
{
    public class Game
    {
        public static Color GridColor = Color.FromArgb(32, 32, 32);
        public static Color NodeColor = Color.FromArgb(128, 128, 128);
        public static Color SymbolColor = Color.FromArgb(128, 128, 128);
        public static Color SelectedColor = Color.Yellow;
        public static Color MoveCollisionColor = Color.Red;
        public static Color WireColor = Color.White;


        public static int OffsetX = 0;
        public static int OffsetY = 0;
        public static int BlockSize = 15;
        public static List<Element> Elements = new List<Element>();
        public static List<Wire> Wires = new List<Wire>();

        public static Element SelectedElement;
        public static bool DraggingElement = false;
        public static bool DraggingBackground = false;
        public static bool Routing = false;
        public static Vector NewLocation;

        public static Node SelectedNode;
        public static Wire TempPath;

        public static bool Simulating = false;

        public static Color BoolColor(bool b)
        {
            return b ? Color.Red : Color.Blue;
        }

        public static Element CollisionElement()
        {
            foreach (Element e in Elements)
            {
                if (e != SelectedElement)
                {
                    Rectangle SelectedRectangle = new Rectangle(NewLocation.X, NewLocation.Y, SelectedElement.Size.X, SelectedElement.Size.Y);
                    Rectangle eRectangle = new Rectangle(e.Location.X, e.Location.Y, e.Size.X, e.Size.Y);
                    if (SelectedRectangle.IntersectsWith(eRectangle))
                    {
                        return e;
                    }
                }
            }
            return null;
        }

        public static bool SelectElement(int x, int y)
        {
            SelectedElement = SearchElement(Vector.FromDisplaySize(x, y));
            if (SelectedElement != null)
            {
                GameGraphics.Refresh();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Element SearchElement(Vector v)
        {
            foreach (Element e in Elements)
            {
                if (e.Contains(v))
                {
                    return e;
                }
            }
            return null;
        }

        public static bool SelectNode(int x, int y)
        {
            foreach (Element e in Elements)
            {
                for (int i = 0; i < e.Inputs.Count; i++)
                {
                    Vector Center = (e.Location + e.Inputs[i]).DisplaySize();
                    if ((x - Center.X) * (x - Center.X) + (y - Center.Y) * (y - Center.Y) <= (Game.BlockSize * .25f) * (Game.BlockSize * .25f))
                    {
                        SelectedNode = new Node(e, NodeType.Input, i);
                        GameGraphics.Refresh();
                        return true;
                    }
                }
                for (int i = 0; i < e.Outputs.Count; i++)
                {
                    Vector Center = (e.Location + e.Outputs[i]).DisplaySize();
                    if ((x - Center.X) * (x - Center.X) + (y - Center.Y) * (y - Center.Y) <= (Game.BlockSize * .25f) * (Game.BlockSize * .25f))
                    {
                        SelectedNode = new Node(e, NodeType.Output, i);
                        GameGraphics.Refresh();
                        return true;
                    }
                }
            }
            return false;
        }

        public static Node SearchNode(Vector v)
        {
            foreach (Element e in Elements)
            {
                for (int i = 0; i < e.Inputs.Count; i++)
                {
                    if (e.Location + e.Inputs[i] == v)
                    {
                        return new Node(e, NodeType.Input, i);
                    }
                }
                for (int i = 0; i < e.Outputs.Count; i++)
                {
                    if (e.Location + e.Outputs[i] == v)
                    {
                        return new Node(e, NodeType.Output, i);
                    }
                }
            }
            return null;
        }

        public static Wire Route(Vector Start, Vector End)
        {
            if (Start.X == End.X && Start.Y == End.Y) return null;
            int MinX, MinY, MaxX, MaxY;
            MinX = MaxX = End.X;
            MinY = MaxY = End.Y;
            foreach (Element e in Elements)
            {
                if (e.Location.X < MinX) MinX = e.Location.X;
                if (e.Location.Y < MinY) MinY = e.Location.Y;
                if ((e.Location + e.Size).X > MaxX) MaxX = (e.Location + e.Size).X;
                if ((e.Location + e.Size).Y > MaxY) MaxY = (e.Location + e.Size).Y;

            }
            MinX--;
            MinY--;
            MaxX++;
            MaxY++;
            int[,] map = new int[MaxX - MinX + 1, MaxY - MinY + 1];
            foreach (Element e in Elements)
            {
                for (int x = e.Location.X + 1; x < e.Location.X + e.Size.X; x++)
                {
                    for (int y = e.Location.Y + 1; y < e.Location.Y + e.Size.Y; y++)
                    {
                        map[x - MinX, y - MinY] = -1;
                        if (x == End.X && y == End.Y) return null;
                    }
                }
            }
            foreach (Element e in Elements)
            {
                foreach (Vector NodePos in e.Inputs.Concat(e.Outputs))
                {
                    map[(e.Location + NodePos).X - MinX, (e.Location + NodePos).Y - MinY] = -1;
                }
            }
            map[End.X - MinX, End.Y - MinY] = 0;
            Queue<Vector> queue = new Queue<Vector>();
            queue.Enqueue(new Vector(Start.X - MinX, Start.Y - MinY));
            map[Start.X - MinX, Start.Y - MinY] = 1;
            List<Vector> OffsetList = new List<Vector> { new Vector(0, 1), new Vector(1, 0), new Vector(0, -1), new Vector(-1, 0) };
            bool Found = false;
            Vector v;
            while (queue.Count > 0 && !Found)
            {
                v = queue.Dequeue();
                foreach (Vector offset in OffsetList)
                {
                    Vector newv = v + offset;
                    if (newv.X >= 0 && newv.X < MaxX - MinX + 1 && newv.Y >= 0 && newv.Y < MaxY - MinY + 1)
                    {
                        if (map[newv.X, newv.Y] == 0)
                        {
                            map[newv.X, newv.Y] = map[v.X, v.Y] + 1;
                            queue.Enqueue(newv);
                        }
                        if (newv.X + MinX == End.X && newv.Y + MinY == End.Y)
                        {
                            Found = true;
                            break;
                        }
                    }
                }
            }
            if (!Found) return null;
            List<Vector> Path = new List<Vector>();
            v = End;
            Path.Add(v);
            while (v.X != Start.X || v.Y != Start.Y)
            {
                foreach (Vector offset in OffsetList)
                {
                    Vector newv = v + offset;
                    if (newv.X - MinX >= 0 && newv.X - MinX < MaxX - MinX + 1 && newv.Y - MinY >= 0 && newv.Y - MinY < MaxY - MinY + 1)
                    {
                        if (map[newv.X - MinX, newv.Y - MinY] == map[v.X - MinX, v.Y - MinY] - 1)
                        {
                            v = newv;
                            break;
                        }
                    }
                }
                Path.Add(v);
            }
            return new Wire(Path);
        }

        public static void Simulate()
        {
            foreach (Element e in Elements)
            {
                e.Visited = false;
            }
            foreach (Element e in Elements)
            {
                e.Process();
            }
            GameGraphics.Refresh();
        }
    }
}

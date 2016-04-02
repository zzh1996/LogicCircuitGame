using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuitGame
{
    public class Vector
    {
        public Vector(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public static Vector FromDisplaySize(int x, int y)
        {
            return new Vector(x / Game.BlockSize, y / Game.BlockSize);
        }
        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }
        public static Vector operator -(Vector a)
        {
            return new Vector(-a.X, -a.Y);
        }
        public static Vector operator -(Vector a, Vector b)
        {
            return a + (-b);
        }
        public static Vector operator *(Vector a, int b)
        {
            return new Vector(a.X * b, a.Y * b);
        }
        public static bool operator ==(Vector a, Vector b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Vector a, Vector b)
        {
            return !(a == b);
        }
        public Vector DisplaySize()
        {
            return this * Game.BlockSize;
        }
        public static implicit operator Point(Vector a)
        {
            return new Point(a.X, a.Y);
        }
        public Vector DisplayLocation()
        {
            return DisplaySize() + new Vector(Game.OffsetX, Game.OffsetY);
        }
    }

    public enum NodeType { Input, Output };

    public class Node
    {
        public Element Parent { get; set; }
        public NodeType Type { get; set; }
        public int Index { get; set; }

        public Node(Element e, NodeType t, int i)
        {
            Parent = e;
            Type = t;
            Index = i;
        }

        public Vector LocalLocation()
        {
            if (Type == NodeType.Input)
                return new Vector(Parent.Inputs[Index].X, Parent.Inputs[Index].Y);
            else
                return new Vector(Parent.Outputs[Index].X, Parent.Outputs[Index].Y);
        }

        public Vector Location()
        {
            return LocalLocation() + Parent.Location;
        }

        public Vector DisplayLocation()
        {
            return Location().DisplayLocation();
        }

        public bool Value()
        {
            if (Type == NodeType.Output)
            {
                return Parent.OutputValues[Index];
            }
            else {
                throw new ArgumentException();
            }
        }
    }

    public abstract class Element
    {
        public Vector Location { get; set; }
        public abstract Vector Size { get; }
        public abstract List<Vector> Inputs { get; }
        public abstract List<Vector> Outputs { get; }
        protected bool Invalidated = true;
        protected Bitmap SymbolCache;

        public List<Node> InputConnections { get; set; }
        public List<bool> OutputValues { get; set; }
        public bool Visited { get; set; }

        public Element(Vector Location)
        {
            this.Location = Location;
        }

        public void ClearInputConnections()
        {
            InputConnections = new List<Node>();
            for (int i = 0; i < Inputs.Count; i++) InputConnections.Add(null);
        }

        public void ClearOutputValues()
        {
            OutputValues = new List<bool>(Outputs.Count);
            for (int i = 0; i < Outputs.Count; i++) OutputValues.Add(false);
        }

        public Bitmap Symbol()
        {
            if (Invalidated)
            {
                DrawSymbol();
                Invalidated = false;
            }
            return SymbolCache;
        }

        protected abstract void DrawSymbol();

        public void Process()
        {
            if (!Visited)
            {
                Visited = true;
                foreach (Node n in InputConnections)
                {
                    if (n != null)
                    {
                        n.Parent.Process();
                    }
                }
                Logic();
            }
        }

        protected abstract void Logic();

        public bool Contains(Vector v)
        {
            return v.X >= Location.X && v.Y >= Location.Y && v.X < Location.X + Size.X && v.Y < Location.Y + Size.Y;
        }

        public Rectangle DisplayRectangle()
        {
            return new Rectangle(Location.DisplaySize().X + Game.OffsetX, Location.DisplaySize().Y + Game.OffsetY, Size.DisplaySize().X, Size.DisplaySize().Y);
        }

        public void Invalidate()
        {
            Invalidated = true;
        }

        public bool InputValue(int n)
        {
            if (InputConnections[n] == null)
            {
                return false;
            }
            else {
                return InputConnections[n].Value();
            }
        }
    }

    public class NANDGate : Element
    {
        public NANDGate(Vector Location) : base(Location) { }
        public override Vector Size { get { return new Vector(4, 4); } }
        public override List<Vector> Inputs
        {
            get
            {
                return new List<Vector>() {
                    new Vector(0,1),
                    new Vector(0,3)
                };
            }
        }
        public override List<Vector> Outputs
        {
            get
            {
                return new List<Vector>() {
                    new Vector(4,2)
                };
            }
        }
        protected override void DrawSymbol()
        {
            SymbolCache = new Bitmap(Size.DisplaySize().X, Size.DisplaySize().Y);
            Graphics g = Graphics.FromImage(SymbolCache);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen p = new Pen(Game.SymbolColor, 2);
            Brush b = new SolidBrush(Game.SymbolColor);
            g.DrawRectangle(p, Game.BlockSize * 0.75f, Game.BlockSize * 0.5f, Game.BlockSize * 2, Game.BlockSize * 3);
            g.DrawEllipse(p, Game.BlockSize * 2.75f, Game.BlockSize * 1.75f, Game.BlockSize * .5f, Game.BlockSize * .5f);
            g.DrawLine(p, 0, Game.BlockSize, Game.BlockSize * 0.75f, Game.BlockSize);
            g.DrawLine(p, 0, Game.BlockSize * 3, Game.BlockSize * 0.75f, Game.BlockSize * 3);
            g.DrawLine(p, Game.BlockSize * 3.25f, Game.BlockSize * 2, Game.BlockSize * 4, Game.BlockSize * 2);
            g.DrawString("&", new Font("Consolas", Game.BlockSize), b, Game.BlockSize * 1.25f, Game.BlockSize);
        }

        protected override void Logic()
        {
            OutputValues[0] = !(InputValue(0) & InputValue(1));
        }
    }

    public class Wire
    {
        public Wire(List<Vector> l)
        {
            Nodes = l;
        }
        public List<Vector> Nodes { get; set; }
    }
}

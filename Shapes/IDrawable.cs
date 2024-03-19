using System.Numerics;
using rl = Raylib_cs;


namespace Playground.Shapes;

public interface IDrawable
{
    //public Vector2 Position {get;set;}
    public void Draw();
}
using System.Numerics;
using rl = Raylib_cs;

namespace Playground.Shapes;

class Circle2D : IDrawable
{
    public Vector2 Position{get;set;}
    float radius;
    rl.Color color;

    public Circle2D(Vector2 position, float radius)
    {
        this.Position = position;
        this.radius = radius;
        this.color = rl.Color.BLACK;
    }
    public Circle2D(Vector2 position, float radius, rl.Color color) : this (position, radius)
    {
        this.color = color;
    }
    public Circle2D(float positionX, float positionY, float radius)
    {
        this.Position = new Vector2(positionX, positionY);
        this.radius = radius;
        this.color = rl.Color.BLACK;
    }
    public Circle2D(float positionX, float positionY, float radius, rl.Color color) : this(positionX,positionY, radius)
    {
        this.color = color;
    }
    public void Draw()
    {
        //Vector2 cameraWorldPosition = rl.Raylib.GetScreenToWorld2D(Position, camera);
        rl.Raylib.DrawCircle((int)Position.X, (int)Position.Y, radius, color);
        //Console.WriteLine($"x: {Position.X} y:{Position.Y}");
    }
}
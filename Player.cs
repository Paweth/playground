using System.Numerics;
using Playground.Shapes;
using rl = Raylib_cs;


namespace Playground;
public class Player
{
    private Vector2 moveDirection;//to be part of InputState object?
    private float movementSpeed = 0.05f;
    private bool shouldMove;
    public IDrawable shape;
    
    public Player()
    {
        moveDirection = new Vector2(0,0);
        shape = new Circle2D(new Vector2(0,0), 50.0f);
    }
    public Player(IDrawable shape)
    {
        moveDirection = new Vector2(0,0);
        this.shape = shape;
    }
    private void CheckInputKeys()
    {
        shouldMove = false;
        moveDirection.X = 0; moveDirection.Y = 0;
        if(rl.Raylib.IsKeyDown(rl.KeyboardKey.KEY_W)) moveDirection.Y -= 1; shouldMove = true;
        if(rl.Raylib.IsKeyDown(rl.KeyboardKey.KEY_S)) moveDirection.Y += 1; shouldMove = true;
        if(rl.Raylib.IsKeyDown(rl.KeyboardKey.KEY_A)) moveDirection.X -= 1; shouldMove = true;
        if(rl.Raylib.IsKeyDown(rl.KeyboardKey.KEY_D)) moveDirection.X += 1; shouldMove = true;
        moveDirection.X = Math.Clamp(moveDirection.X,-1,1);
        moveDirection.Y = Math.Clamp(moveDirection.Y,-1,1);
        Vector2.Normalize(moveDirection);
        
    }
    public void Move()
    {
        CheckInputKeys();
        //if(!shouldMove) moveDirection.X = 0.0f; moveDirection.Y = 0.0f;
        // this.shape.Position += moveDirection * movementSpeed;
    }
    public void Render()
    {
        shape.Draw();
        //rl.Raylib.DrawCircle((int)Position.X,(int)Position.Y,100,rl.Color.BLACK);
    }
}
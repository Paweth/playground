using rl = Raylib_cs;
using System.Numerics;
using Playground;
using Playground.Shapes;
using Playground.World;
using Playground.Pathfinding;

Random random = new Random();

const int screenWidth = 1200;
const int screenHeight = 900;

rl.Camera2D mainCamera = new rl.Camera2D();

GridSpace2D baseGrid = new GridSpace2D(14);

//var navGrid = new NavGrid(baseGrid, 10, 10);

var navGrid = new NavGrid(baseGrid, 80, 50);
var pathState = new PathState(navGrid);
var start = navGrid.GetRandomWalkableCell();
var target = start;
while(start == target) //set random cell as target
{
    target = navGrid.GetRandomWalkableCell();
}


var tokenSource = new CancellationTokenSource();
var ct = tokenSource.Token;
Task pathfinding = Pathfinder.AStarSearchCancellable(start, target, (node, state) => pathState.StateMap[node] = state, ct);

rl.Raylib.InitWindow(screenWidth, screenHeight, "Hello World");
mainCamera.zoom = 1;

Vector2 textPosition = new Vector2(12,12);
// Player player = new Player(new Circle2D(new Vector2(0,0), 200));

while (!rl.Raylib.WindowShouldClose())
{
    // player.Move();
    if(rl.Raylib.IsMouseButtonPressed(rl.MouseButton.MOUSE_BUTTON_LEFT))
    {
        tokenSource.Cancel();

        navGrid = new NavGrid(baseGrid, 80, 50);
        pathState = new PathState(navGrid);
        start = navGrid.GetRandomWalkableCell();
        target = start;
        while(start == target)
        {
            target = navGrid.GetRandomWalkableCell();
        }
        Console.WriteLine(pathfinding.IsCanceled);
        tokenSource = new CancellationTokenSource();
        ct = tokenSource.Token;
        pathfinding = Pathfinder.AStarSearchCancellable(start, target, (node, state) => pathState.StateMap[node] = state, ct);
    
    }
    //reset pathState
    rl.Raylib.BeginDrawing();
    rl.Raylib.ClearBackground(rl.Color.WHITE);
    rl.Raylib.DrawText("Hello, world!", (int)textPosition.X, (int)textPosition.Y, 20, rl.Color.BLACK);

    rl.Raylib.BeginMode2D(mainCamera);
    
    pathState.Draw();



    rl.Raylib.EndMode2D();

    

    rl.Raylib.EndDrawing();

}

rl.Raylib.CloseWindow();

// for(int i = 0; i < 10; i++)
// {
//     for(int j = 0; j < 10; j++)
//     {
//         Console.WriteLine($"x = {(int)baseGrid[i,j].X}, y = {(int)baseGrid[i,j].Y}");
//     }
// }


using rl = Raylib_cs;

namespace Playground;


public class Texture
{
    rl.Color[,] colorArray = {{new rl.Color(1,1,1,255), new rl.Color(1,1,1,255)},{new rl.Color(100,1,1,255),new rl.Color(1,1,1,255)}};
    
    public void ColorArrayToImage(rl.Color[,] colors)
    {
        rl.Image image = new rl.Image();
        for(int i = 0; i < colors.GetLength(0); i++)
        {
            for(int j = 0; j < colors.GetLength(1); j++)
            {
                //image.data
            }

        }
    }

    public Texture(rl.Color[,] colorMatrix)
    {
        colorArray = colorMatrix;
    }
    public void DrawRandomTexture()
    {
        
    }
}
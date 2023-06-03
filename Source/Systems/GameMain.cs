using Microsoft.Xna.Framework;
using Nez;

namespace EternitySaga;

public class GameMain : Nez.Core
{
    protected override void Initialize()
    {
        base.Initialize();

        // TODO: Add your initialization logic here
        Scene = Scene.CreateWithDefaultRenderer(Color.CornflowerBlue);
    }
}

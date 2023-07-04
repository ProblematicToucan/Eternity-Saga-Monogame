using EternitySaga.Systems;
using Microsoft.Xna.Framework;
using Nez;

namespace EternitySaga.World;

public class Empty : BaseScene
{
    public override void Initialize()
    {
        base.Initialize();

        ClearColor = Color.CornflowerBlue;
        SetDesignResolution(512, 256, SceneResolutionPolicy.ShowAllPixelPerfect);
        Screen.SetSize(512 * 2, 256 * 2);
    }
}
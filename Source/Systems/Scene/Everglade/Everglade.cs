using EternitySaga.Systems;

namespace EternitySaga.World;

public class Everglade : BaseScene
{
    public override void Initialize()
    {
        base.Initialize();

        SetDesignResolution(512, 256, SceneResolutionPolicy.ShowAllPixelPerfect);
        Nez.Screen.SetSize(512 * 2, 256 * 2);
        var player = CreateEntity("player", new(512 / 2, 256 / 2));
        player.AddComponent(new Components.Red());
    }
}
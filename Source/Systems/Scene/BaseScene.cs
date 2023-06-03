using Microsoft.Xna.Framework;
using Nez;

namespace EternitySaga.Systems;

public abstract class BaseScene : Scene
{
    public BaseScene(Color? clearColor = null)
    {
        if (clearColor.HasValue)
            ClearColor = clearColor.Value;
        AddRenderer(new DefaultRenderer());
    }
}
using EternitySaga.World;
using Nez;

namespace EternitySaga;

public class GameMain : Core
{
    protected override void Initialize()
    {
        base.Initialize();

        // TODO: Add your initialization logic here
        IsFixedTimeStep = true;
        Scene = new Everglade();
    }
}

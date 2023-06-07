using EternitySaga.Components;
using EternitySaga.Systems;
using Microsoft.Xna.Framework;
using Nez;

namespace EternitySaga.World;

public class Everglade : BaseScene
{
    public override void Initialize()
    {
        base.Initialize();

        ClearColor = Color.Black;
        SetDesignResolution(512, 256, SceneResolutionPolicy.ShowAllPixelPerfect);
        Screen.SetSize(512 * 2, 256 * 2);
        var map = Content.LoadTiledMap("Content/Tilemap/Tileset/Map/Everglade.tmx");
        var tiled = CreateEntity("tiled-map");
        var objectGroup = map.GetObjectGroup("object");
        var spawn = objectGroup.Objects["spawn"];
        var player = CreateEntity("player", new(spawn.X + 16, spawn.Y + 16));

        Camera.Position = new(map.Width * map.TileWidth / 2, map.Height * map.TileHeight / 2);
        tiled.AddComponent(new TiledMapRenderer(map));
        player.AddComponent(new Red());
    }
}
using Nez;

namespace EternitySaga.Components;

public class Red : Component
{
    public override void OnAddedToEntity()
    {
        var texture = Entity.Scene.Content.LoadTexture("Sprite/Red/Front Movement");
        var sprites = Nez.Textures.Sprite.SpritesFromAtlas(texture, 64, 64);

        Entity.AddComponent(new Nez.Sprites.SpriteRenderer(sprites[0]));
    }
}
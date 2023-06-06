using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace EternitySaga.Components;
/// <summary>EternitySaga Character class base on <see cref="Component"/></summary>
public class Red : Component, IUpdatable
{
    private Utils.Pathfinder _pathfinder;
    private List<Vector2> _path;
    private int _currentWaypoint = 0;
    public override void OnAddedToEntity()
    {
        base.OnAddedToEntity();
        var texture = Entity.Scene.Content.LoadTexture("Sprite/Red/Front Movement");
        var sprites = Sprite.SpritesFromAtlas(texture, 64, 64);
        var animator = Entity.AddComponent(new SpriteAnimator());
        _pathfinder = Entity.AddComponent(new Utils.Pathfinder());
        RegisterAnimation(sprites, animator);
        animator.Play("idle");
    }

    private static void RegisterAnimation(List<Sprite> sprites, SpriteAnimator animator)
    {
        animator.AddAnimation("idle", 10f, sprites[0], sprites[1], sprites[2], sprites[3], sprites[4], sprites[5]);
        animator.AddAnimation("walk-front", 10f, sprites[6], sprites[7], sprites[8], sprites[9], sprites[10], sprites[11]);
    }

    void IUpdatable.Update()
    {
        if (Input.RightMouseButtonPressed)
        {
            var start = Entity.Position;
            var end = Entity.Scene.Camera.MouseToWorldPoint();
            var newPath = _pathfinder.SearchPath(start, end);
            if (newPath != null)
            {
                _currentWaypoint = 0;
                _path = newPath;
            }
        }
        Move();
    }

    void Move()
    {
        if (_path != null && _currentWaypoint < _path.Count)
        {
            var nextWayPoint = _path[_currentWaypoint];
            var direction = nextWayPoint - Entity.Position;
            direction.Normalize();

            var speed = 25;
            Entity.Position += direction * speed * Time.DeltaTime;

            var waypointThreshold = 1f;
            if (Vector2.Distance(Entity.Position, nextWayPoint) < waypointThreshold)
            {
                _currentWaypoint++;
            }
        }
    }
}
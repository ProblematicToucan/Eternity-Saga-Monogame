using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace EternitySaga.Components;
/// <summary>EternitySaga Character class base on
/// <see langword="Nez.Component"/>.
/// <inheritdoc/>
/// </summary>
public class Red : Component, IUpdatable
{
    [Inspectable, Tooltip("Character movement speed")]
    private float _moveSpeed;
    private SpriteAnimator _animator;
    private Utils.Pathfinder _pathfinder;
    private List<Vector2> _path;
    private int _currentWaypoint = 0;
    private Mover _mover;
    private SubpixelVector2 _subpixelV2 = new();
    private Vector2 moveDir = Vector2.Zero;
    private bool _isMoving;

    /// <inheritdoc cref="Red" path="/summary"/>
    public Red() { }

    public override void OnAddedToEntity()
    {
        base.OnAddedToEntity();
        _animator = Entity.AddComponent(new SpriteAnimator());
        RegisterAnimation();
        _moveSpeed = 25;
        _pathfinder = Entity.AddComponent(new Utils.Pathfinder());
        _path = new();
        _mover = Entity.AddComponent(new Mover());
    }

    private void RegisterAnimation()
    {
        var textureFront = Entity.Scene.Content.LoadTexture("Sprite/Red/Front Movement");
        var spritesFront = Sprite.SpritesFromAtlas(textureFront, 64, 64);
        var textureBack = Entity.Scene.Content.LoadTexture("Sprite/Red/Back Movement");
        var spritesBack = Sprite.SpritesFromAtlas(textureBack, 64, 64);
        var textureSide = Entity.Scene.Content.LoadTexture("Sprite/Red/Side Movement");
        var spritesSide = Sprite.SpritesFromAtlas(textureSide, 64, 64);

        _animator.AddAnimation("idle-front", 10f, spritesFront[0], spritesFront[1], spritesFront[2], spritesFront[3], spritesFront[4], spritesFront[5]);
        _animator.AddAnimation("idle-back", 10f, spritesBack[0], spritesBack[1], spritesBack[2], spritesBack[3], spritesBack[4], spritesBack[5]);
        _animator.AddAnimation("idle-side", 10f, spritesSide[0], spritesSide[1], spritesSide[2], spritesSide[3], spritesSide[4], spritesSide[5]);
        _animator.AddAnimation("walk-front", 10f, spritesFront[6], spritesFront[7], spritesFront[8], spritesFront[9], spritesFront[10], spritesFront[11]);
        _animator.AddAnimation("walk-back", 10f, spritesBack[6], spritesBack[7], spritesBack[8], spritesBack[9], spritesBack[10], spritesBack[11]);
        _animator.AddAnimation("walk-side", 10f, spritesSide[6], spritesSide[7], spritesSide[8], spritesSide[9], spritesSide[10], spritesSide[11]);
    }

    void IUpdatable.Update()
    {
        if (Input.RightMouseButtonPressed)
        {
            var start = Entity.Position;
            var end = Entity.Scene.Camera.MouseToWorldPoint();
            var newPath = _pathfinder.SearchPath(start, end);
            if (newPath != null && !newPath.SequenceEqual(_path))
            {
                UpdatePath(newPath);
            }
        }
        var pathCount = _path?.Count ?? 0;
        _isMoving = pathCount > 0 && _currentWaypoint < pathCount;
        if (_isMoving)
        {
            Move();

            var animationName = "idle-front";
            if (moveDir.X != 0)
            {
                animationName = "walk-side";
                _animator.FlipX = moveDir.X < 0;
            }
            else if (moveDir.Y != 0)
            {
                animationName = moveDir.Y < 0 ? "walk-back" : "walk-front";
            }

            SetAnimation(animationName);
        }
        else
        {
            var animationName = "idle-front";
            if (moveDir.X != 0)
            {
                animationName = "idle-side";
                _animator.FlipX = moveDir.X < 0;
            }
            else if (moveDir.Y != 0)
            {
                animationName = moveDir.Y < 0 ? "idle-back" : "idle-front";
            }

            SetAnimation(animationName);
        }
    }

    private void UpdatePath(List<Vector2> newPath)
    {
        if (newPath.Count == 1) return;
        _currentWaypoint = 0;
        _path = newPath;
    }

    private void Move()
    {
        var nextWayPoint = _path[_currentWaypoint];
        moveDir = CalculateDirection(Entity.Position, nextWayPoint);
        var waypointThreshold = 1f;
        var movement = moveDir * _moveSpeed * Time.DeltaTime;

        _mover.CalculateMovement(ref movement, out var _);
        _subpixelV2.Update(ref movement);
        _mover.ApplyMovement(movement);
        if (Vector2.Distance(Entity.Position, nextWayPoint) < waypointThreshold)
        {
            _currentWaypoint++;
        }
    }

    ///<summary>Calculate direction from start point to end point</summary>
    /// <param name="start">Departure point.</param>
    /// <param name="end">Destination point.</param>
    /// <returns>Normalized Vector2</returns>
    private static Vector2 CalculateDirection(Vector2 start, Vector2 end)
    {
        var direction = end - start;
        return direction.Length() < 0.000001f ?
            Vector2.Zero : Vector2.Normalize(end - start);
    }

    private void SetAnimation(string animationName)
    {
        if (animationName != _animator.CurrentAnimationName)
        {
            _animator.Play(animationName);
        }
    }
}
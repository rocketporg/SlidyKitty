using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using SlidyKitty.Code.Shared;

namespace SlidyKitty.Code.Player;

internal class PlayerAnimationSystem : EntityProcessingSystem
{
    private ComponentMapper<CharacterComponent> _characterMapper = default!;
    private ComponentMapper<SpriteComponent> _spriteMapper = default!;

    public PlayerAnimationSystem() : base(Aspect.All(
        typeof(CharacterComponent),
        typeof(PlayerComponent),
        typeof(SpriteComponent)))
    { }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _characterMapper = mapperService.GetMapper<CharacterComponent>();
        _spriteMapper = mapperService.GetMapper<SpriteComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        // Get references for our components
        var characterComponent = _characterMapper.Get(entityId);
        var spriteComponent = _spriteMapper.Get(entityId);

        // If for some reason we don't have an animated player
        // sprite, we can't do anything, so we just return
        if (spriteComponent.Sprite is not AnimatedSprite sprite)
            return;

        // Otherwise, change animations based on whether the player
        // is in the 'swift' pose or not
        if (characterComponent.IsInSwiftPose)
            sprite.SetAnimation(nameof(PlayerAnimationState.SwiftPose));
        else
            sprite.SetAnimation(nameof(PlayerAnimationState.IdlePose));

        // When the player presses space we want to go into 'swift' pose, to make
        // the player character fall faster and slide quicker ;-)
        characterComponent.IsInSwiftPose = Keyboard.GetState().IsKeyDown(Keys.Space);
    }
}

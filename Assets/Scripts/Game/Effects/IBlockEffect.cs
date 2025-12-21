using Game.Action.Internal;

namespace Game.Effects
{
    public interface IBlockEffect
    {
        void OnCallBlocked(Block applyEffect);
    }
}
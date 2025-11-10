using Game.Tile;

namespace Game.Effects
{
    public interface IImmunity
    {
        bool CheckImmunity(FormationType formationType, Effect effect);
    }
}
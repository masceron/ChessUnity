using Game.Managers;
using Game.Piece.PieceLogic;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Stunned: Effect
    {
        public Stunned(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectName.Stunned)
        {}

        public override string Description()
        {
            return string.Format(AssetManager.Ins.EffectData[EffectName].description, Duration);
        }
    }
}
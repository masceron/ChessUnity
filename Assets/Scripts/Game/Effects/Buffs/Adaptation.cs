using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Adaptation: Effect
    {
        public Adaptation(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectName.Adaptation)
        {}

    }
}
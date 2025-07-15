using Game.Board.General;

namespace Game.Board.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Surpass: Effect
    {
        public Surpass(sbyte duration, PieceLogic.PieceLogic piece) : base(duration, 1, piece, EffectType.Surpass)
        {}
    }
}
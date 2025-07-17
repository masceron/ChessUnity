namespace Game.Board.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Slow: Effect
    {
        public Slow(sbyte duration, sbyte strength, PieceLogic.PieceLogic piece) : base(duration, strength, piece, EffectType.Slow)
        {}

        public override void OnApply()
        {
            Piece.moveRange -= Strength;
        }

        public override void OnRemove()
        {
            Piece.moveRange += Strength;
        }
    }
}
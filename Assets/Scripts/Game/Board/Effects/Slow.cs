using Game.Board.General;

namespace Game.Board.Effects
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Slow: Effect
    {
        public Slow(sbyte duration, sbyte strength, PieceLogic.PieceLogic piece) : base(duration, strength, piece, ObserverType.None, 0)
        {}

        public override void OnApply()
        {
            Piece.MoveRange -= Strength;
        }

        public override void OnRemove()
        {
            Piece.MoveRange += Strength;
        }
    }
}
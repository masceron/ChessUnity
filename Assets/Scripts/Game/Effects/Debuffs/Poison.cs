using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Poison: Effect, IEndTurnEffect
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public byte TimeLeft = 3;

        public Poison(sbyte strength, PieceLogic piece) : base(-1, strength, piece, EffectName.Poison)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public void OnCallEnd(Action.Action action)
        {
            if (Strength >= 5) TimeLeft--;
            if (TimeLeft <= 0)
            {
                ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
            }
        }

        public EndTurnEffectType EndTurnEffectType { get; set; }
    }
}
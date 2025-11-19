using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Bleeding: Effect, IEndTurnEffect
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public byte TurnLeftToDie;
        // ReSharper disable once MemberCanBePrivate.Global
        public byte TurnSinceLastMove;

        private const int TurnToRemoveEffect = 5;

        public Bleeding(int turnToDie, PieceLogic piece) : base(-1, 1, piece, "effect_bleeding")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
            TurnLeftToDie = (byte)turnToDie;
        }

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (lastMainAction is not IQuiets) return; 
            if (lastMainAction.Maker != Piece.Pos)
            {
                TurnSinceLastMove++;
                if (TurnSinceLastMove <= TurnToRemoveEffect) return;
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
            else
            {
                TurnSinceLastMove = 0;
                TurnLeftToDie--;
                if (TurnLeftToDie == 0) ActionManager.EnqueueAction(new KillPiece(lastMainAction.Maker));
            }

        }
    }
}
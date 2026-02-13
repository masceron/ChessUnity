using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Bleeding : Effect, IEndTurnTrigger
    {
        private const int TurnToRemoveEffect = 5;

        // ReSharper disable once MemberCanBePrivate.Global
        public byte TurnLeftToDie;

        // ReSharper disable once MemberCanBePrivate.Global
        public byte TurnSinceLastMove;

        public Bleeding(int turnToDie, PieceLogic piece) : base(-1, 1, piece, "effect_bleeding")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
            TurnLeftToDie = (byte)turnToDie;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Kill;

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

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20 - TurnLeftToDie * 10;
        }
    }
}
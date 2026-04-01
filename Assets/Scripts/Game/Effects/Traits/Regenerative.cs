using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Regenerative : Effect, IEndTurnTrigger
    {
        private int currentTurn;
        private const int TurnToRemoveEffect = 2;
        public Regenerative(PieceLogic piece) : base(-1, -1, piece, "effect_regenerative")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
            currentTurn = 0;
        }
        public void OnCallEnd(Action.Action action)
        {
            if (action.GetMakerAsPiece() == Piece) return;
            var piece = PieceOn(Piece.Pos);

            Effect bleeding = null;
            if (piece.Effects.Any(effects => effects.EffectName == "effect_bleeding"))
            {
                bleeding = piece.Effects.First(e => e.EffectName == "effect_bleeding");
            }
            if (bleeding == null) return;

            currentTurn++;
            if (currentTurn < TurnToRemoveEffect) return;
            ActionManager.EnqueueAction(new RemoveEffect(bleeding));
            currentTurn = 0;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Other;

        public EndTurnEffectType EndTurnEffectType { get; set; }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 40;
        }
    }
}
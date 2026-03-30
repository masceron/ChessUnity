using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaTurtleCountdown : Effect, IEndTurnTrigger, IOnRemoveTrigger
    {
        public SeaTurtleCountdown(int duration, PieceLogic piece) : base(duration, 1, piece,
            "effect_sea_turtle_countdown")
        {
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Buff;

        public EndTurnEffectType EndTurnEffectType => EndTurnEffectType.EndOfEnemyTurn;

        public void OnCallEnd(Action.Action action)
        {
            if (action.GetMakerAsPiece() != Piece) ActionManager.EnqueueAction(new RemoveEffect(this));
        }

        public void OnRemove()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Carapace(1, Piece)));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 0;
        }
    }
}
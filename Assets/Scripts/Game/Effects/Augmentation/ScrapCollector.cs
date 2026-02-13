using Game.Action;
using Game.Action.Internal;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ScrapCollector : Effect, IEndTurnTrigger
    {
        private EndTurnTriggerPriority _priority;

        // ReSharper disable once MemberCanBePrivate.Global
        public byte TimeLeft = 3;

        public ScrapCollector(PieceLogic piece) : base(-1, 1, piece, "effect_scrap_collector")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            ActionManager.EnqueueAction(new Purge(Piece.Pos, Piece.Pos));
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Buff;

        public EndTurnEffectType EndTurnEffectType { get; set; }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 80 - Strength * 10;
        }
    }
}
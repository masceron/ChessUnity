using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;


namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaTurtleCountdown : Effect, IEndTurnTrigger, IOnRemoveTrigger
    {
        private readonly int _pos;

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Buff;

        public EndTurnEffectType EndTurnEffectType => EndTurnEffectType.EndOfEnemyTurn;
        public SeaTurtleCountdown(int duration, PieceLogic piece) : base(duration, 1, piece, "effect_sea_turtle_countdown")
        { _pos = piece.Pos; }
        public void OnCallEnd(Action.Action action)
        {
            if (action.Maker != _pos) ActionManager.EnqueueAction(new RemoveEffect(this));
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
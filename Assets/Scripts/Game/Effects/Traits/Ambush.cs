using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Ambush : Effect, IEndTurnTrigger, IAttackRangeModifier
    {
        private const byte RangeOffset = 2;
        private const byte TurnsToActive = 3;
        private bool _active;
        private byte _lastUsed;

        public Ambush(PieceLogic piece) : base(-1, -1, piece, "effect_ambush")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public int ModifyAttackRange(int baseRange)
        {
            if (_active) return baseRange + RangeOffset;

            return baseRange;
        }

        public void OnCallEnd(Action.Action action)
        {
            if (action.Maker != Piece.Pos)
            {
                _lastUsed++;
                if (_lastUsed < TurnsToActive || _active) return;
                _active = true;
            }
            else if (_active)
            {
                _active = false;
                _lastUsed = 0;
            }
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Buff;

        public EndTurnEffectType EndTurnEffectType { get; set; }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 30;
        }
    }
}
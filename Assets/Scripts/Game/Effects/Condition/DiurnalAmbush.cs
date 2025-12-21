using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Condition
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DiurnalAmbush: Effect, IEndTurnEffect, IAttackRangeModifier
    {
        private byte lastUsed;
        private bool active;
        private const byte RangeOffset = 2;

        public DiurnalAmbush(PieceLogic piece) : base(-1, -1, piece, "effect_diurnal_ambush")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public void OnCallEnd(Action.Action action)
        {
            if (!MatchManager.Ins.GameState.IsDay) return;
            if (action.Maker != Piece.Pos)
            {
                lastUsed++;
                if (lastUsed < 6 || active) return;
                active = true;
            }
            else if (active)
            {
                active = false;
                lastUsed = 0;
            }
        }

        public EndTurnEffectType EndTurnEffectType { get; set; }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 20;    
        }

        public int ModifyAttackRange(int baseRange)
        {
            if (active) return baseRange + RangeOffset;

            return baseRange;
        }
    }
}
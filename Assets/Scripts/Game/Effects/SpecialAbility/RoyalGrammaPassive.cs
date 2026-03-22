using Game.Effects.Traits;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class RoyalGrammaPassive : Effect, IMoveRangeModifierTrigger, IAttackRangeModifier, IOnPieceSpawnedTrigger, IDeadTrigger, IOnApplyTrigger
    {
        public int rangeBonus = 0;
        public RoyalGrammaPassive(PieceLogic piece) : base(-1, 1, piece, "effect_royal_gramma_passive")
        {
            
        }
        public void OnApply()
        {
            RefreshDynamicBonuses();
        }

        public void OnPieceSpawn(PieceLogic piece)
        {
            RefreshDynamicBonuses();
        }
        public void OnCallDead(PieceLogic pieceToDie){
            RefreshDynamicBonuses();
        }

        public int ModifyMoveRange(int baseRange)
        {
            return baseRange + rangeBonus;
        }

        public int ModifyAttackRange(int baseRange)
        {
            return baseRange + rangeBonus;
        }

        private void RefreshDynamicBonuses()
        {
            if (Piece == null || !IsAlive(Piece)) return;

            var allies = FindAllies(Piece.Color);
            int blackcapCount = allies.Count(p => p is BlackcapBasslet);
            int yellowlinedCount = allies.Count(p => p is YellowlinedBasslet);
            int goldenCount = allies.Count(p => p is GoldenBasslet);

            rangeBonus = yellowlinedCount / 2;
            
            Evasion evasion = Piece.Effects.First(e => e is Evasion) as Evasion;
            evasion.Strength = 5 + 5 * blackcapCount;

            Relentless relentless = Piece.Effects.Find(e => e is Relentless) as Relentless;
            relentless.SetStat(EffectStat.Stack, 1 + goldenCount/2);
        }
    }
}
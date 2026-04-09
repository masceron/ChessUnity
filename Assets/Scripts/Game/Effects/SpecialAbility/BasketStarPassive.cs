using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class BasketStarPassive : Effect, IAfterPieceActionTrigger, IStartTurnTrigger
    {
        private bool isPreviousTurnNight = false;
        public BasketStarPassive(PieceLogic piece) : base(-1, 1, piece, "effect_basket_star_passive")
        {
            SetStat(EffectStat.Radius, 2);
            SetStat(EffectStat.Duration, 1, 1);
            SetStat(EffectStat.Duration, 10, 2);
        }
        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Debuff;
        StartTurnTriggerPriority IStartTurnTrigger.Priority => StartTurnTriggerPriority.Buff;
        public StartTurnEffectType StartTurnEffectType => StartTurnEffectType.StartOfAnyTurn;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            //Làm lại
        }
        public void OnCallStart(Action.Action lastMainAction)
        {
            if (isPreviousTurnNight && IsDay())
            {
                ActionManager.EnqueueAction(new ApplyEffect(new HardenedShield(Piece, 1, GetStat(EffectStat.Duration, 2))));
            }
            isPreviousTurnNight = IsDay() == false;
        }
    }


}
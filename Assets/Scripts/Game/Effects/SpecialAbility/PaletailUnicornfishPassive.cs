using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class PaletailUnicornfishPassive : Effect, IAfterPieceActionEffect, IApplyEffect
    {
        public PaletailUnicornfishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_paletail_unicornfish_passive")
        {
            SetStat(EffectStat.Duration, 3);
        }
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect is Blinded)
            {
                applyEffect.Result = ResultFlag.Incorruptible;
            }
        }
        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is ICaptures && action.Maker == Piece.Pos && (action.Result == ResultFlag.Blocked || action.Result == ResultFlag.Miss))
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Blinded((sbyte)GetStat(EffectStat.Duration), 50, PieceOn(action.Target)), Piece));
            }
        }
    }
}
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Augmentation
{
    public class RaySTailPassive : Effect, IAfterPieceActionTrigger
    {
        public RaySTailPassive(PieceLogic piece) : base(-1, 1, piece, "effect_ray's_tail_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Maker != Piece.Pos) return;
            var targetIndex = action.Target;
            var targetPiece = PieceOn(targetIndex);
            var hasShield = false;
            var hasHardenedShield = false;
            var hasCarapace = false;
            Effect shield = null;
            foreach (var t in targetPiece.Effects)
            {
                if (t.EffectName == "effect_carapace") hasCarapace = true;

                if (t.EffectName == "effect_hardened_shield")
                {
                    hasHardenedShield = true;
                    shield = t;
                    break;
                }

                if (t.EffectName == "effect_shield")
                {
                    hasShield = true;
                    shield = t;
                }
            }

            if (!hasCarapace)
                if (hasHardenedShield || hasShield)
                    if (shield.Strength == 1)
                        ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, targetPiece)));
        }
    }
}
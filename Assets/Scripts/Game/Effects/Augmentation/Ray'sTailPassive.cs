using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;
namespace Game.Effects.Augmentation
{
    public class RaySTailPassive : Effect
    {
        public RaySTailPassive(PieceLogic piece) : base(-1, 1, piece, EffectName.RayTailPassive)
        {
        }

        public override void OnCallPieceAction(Action.Action action)
        {
            var targetIndex = action.Target;
            var targetPiece = PieceOn(targetIndex);
            bool hasShield = false;
            bool hasHardenedShield = false;
            bool hasCarapace = false;
            Effect shield = null;
            for (int i = 0; i < targetPiece.Effects.Count; i++)
            {
                if (targetPiece.Effects[i].EffectName == EffectName.Carapace)
                {
                    hasCarapace = true;
                }

                if (targetPiece.Effects[i].EffectName == EffectName.HardenedShield)
                {
                    hasHardenedShield = true;
                    shield = targetPiece.Effects[i];
                    break;
                }

                if (targetPiece.Effects[i].EffectName == EffectName.Shield)
                {
                    hasShield = true;
                    shield = targetPiece.Effects[i];
                }
            }

            if (!hasCarapace)
            {
                if (hasHardenedShield || hasShield)
                {
                    if (shield.Strength == 1)
                    {
                        ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, targetPiece)));
                    }
                }
            }
        }
    }
}
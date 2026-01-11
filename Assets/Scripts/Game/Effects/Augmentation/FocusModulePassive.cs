using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;
namespace Game.Effects.Augmentation
{
    public class FocusModulePassive : Effect, IApplyEffect
    {
        public FocusModulePassive(PieceLogic piece) : base(-1, 1, piece, "effect_focus_module_passive")
        {
        }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Maker != Piece.Pos) return;
            if (PieceOn(applyEffect.Maker).SkillCooldown > 1)
            {
                PieceOn(applyEffect.Maker).SkillCooldown -= 2;
            } else if (PieceOn(applyEffect.Maker).SkillCooldown == 1)
            {
                PieceOn(applyEffect.Maker).SkillCooldown -= 1;
            }
        }
    }
}
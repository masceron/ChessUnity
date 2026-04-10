using System;
using Game.Action.Internal;
using Game.AI;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UX.UI.Ingame;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class BarnacleActive : Action, ISkills, IAIAction
    {
        [MemoryPackConstructor]
        private BarnacleActive()
        {
        }

        public BarnacleActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        public void CompleteActionForAI()
        {
            // var allPieces = PieceBoard();
            // var listPieces = allPieces.Where(p => p != null && p.Color != GetMakerAsPiece().Color &&
            //                                       p.Effects.Any(e =>
            //                                           e.EffectName is "effect_shield" or "effect_hardened_shield")).ToList();
            //
            // if (listPieces.Count == 0) return;
            // var maxValue = listPieces.Max(p => p.GetValueForAI());
            // var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
            // if (bestPieces.Count == 0) return;
            // var random = new Random();
            // var selectedPiece = bestPieces[random.Next(bestPieces.Count)];
            //
            // SetCooldown(GetMakerAsPiece(), -1);
            // foreach (var effect in selectedPiece.Effects
            //              .Where(effect => effect.EffectName is "effect_shield" or "effect_hardened_shield"))
            //     if (effect.Duration > 0)
            //         effect.Duration -= 1;
            //     else
            //         BoardViewer.Ins.ExecuteAction(new RemoveEffect(effect));
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            foreach (var effect in GetTargetAsPiece().Effects
                         .Where(effect => effect.EffectName is "effect_shield" or "effect_hardened_shield"))
                if (effect.Duration > 0)
                    effect.Duration -= 1;
                else
                    ActionManager.EnqueueAction(new RemoveEffect(effect));

            SetCooldown(GetMakerAsPiece(), -1);
            //ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}
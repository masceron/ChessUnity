using System;
using System.Collections.Generic;
using Game.Action.Internal;
using Game.AI;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UX.UI.Ingame;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class HatchetfishActive : Action, ISkills, IAIAction
    {
        [MemoryPackConstructor]
        private HatchetfishActive()
        {
        }

        public HatchetfishActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        public void CompleteActionForAI()
        {
            var listPieces = new List<PieceLogic>();
            var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(GetMakerAsPiece(), 4);
            foreach (var target in targets)
                if (GetTargetAsPiece().Effects.Any(e => e.EffectName == "effect_camouflage") &&
                    !GetTargetAsPiece().Effects.Any(e => e.EffectName is "effect_blinded" or "effect_extremophile"))
                    listPieces.Add(target);


            if (listPieces.Count == 0)
            {
                var targets1 = SkillRangeHelper.GetActiveEnemyPieceInRadius(GetMakerAsPiece(), 2);
                foreach (var target in targets1)
                    if (GetTargetAsPiece().Effects.Any(e =>
                            e.EffectName == "effect_marked" || e.EffectName == "effect_extremophile"))
                        listPieces.Add(target);
            }

            if (listPieces.Count == 0) return;
            var maxValue = listPieces.Max(p => p.GetValueForAI());
            var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
            if (bestPieces.Count == 0) return;
            var random = new Random();
            var selectedPiece = bestPieces[random.Next(bestPieces.Count)];


            //ActionManager.EnqueueAction(new ApplyEffect(new Marked(-1, selectedPiece)));
            BoardViewer.Ins.ExecuteAction(new ApplyEffect(new Marked(-1, selectedPiece), GetMakerAsPiece()));

            if (selectedPiece.Effects.Any(e => e.EffectName == "effect_camouflage"))
                //ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, selectedPiece)));
                BoardViewer.Ins.ExecuteAction(new ApplyEffect(new Blinded(2, 100, selectedPiece), GetMakerAsPiece()));

            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            //Apply effect Marked no duration
            ActionManager.EnqueueAction(new ApplyEffect(new Marked(-1, GetTargetAsPiece()), GetMakerAsPiece()));

            var targetPiece = GetTargetAsPiece();
            if (targetPiece == null) return;

            if (targetPiece.Effects.Any(e => e.EffectName == "effect_camouflage"))
                ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, targetPiece), GetMakerAsPiece()));

            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}
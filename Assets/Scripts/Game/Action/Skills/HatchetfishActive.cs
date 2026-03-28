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

        public HatchetfishActive(int maker, int target) : base(maker, target)
        {
        }

        public void CompleteActionForAI()
        {
            var listPieces = new List<PieceLogic>();
            var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(GetFrom(), 4);
            foreach (var target in targets)
                if (GetTarget().Effects.Any(e => e.EffectName == "effect_camouflage") &&
                    !GetTarget().Effects.Any(e => e.EffectName is "effect_blinded" or "effect_extremophile"))
                    listPieces.Add(PieceOn(target));


            if (listPieces.Count == 0)
            {
                var targets1 = SkillRangeHelper.GetActiveEnemyPieceInRadius(GetFrom(), 2);
                foreach (var target in targets1)
                    if (GetTarget().Effects.Any(e =>
                            e.EffectName == "effect_marked" || e.EffectName == "effect_extremophile"))
                        listPieces.Add(PieceOn(target));
            }

            if (listPieces.Count == 0) return;
            var maxValue = listPieces.Max(p => p.GetValueForAI());
            var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
            if (bestPieces.Count == 0) return;
            var random = new Random();
            var selectedPiece = bestPieces[random.Next(bestPieces.Count)];


            //ActionManager.EnqueueAction(new ApplyEffect(new Marked(-1, selectedPiece)));
            BoardViewer.Ins.ExecuteAction(new ApplyEffect(new Marked(-1, selectedPiece), GetMaker()));

            if (selectedPiece.Effects.Any(e => e.EffectName == "effect_camouflage"))
                //ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, selectedPiece)));
                BoardViewer.Ins.ExecuteAction(new ApplyEffect(new Blinded(2, 100, selectedPiece), GetMaker()));

            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            //Apply effect Marked no duration
            ActionManager.EnqueueAction(new ApplyEffect(new Marked(-1, GetTarget()), GetMaker()));

            var targetPiece = GetTarget();
            if (targetPiece == null) return;

            if (targetPiece.Effects.Any(e => e.EffectName == "effect_camouflage"))
                ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, targetPiece), GetMaker()));

            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}
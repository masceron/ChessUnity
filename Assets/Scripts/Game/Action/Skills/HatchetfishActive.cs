using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.AI;
using System.Collections.Generic;
using static Game.Common.BoardUtils;
using UX.UI.Ingame;
using ZLinq;

namespace Game.Action.Skills
{
    public class HatchetfishActive : Action, ISkills, IAIAction
    {
        public HatchetfishActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target =  target;
        }

        protected override void ModifyGameState()
        {
            //Apply effect Marked no duration
            ActionManager.EnqueueAction(new ApplyEffect(new Marked(-1, PieceOn(Target)), PieceOn(Maker)));

            var targetPiece = PieceOn(Target);
            if (targetPiece == null) return;

            if (targetPiece.Effects.Any(e => e.EffectName == "effect_camouflage"))
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, targetPiece), PieceOn(Maker)));
            }

            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            var listPieces = new List<PieceLogic>();
            var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(Maker, 4);
            foreach (var target in targets)
            {
                if (PieceOn(target).Effects.Any(e => e.EffectName == "effect_camouflage") &&
                !PieceOn(target).Effects.Any(e => e.EffectName == "effect_blinded" || e.EffectName == "effect_extremophile"))
                {
                    listPieces.Add(PieceOn(target));
                }
            }


            if (listPieces.Count == 0) 
            {
                var targets1 = SkillRangeHelper.GetActiveEnemyPieceInRadius(Maker, 2);
                foreach (var target in targets1)
                {
                    if (PieceOn(target).Effects.Any(e => e.EffectName == "effect_marked" || e.EffectName == "effect_extremophile"))
                    {
                        listPieces.Add(PieceOn(target));
                    }
                }
            }

            if (listPieces.Count == 0) return;
            var maxValue = listPieces.Max(p => p.GetValueForAI());
            var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
            if (bestPieces.Count == 0) return;
            var random = new System.Random();
            var selectedPiece = bestPieces[random.Next(bestPieces.Count)];




            //ActionManager.EnqueueAction(new ApplyEffect(new Marked(-1, selectedPiece)));
            BoardViewer.Ins.ExecuteAction(new ApplyEffect(new Marked(-1, selectedPiece), PieceOn(Maker)));

            if (selectedPiece.Effects.Any(e => e.EffectName == "effect_camouflage"))
            {
                //ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, selectedPiece)));
                BoardViewer.Ins.ExecuteAction(new ApplyEffect(new Blinded(2, 100, selectedPiece), PieceOn(Maker)));
            }

            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}

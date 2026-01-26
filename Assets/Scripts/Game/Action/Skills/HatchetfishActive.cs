using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using System.Linq;
using Game.Piece.PieceLogic.Commons;
using Game.AI;
using System.Collections.Generic;
using static Game.Common.BoardUtils;
using UX.UI.Ingame;
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
            
            foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Maker), FileOf(Maker), 4))
            {
                var idx = IndexOf(rank, file);
                var pOn = PieceOn(idx);
                if (pOn != null && pOn.Color != PieceOn(Maker).Color)
                {
                    if(pOn.Effects != null && pOn.Effects.Any(e => e.EffectName == "effect_camouflage") &&
                    !pOn.Effects.Any(e => e.EffectName == "effect_blinded" || e.EffectName == "effect_extremophile")) 
                    {
                        listPieces.Add(pOn);
                    }
        
                }
            }
            if (listPieces.Count == 0) 
            {
                foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Maker), FileOf(Maker), 2))
                {
                    var idx = IndexOf(rank, file);
                    var pOn = PieceOn(idx);
                    if (pOn != null && pOn.Color != PieceOn(Maker).Color)
                    {
                        if(pOn.Effects != null &&
                        !pOn.Effects.Any(e => e.EffectName == "effect_marked" || e.EffectName == "effect_extremophile")) 
                        {
                            listPieces.Add(pOn);
                        }
                    }
                }
            }

            if (listPieces.Count == 0) return;
            int maxValue = listPieces.Max(p => p.GetValueForAI());
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

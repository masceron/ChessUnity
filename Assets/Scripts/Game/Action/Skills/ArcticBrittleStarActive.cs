using static Game.Common.BoardUtils;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using UnityEngine;
using Game.AI;
using System.Collections.Generic;
using System.Linq;
using Game.Common;

namespace Game.Action.Skills
{
    public class ArcticBrittleStarActive : Action, ISkills, IAIAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -5;
            return 0;
        }

        private Tile.Tile hoveringTile;
        public ArcticBrittleStarActive(int maker, int to) : base(maker)
        {
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            Debug.Log("Execute Arctic Brittle Star");
            Formation AnchorIce = new AnchorIce(PieceOn(Maker).Color);
            AnchorIce.SetDuration(3);
            FormationManager.Ins.SetFormation(Target, AnchorIce);
            
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            var listPieces = new List<PieceLogic>();
            
            var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(Maker, 3);
            foreach (var target in targets)
            {
                if (PieceOn(target).Effects.Any(e => e.EffectName == "effect_extremophile")) continue;
                listPieces.Add(PieceOn(target));
            }

            if (listPieces.Count == 0) return;
            int maxValue = listPieces.Max(p => p.GetValueForAI());
            var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
            if (bestPieces.Count == 0) return;

            var random = new System.Random();
            var selectedPiece = bestPieces[random.Next(bestPieces.Count)];
            Formation AnchorIce = new AnchorIce(PieceOn(Maker).Color);
            AnchorIce.SetDuration(3);
            FormationManager.Ins.SetFormation(selectedPiece.Pos, AnchorIce);

            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}
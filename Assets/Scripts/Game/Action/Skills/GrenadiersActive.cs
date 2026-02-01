using Game.Common;
using System.Linq;
using Game.Piece.PieceLogic.Commons;
using Game.AI;
using System.Collections.Generic;
using Game.Managers;
using static Game.Common.BoardUtils;
using Game.Tile;
using ZLinq;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GrenadiersActive: Action, ISkills, IAIAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            return pieceAI.Color != maker.Color ? -60 : 0;
        }

        public GrenadiersActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void ModifyGameState()
        {
            FormationManager.Ins.SetFormation(Target, new NavalMines(true, PieceOn(Maker).Color));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            var listPieces = new List<PieceLogic>();
            var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(Maker, 3);
            foreach (var target in targets)
            {
                listPieces.Add(PieceOn(target));
            }

            if (listPieces.Count == 0) return;
            int maxValue = listPieces.Max(p => p.GetValueForAI());
            var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
            if (bestPieces.Count == 0) return;
            var random = new System.Random();
            var selectedPiece = bestPieces[random.Next(bestPieces.Count)];
            
            FormationManager.Ins.SetFormation(selectedPiece.Pos, new NavalMines(true, selectedPiece.Color));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}
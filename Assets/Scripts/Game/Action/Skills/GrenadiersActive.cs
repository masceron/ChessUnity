using System;
using System.Collections.Generic;
using Game.AI;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using MemoryPack;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class GrenadiersActive : Action, ISkills, IAIAction
    {
        [MemoryPackConstructor]
        private GrenadiersActive()
        {
        }

        public GrenadiersActive(PieceLogic maker, int target) : base(maker, target)
        {
        }

        public void CompleteActionForAI()
        {
            var listPieces = new List<PieceLogic>();
            var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(GetFrom(), 3);
            foreach (var target in targets) listPieces.Add(PieceOn(target));

            if (listPieces.Count == 0) return;
            var maxValue = listPieces.Max(p => p.GetValueForAI());
            var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
            if (bestPieces.Count == 0) return;
            var random = new Random();
            var selectedPiece = bestPieces[random.Next(bestPieces.Count)];

            SetFormation(selectedPiece.Pos, new NavalMines(true, selectedPiece.Color));
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMakerAsPiece();
            if (maker == null || pieceAI == null) return 0;
            return pieceAI.Color != maker.Color ? -60 : 0;
        }

        protected override void ModifyGameState()
        {
            SetFormation(GetTargetPos(), new NavalMines(true, GetMakerAsPiece().Color));
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}
using System;
using System.Collections.Generic;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SiphonophorePending : PendingAction, IDisposable, ISkills
    {
        private static List<int> selectedTiles = new();
        private static int unitCount;
        private static bool isInitialized;

        public SiphonophorePending(int maker, int target) : base(maker, target, TargetingType.LocationTargeting)
        {
            var makerPiece = GetMaker();
            if (makerPiece == null || isInitialized) return;
            
            unitCount = makerPiece.GetStat(SkillStat.Unit);
            selectedTiles.Clear();
            isInitialized = true;
        }

        public void Dispose()
        {
            Reset();
            BoardViewer.SelectingFunction = 0;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void CompleteAction()
        {
            var makerPiece = GetMaker();
            if (makerPiece == null) return;


            if (selectedTiles.Count < unitCount)
            {
                selectedTiles.Add(GetTargetPos());
                TileManager.Ins.UnMark(GetTargetPos());
            }

            if (selectedTiles.Count == unitCount)
            {
                CommitResult(new SiphonophoreActive(GetFrom(), new List<int>(selectedTiles)));
                Reset();
            }
        }

        private static void Reset()
        {
            selectedTiles.Clear();
            unitCount = 0;
            isInitialized = false;
        }
    }
}

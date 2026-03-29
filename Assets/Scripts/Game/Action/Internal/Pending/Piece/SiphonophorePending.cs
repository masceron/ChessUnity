using System;
using System.Collections.Generic;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SiphonophorePending : PendingAction, IDisposable, ISkills
    {
        private readonly List<int> selectedTiles = new();
        private int _unitCount;
        private bool _isInitialized;

        public SiphonophorePending(PieceLogic maker, int target) : base(maker, target)
        {
            if (_isInitialized) return;
            
            _unitCount = ((PieceLogic)GetMaker()).GetStat(SkillStat.Unit);
            selectedTiles.Clear();
            _isInitialized = true;
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
            if (selectedTiles.Count < _unitCount)
            {
                selectedTiles.Add(GetTargetPos());
                TileManager.Ins.UnMark(GetTargetPos());
            }

            if (selectedTiles.Count != _unitCount) return;
            
            CommitResult(new SiphonophoreActive(GetFrom(), new List<int>(selectedTiles)));
            Reset();
        }

        private void Reset()
        {
            selectedTiles.Clear();
            _unitCount = 0;
            _isInitialized = false;
        }
    }
}

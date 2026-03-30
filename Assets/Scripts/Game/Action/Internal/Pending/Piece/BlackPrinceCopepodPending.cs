using System;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlackPrinceCopepodPending : PendingAction, IDisposable, ISkills
    {
        private static int _firstPos = -1;
        private static int _secondPos = -1;
        private static int _thirdPos = -1;

        public BlackPrinceCopepodPending(PieceLogic maker, int target) : base(maker, target)
        {
        }

        public void Dispose()
        {
            _firstPos = -1;
            _secondPos = -1;
            _thirdPos = -1;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            if (_firstPos == -1 || _secondPos == -1 || _thirdPos == -1)
            {
                if (GetTargetAsPiece() == null)
                {
                    if (_firstPos == -1) {
                        _firstPos = GetTargetPos();
                        TileManager.Ins.UnMark(_firstPos);
                        Debug.Log("selected first target");
                    }
                    else if (_secondPos == -1)
                    {
                        _secondPos = GetTargetPos();
                        TileManager.Ins.UnMark(_secondPos);
                        Debug.Log("selected second target");
                    }
                    else if (_thirdPos == -1)
                    {
                        _thirdPos = GetTargetPos();
                        TileManager.Ins.UnMark(_thirdPos);
                        Debug.Log("selected third target");
                    }
                }
            }

            if (_firstPos == -1 || _secondPos == -1 || _thirdPos == -1) return;
            CommitResult(new BlackPrinceCopepodActive(GetMakerAsPiece(), _firstPos, _secondPos, _thirdPos));
            _firstPos = -1;
            _secondPos = -1;
            _thirdPos = -1;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
    }
}
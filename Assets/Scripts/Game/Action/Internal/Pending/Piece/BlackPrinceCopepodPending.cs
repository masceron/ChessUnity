using System;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlackPrinceCopepodPending : PendingAction, IDisposable, ISkills
    {
        private static int _firstPos = -1;
        private static int _secondPos = -1;
        private static int _thirdPos = -1;
        private readonly PieceLogic _blackPrinceCopepod;

        public BlackPrinceCopepodPending(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
            _blackPrinceCopepod = PieceOn(Maker);
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
                if (PieceOn(Target) == null)
                {
                    if (_firstPos == -1) {
                        _firstPos = Target;
                        TileManager.Ins.UnMark(_firstPos);
                        Debug.Log("selected first target");
                    }
                    else if (_secondPos == -1)
                    {
                        _secondPos = Target;
                        TileManager.Ins.UnMark(_secondPos);
                        Debug.Log("selected second target");
                    }
                    else if (_thirdPos == -1)
                    {
                        _thirdPos = Target;
                        TileManager.Ins.UnMark(_thirdPos);
                        Debug.Log("selected third target");
                    }
                }
            }
            
            if (_firstPos != -1 && _secondPos != -1 && _thirdPos != -1)
            {
                CommitResult(new BlackPrinceCopepodActive(Maker, _firstPos, _secondPos, _thirdPos));
                _firstPos = -1;
                _secondPos = -1;
                _thirdPos = -1;
            }
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
    }
}
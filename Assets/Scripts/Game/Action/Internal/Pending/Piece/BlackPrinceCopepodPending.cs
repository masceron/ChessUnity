using System;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlackPrinceCopepodPending : PendingAction, IDisposable
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
                    }
                    else if (_secondPos == -1)
                    {
                        _secondPos = Target;
                    }
                    else if (_thirdPos == -1)
                    {
                        _thirdPos = Target;
                    }

                    //foreach (var pending in BoardViewer.ListOf.Where(pending =>
                    //                 PieceOn(pending.Target) == null))
                    //    TileManager.Ins.UnMark(pending.Target);
                }
            }
            else
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
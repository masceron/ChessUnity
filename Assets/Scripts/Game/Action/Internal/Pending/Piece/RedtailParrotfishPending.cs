using System;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RedtailParrotfishPending : PendingAction, IDisposable
    {
        private static int _formationPos = -1;
        private static int _moveTo = -1;
        private readonly PieceLogic _redtail;

        public RedtailParrotfishPending(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
            _redtail = PieceOn(Maker);
        }

        public void Dispose()
        {
            Reset();
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            if (_formationPos == -1)
            {
                _formationPos = Target;
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                for (var i = 0; i < BoardSize; ++i)
                {
                    if (!IsActive(i)) return;
                    var formation = GetFormation(i);
                    if (formation != null) continue;

                    if (IsOnBlackSide(Maker) != _redtail.Color) continue;
                    BoardViewer.ListOf.Add(new RedtailParrotfishPending(Maker, i));
                    TileManager.Ins.MarkAsMoveable(i);
                }
            }
            else
            {
                _moveTo = Target;
                CommitResult(new RedtailParrotfishActive(Maker, _formationPos, _moveTo));
                Reset();
            }
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        private static void Reset()
        {
            _formationPos = -1;
            _moveTo = -1;
        }
    }
}
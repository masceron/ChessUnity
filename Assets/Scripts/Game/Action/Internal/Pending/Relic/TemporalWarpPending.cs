using Game.Managers;
using Game.Action;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class TemporalWarpPending : PendingAction, System.IDisposable, IInternal
    {
        private TemporalWarp _temporalWarp;

        private static PieceLogic _firstTarget;
        private static int _secondPos;

        public TemporalWarpPending(int maker, TemporalWarp tw) : base(maker)
        {
            Maker = (ushort)maker;
            _temporalWarp = tw;
        }

        public override void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);
            
            if (_firstTarget == null || _firstTarget.Color == hovering.Color)
            {
                _firstTarget = hovering;
                TileManager.Ins.Select(_firstTarget.Pos);
                SecondMark();
                
                return;
            }

            _secondPos = BoardViewer.HoveringPos;
            TileManager.Ins.UnmarkAll();

            var execute = new TemporalWarpExecute(_firstTarget.Pos, _secondPos);
            BoardViewer.Ins.ExecuteAction(execute);
            ResetTargets();

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();

            Dispose();
        }

        private void SecondMark()
        {
            var startingSize = MatchManager.Ins.StartingSize;
            var rankStart = (MaxLength - startingSize.x) / 2;
            var fileStart = (MaxLength - startingSize.y) / 2;
            var midRank = rankStart + startingSize.x / 2;
            var side = _firstTarget.Color;

            if (side)
            {
                for (var r = midRank; r < rankStart + startingSize.x; r++)
                {
                    for (var f = fileStart; f < fileStart + startingSize.y; f++)
                    {
                        var idx = IndexOf(r, f);
                        var p = PieceOn(idx);
                        if (p == null)
                            TileManager.Ins.MarkAsMoveable(idx);
                    }
                }
            }
            else
            {
                for (var r = rankStart; r < midRank; r++)
                {
                    for (var f = fileStart; f < fileStart + startingSize.y; f++)
                    {
                        var idx = IndexOf(r, f);
                        var p = PieceOn(idx);
                        if (p == null) 
                            TileManager.Ins.MarkAsMoveable(idx);
                    }
                }
            }
        }

        public void Dispose()
        {
            BoardViewer.SelectingFunction = 0;
            _temporalWarp = null;

            Tile.Tile.OnPointEnterHandle = null;
        }

        private static void ResetTargets()
        {
            _firstTarget = null;
            _secondPos = -1;
        }
    }
}


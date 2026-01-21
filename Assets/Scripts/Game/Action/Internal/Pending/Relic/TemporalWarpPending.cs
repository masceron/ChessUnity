using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action.Quiets;
using UnityEngine;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class TemporalWarpPending : Action, IPendingAble, System.IDisposable, IRelicAction
    {
        private TemporalWarp temporalWarp;

        private static PieceLogic FirstTarget;
        private static int secondPos;

        private const int TurnToBack = 4;
        private int currentTurn = TurnToBack;

        public TemporalWarpPending(int maker, TemporalWarp tw) : base(maker)
        {
            Maker = (ushort)maker;
            temporalWarp = tw;
        }

        public void CompleteAction()
        {
            var hovering = BoardUtils.PieceOn(BoardViewer.HoveringPos);
            
            if (FirstTarget == null || FirstTarget.Color == hovering.Color)
            {
                FirstTarget = hovering;
                TileManager.Ins.Select(FirstTarget.Pos);
                SecondMark();
                
                return;
            }

            Debug.Log(BoardViewer.HoveringPos);
            secondPos = BoardViewer.HoveringPos;
            TileManager.Ins.UnmarkAll();

            ActionManager.ExecuteImmediately(new NormalMove(FirstTarget.Pos, secondPos));
            ResetTargets();

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            temporalWarp.SetCooldown();
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
            var side = FirstTarget.Color;

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
            temporalWarp = null;

            Tile.Tile.OnPointEnterHandle = null;
        }

        private static void ResetTargets()
        {
            FirstTarget = null;
            secondPos = -1;
        }

        protected override void ModifyGameState()
        {
            

        }
    }
}


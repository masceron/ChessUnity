using Game.Action.Internal.Pending.Piece;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using MemoryPack;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class PencilUrchinActive : Action, ISkills, IDontEndTurn
    {
        [MemoryPackConstructor]
        private PencilUrchinActive() { }

        private int _hoveringPos = -1;
        private int _gridSize    = 1;
        private int _castRange   = 3;

        public PencilUrchinActive(PieceLogic maker, int target, int gridSize, int castRange) : base(maker, target)
        {
            _gridSize  = gridSize;
            _castRange = castRange;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMakerAsPiece();
            if (maker == null || pieceAI == null) return 0;
            return pieceAI.Color != maker.Color ? -5 : 0;
        }

        protected override void ModifyGameState()
        {
            UnityEngine.Debug.Log("PencilUrchinActive ModifyGameState");
            //Làm lại
            // Tile.Tile.OnPointEnterHandle = thisTile =>
            // {
            //     if (Distance(IndexOf(thisTile.Rank, thisTile.File), From) > _castRange)
            //     {
            //         TileManager.Ins.UnmarkAll();
            //         return;
            //     }
            //
            //     var hoveringTile = TileManager.Ins.GetTile(_hoveringPos);
            //     if (hoveringTile != null) TileManager.Ins.MarkTileInRange(hoveringTile, _gridSize, false);
            //
            //     hoveringTile = thisTile;
            //     _hoveringPos = IndexOf(hoveringTile.Rank, hoveringTile.File);
            //     TileManager.Ins.MarkTileInRange(hoveringTile, _gridSize, true);
            //
            //     if (BoardViewer.SelectingFunction != 3)
            //         BoardViewer.SelectingFunction = 3;
            //     if (BoardViewer.Selecting != -2)
            //         BoardViewer.Selecting = -2;
            //
            //     var pending = new PencilUrchinSkillPending(_hoveringPos, GetMakerAsPiece(), _gridSize);
            //     if (!BoardViewer.ListOf.Contains(pending, new ActionComparer()))
            //         BoardViewer.ListOf.Add(pending);
            // };
        }
    }
}
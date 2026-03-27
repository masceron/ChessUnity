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
    public partial class ArcticBrittleStarActive : Action, ISkills, IDontEndTurn
    {
        [MemoryPackConstructor]
        private ArcticBrittleStarActive()
        {
        }

        private int _hoveringPos = -1;
        private int _gridSize = 2;
        private int _castRange = 3;

        public ArcticBrittleStarActive(int maker, int target, int gridSize, int castRange) : base(maker)
        {
            Target = target;
            _gridSize = gridSize;
            _castRange = castRange;

        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -5;
            return 0;
        }

        protected override void ModifyGameState()
        {
            Tile.Tile.OnPointEnterHandle = thisTile =>
            {
                if (Distance(IndexOf(thisTile.rank, thisTile.file), Maker) > _castRange)
                {
                    TileManager.Ins.UnmarkAll();
                    return;
                }

                var hoveringTile = TileManager.Ins.GetTile(_hoveringPos);
                if (hoveringTile != null) TileManager.Ins.MarkTileInRange(hoveringTile, _gridSize, false);

                hoveringTile = thisTile;
                _hoveringPos = IndexOf(hoveringTile.rank, hoveringTile.file);
                TileManager.Ins.MarkTileInRange(hoveringTile, _gridSize, true);

                if (BoardViewer.SelectingFunction != 3)
                {
                    BoardViewer.Selecting         = -2;
                    BoardViewer.SelectingFunction = 3;
                }

                // Tính startRank/startFile giống TileManager.MarkTileInRange để execute khớp preview
                int startRank, startFile;
                if (_gridSize % 2 == 0)
                {
                    startRank = thisTile.rank;
                    startFile = thisTile.file;
                    if      (thisTile.corner == Corner.BottomRight) { startRank = startRank - _gridSize / 2 + 1; startFile = startFile - _gridSize / 2 + 1; }
                    else if (thisTile.corner == Corner.TopLeft)     { startFile = startFile - _gridSize / 2 + 1; startRank = startRank - _gridSize / 2; }
                    else if (thisTile.corner == Corner.TopRight)    { startRank = startRank - _gridSize / 2;     startFile = startFile - _gridSize / 2; }
                    else                                             { startRank = startRank - _gridSize / 2 + 1; startFile = startFile - _gridSize / 2; } // BottomLeft
                }
                else
                {
                    var radius = _gridSize / 2;
                    startRank = thisTile.rank - radius;
                    startFile = thisTile.file - radius;
                }

                var pending = new ArcticBrittleStarSkillPending(
                    _hoveringPos, (ArcticBrittleStar)PieceOn(Maker), startRank, startFile, _gridSize);

                if (!BoardViewer.ListOf.Contains(pending, new ActionComparer()))
                    BoardViewer.ListOf.Add(pending);
            };
        }
    }
}
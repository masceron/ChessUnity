using System;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PencilUrchinSkillPending : PendingAction, IDisposable, ISkills, ILocaltionTarget
    {
        private PieceLogic _piece;
        private readonly int _gridSize;

        public PencilUrchinSkillPending(int tilePos, PieceLogic piece, int gridSize) : base(tilePos)
        {
            Maker     = tilePos;
            Target    = tilePos;
            _piece    = piece;
            _gridSize = gridSize;
        }

        public void Dispose()
        {
            _piece = null;
            Tile.Tile.OnPointEnterHandle = null;
        }

        public int AIPenaltyValue(PieceLogic maker) => 0;

        protected override void CompleteAction()
        {
            var clickedTile = TileManager.Ins.GetTile(Maker);
            if (clickedTile == null) return;

            int startRank, startFile;
            if (_gridSize % 2 == 0)
            {
                startRank = clickedTile.rank;
                startFile = clickedTile.file;
                if      (clickedTile.corner == Corner.BottomRight) { startRank = startRank - _gridSize / 2 + 1; startFile = startFile - _gridSize / 2 + 1; }
                else if (clickedTile.corner == Corner.TopLeft)     { startFile = startFile - _gridSize / 2 + 1; startRank = startRank - _gridSize / 2; }
                else if (clickedTile.corner == Corner.TopRight)    { startRank = startRank - _gridSize / 2;     startFile = startFile - _gridSize / 2; }
                else                                               { startRank = startRank - _gridSize / 2 + 1; startFile = startFile - _gridSize / 2; } // BottomLeft
            }
            else
            {
                var radius = _gridSize / 2;
                startRank  = clickedTile.rank - radius;
                startFile  = clickedTile.file - radius;
            }

            BoardUtils.SetCooldown(_piece.Pos, ((IPieceWithSkill)_piece).TimeToCooldown);
            CommitResult(new PencilUrchinSkillExecute(_piece.Pos, startRank, startFile, _gridSize));
        }
    }
}

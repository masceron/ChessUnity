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
    /// <summary>
    /// PendingAction + ISkills.
    /// Được tạo 2 lần:
    ///  1. Trong Skills lambda (Maker=Target=Pos) → MarkSkill() highlight ô quân.
    ///  2. Trong OnPointEnterHandle (Maker=Target=tilePos) → thêm vào ListOf để click tìm thấy.
    /// CompleteAction: lưu targetPos, set cooldown, CommitResult(execute NxN).
    /// </summary>
    public class ArcticBrittleStarSkillPending : PendingAction, IDisposable, ISkills, ILocaltionTarget
    {
        private ArcticBrittleStar _piece;
        private readonly int _startRank; // top-left ô của NxN grid (giống TileManager)
        private readonly int _startFile;
        private readonly int _gridSize;

        public ArcticBrittleStarSkillPending(int tilePos, ArcticBrittleStar piece,
            int startRank, int startFile, int gridSize) : base(tilePos)
        {
            Maker      = tilePos;
            Target     = tilePos;
            _piece     = piece;
            _startRank = startRank;
            _startFile = startFile;
            _gridSize  = gridSize;
        }

        public void Dispose()
        {
            _piece = null;
            Tile.Tile.OnPointEnterHandle = null;
        }

        public int AIPenaltyValue(PieceLogic maker) => 0;

        protected override void CompleteAction()
        {
            BoardUtils.SetCooldown(_piece.Pos, ((IPieceWithSkill)_piece).TimeToCooldown);
            CommitResult(new ArcticBrittleStarSkillExecute(_piece.Pos, _startRank, _startFile, _gridSize));
        }
    }
}
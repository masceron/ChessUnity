using System;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;


namespace Game.Action.Internal.Pending.Piece
{
    //Làm lại
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // /// <summary>
    // /// PendingAction + ISkills.
    // /// Được tạo 2 lần:
    // ///  1. Trong Skills lambda (Maker=Target=Pos) → MarkSkill() highlight ô quân.
    // ///  2. Trong OnPointEnterHandle (Maker=Target=tilePos) → thêm vào ListOf để click tìm thấy.
    // /// CompleteAction: lưu targetPos, set cooldown, CommitResult(execute NxN).
    // /// </summary>
    // public class ArcticBrittleStarSkillPending : PendingAction, IDisposable, ISkills, ILocaltionTarget
    // {
    //     private PieceLogic _piece;
    //     private readonly int _gridSize;
    //
    //     public ArcticBrittleStarSkillPending(int tilePos, PieceLogic piece, int gridSize) : base(piece, tilePos)
    //     {
    //         _piece     = piece;
    //         _gridSize  = gridSize;
    //     }
    //
    //     public void Dispose()
    //     {
    //         _piece = null;
    //         //Tile.Tile.OnPointEnterHandle = null;
    //     }
    //
    //     public int AIPenaltyValue(PieceLogic maker) => 0;
    //
    //     protected override void CompleteAction()
    //     {
    //         // Lấy Tile object của ô đang hover/click để đọc corner
    //         var clickedTile = TileManager.Ins.GetTile(Target);
    //         if (clickedTile == null) return;
    //
    //         int startRank, startFile;
    //         if (_gridSize % 2 == 0)
    //         {
    //             startRank = clickedTile.Rank;
    //             startFile = clickedTile.File;
    //             if      (clickedTile.Corner == Corner.BottomRight) { startRank = startRank - _gridSize / 2 + 1; startFile = startFile - _gridSize / 2 + 1; }
    //             else if (clickedTile.Corner == Corner.TopLeft)     { startFile = startFile - _gridSize / 2 + 1; startRank = startRank - _gridSize / 2; }
    //             else if (clickedTile.Corner == Corner.TopRight)    { startRank = startRank - _gridSize / 2;     startFile = startFile - _gridSize / 2; }
    //             else                                                { startRank = startRank - _gridSize / 2 + 1; startFile = startFile - _gridSize / 2; } // BottomLeft
    //         }
    //         else
    //         {
    //             var radius = _gridSize / 2;
    //             startRank  = clickedTile.Rank - radius;
    //             startFile  = clickedTile.File - radius;
    //         }
    //
    //         BoardUtils.SetCooldown(_piece, ((IPieceWithSkill)_piece).TimeToCooldown);
    //         CommitResult(new ArcticBrittleStarSkillExecute(_piece, startRank, startFile, _gridSize));
    //     }
    // }
}
using System;
using Game.Action.Relics;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // public class MangroveCharmPending : PendingAction, IDisposable, IRelicAction
    // {
    //     public static PieceLogic FirstTarget;
    //     public static PieceLogic SecondTarget;
    //     private MangroveCharm _mangroveCharm;
    //
    //     public MangroveCharmPending(MangroveCharm e, int target) : base(null, target)
    //     {
    //         _mangroveCharm = e;
    //     }
    //
    //     public void Dispose()
    //     {
    //         ResetTargets();
    //
    //         _mangroveCharm = null;
    //
    //         BoardViewer.SelectingFunction = 0;
    //     }
    //
    //     protected override void CompleteAction()
    //     {
    //         var hovering = BoardUtils.PieceOn(BoardViewer.HoveringPos);
    //         if (FirstTarget == null)
    //         {
    //             FirstTarget = hovering;
    //             TileManager.Ins.UnmarkAll();
    //             TileManager.Ins.MarkNextEachPiece(FirstTarget.Color, FirstTarget.Pos);
    //             TileManager.Ins.Select(FirstTarget.Pos);
    //             return;
    //         }
    //
    //         SecondTarget = hovering;
    //         TileManager.Ins.UnmarkAll();
    //         CommitResult(new MangroveCharmExecute(FirstTarget.Pos, SecondTarget.Pos));
    //
    //         _mangroveCharm.SetCooldown();
    //         BoardViewer.Selecting = -1;
    //         BoardViewer.SelectingFunction = 0;
    //         MatchManager.Ins.InputProcessor.UpdateRelic();
    //
    //         ResetTargets();
    //     }
    //
    //     private static void ResetTargets()
    //     {
    //         FirstTarget = null;
    //         SecondTarget = null;
    //     }
    // }
    //Làm lại
}
using System;
using System.Collections.Generic;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Piece
{
    //Làm lại
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // public class EmeraldCrabPending : PendingAction, IDisposable, ISkills
    // {
    //     private static List<int> _selectedTarget;
    //     private readonly int _duration;
    //     private readonly int _numTarget;
    //
    //     public EmeraldCrabPending(PieceLogic maker, PieceLogic target, int duration, int numTarget) : base(maker, target)
    //     {
    //         _selectedTarget = new List<int>();
    //         _duration = duration;
    //         _numTarget = numTarget;
    //     }
    //
    //     public void Dispose()
    //     {
    //         _selectedTarget.Clear();
    //         BoardViewer.SelectingFunction = 0;
    //     }
    //
    //     protected override void CompleteAction()
    //     {
    //         if (_selectedTarget.Count < _numTarget)
    //         {
    //             if (GetTargetAsPiece() != null)
    //             {
    //                 _selectedTarget.Add(GetTargetPos());
    //                 TileManager.Ins.UnMark(GetTargetPos());
    //             }
    //         }
    //
    //         if (_selectedTarget.Count == _numTarget)
    //         {
    //             CommitResult(new EmeraldCrabActive(GetMakerAsPiece(), _selectedTarget, _duration));
    //         }
    //     }
    //
    //     public int AIPenaltyValue(PieceLogic p)
    //     {
    //         return 0;
    //     }
    // }
}
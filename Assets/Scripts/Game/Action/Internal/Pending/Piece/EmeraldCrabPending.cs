using System;
using System.Collections.Generic;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EmeraldCrabPending : PendingAction, IDisposable, ISkills
    {
        private static List<int> selectedTarget;
        private int Duration;
        private int NumTarget;

        public EmeraldCrabPending(int maker, int target, int duration, int numTarget) : base(maker)
        {
            Maker = maker;
            Target = target;
            selectedTarget = new List<int>();
            Duration = duration;
            NumTarget = numTarget;
        }

        public void Dispose()
        {
            selectedTarget.Clear();
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            Debug.Log("EmeraldCrabPending CompleteAction");
            if (selectedTarget.Count < NumTarget)
            {
                if (PieceOn(Target) != null)
                {
                    selectedTarget.Add(Target);
                    TileManager.Ins.UnMark(Target);
                }
            }

            if (selectedTarget.Count == NumTarget)
            {
                CommitResult(new EmeraldCrabActive(Maker, selectedTarget, Duration));
            }
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
    }
}
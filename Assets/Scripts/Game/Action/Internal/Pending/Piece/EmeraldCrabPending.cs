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
        private EmeraldCrab emeraldCrab;
        private int Duration;
        private int NumTarget;

        public EmeraldCrabPending(int maker, int target, int duration) : base(maker)
        {
            Maker = maker;
            Target = target;
            emeraldCrab = (EmeraldCrab)PieceOn(Maker);
            selectedTarget = new List<int>();
            Duration = duration;
        }

        public void Dispose()
        {
            selectedTarget.Clear();
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            NumTarget = emeraldCrab.GetStat(SkillStat.Target);
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
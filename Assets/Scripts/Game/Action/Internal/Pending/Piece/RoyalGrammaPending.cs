using System;
using System.Collections.Generic;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using UX.UI.Ingame.RoyalGrammaUI;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    public class RoyalGrammaPending : PendingAction, IDisposable, ISkills
    {
        public static List<int> positions = new();
        public RoyalGrammaPending(int maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }
        public void CommitResult(string chosenType)
        {
            CommitResult(new RoyalGrammaActive(GetFrom(), new List<int>(positions), chosenType));
            positions.Clear();
        }
        public void Dispose()
        {
            positions.Clear();
            BoardViewer.SelectingFunction = 0;
        }
        protected override void CompleteAction()
        {
            positions.Add(GetTargetPos());
            TileManager.Ins.UnMark(GetTargetPos());
            if (positions.Count == GetMaker() as PieceLogic.GetStat(SkillStat.Target))
            {
                var ui = BoardViewer.Ins.GetOrInstantiateUI<RoyalGrammaUI>(IngameSubmenus.RoyalGrammaUI);
                ui.Load(this);
            }
        }
    }
}
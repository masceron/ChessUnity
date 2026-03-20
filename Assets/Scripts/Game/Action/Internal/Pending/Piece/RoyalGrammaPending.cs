using System;
using System.Collections.Generic;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using UX.UI.Ingame.RoyalGrammaUI;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    public class RoyalGrammaPending : PendingAction, ISkills
    {
        public static List<int> positions = new();
        public RoyalGrammaPending(int maker, int target) : base(maker)
        {
            Target = target;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }
        public void CommitResult(string chosenType)
        {
            CommitResult(new RoyalGrammaActive(Maker, new List<int>(positions), chosenType));
            positions.Clear();
        }
        protected override void CompleteAction()
        {
            if (positions.Count == PieceOn(Maker).GetStat(SkillStat.Target))
            {
                var ui = BoardViewer.Ins.GetOrInstantiateUI<RoyalGrammaUI>(IngameSubmenus.RoyalGrammaUI);
            }
            else
            {
                positions.Add(Target);
                TileManager.Ins.UnMark(Target);
            }
        }
    }
}
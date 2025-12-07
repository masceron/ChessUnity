using System;
using System.Collections.Generic;
using Game.AI;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HermitCrabSwap: Action, ISkills, IAIAction
    {
        public HermitCrabSwap(int maker, int to) : base(maker, true)
        {
            Target = (ushort)to;
        }
        protected override void Animate()
        {
           PieceManager.Ins.Swap(Maker, Target);
        }
        protected override void ModifyGameState()
        {
            // var board = PieceBoard();
            // int a = Maker;
            // int b = Target;

            // var pieceA = board[(ushort)a];
            // var pieceB = board[(ushort)b];

            // board[(ushort)a] = pieceB;
            // if (pieceB != null) pieceB.Pos = (ushort)a;
            // board[(ushort)b] = pieceA;
            // if (pieceA != null) pieceA.Pos = (ushort)b;
            // SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);

            MatchManager.Ins.GameState.Swap(Maker, Target);
            SetCooldown(Target, ((IPieceWithSkill)PieceOn(Target)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            int rank = RankOf(Maker);
            int file = FileOf(Maker);
            List<int> candidates = new();
            for(int i = rank - 3; i < rank + 3; ++i)
            {
                for(int j = file - 3; j < file + 3; ++j)
                {
                    if (!VerifyBounds(file) || !VerifyBounds(rank)) { continue; }
                    if (PieceOn(IndexOf(i, j)) != null){ candidates.Add(IndexOf(i, j)); }
                }
            }
            var r = new Random();
            Target = candidates[r.Next(candidates.Count)];
            BoardViewer.Ins.ExecuteAction(this);
        }
    
    }
}
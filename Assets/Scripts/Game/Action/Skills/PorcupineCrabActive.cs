using System;
using System.Collections.Generic;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using MemoryPack;
using UnityEngine.Rendering;

namespace Game.Action.Skills
{
    public class PorcupineCrabActive : Action, ISkills
    {
        private const int BleedingStack = 5;
        [MemoryPackConstructor]
        private PorcupineCrabActive()
        {
        }
        
        public PorcupineCrabActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }   
        
        protected override void ModifyGameState()
        {
            var makerPiece = PieceOn(Maker);

            ActionManager.EnqueueAction(new NormalMove(Maker, Target));

            var (rank1, file1) = RankFileOf(Maker);
            var (rank2, file2) = RankFileOf(Target);

            var tiles = Pathfinder.AllLineBlockers(rank1, file1, rank2, file2);

            foreach (var (rank, file) in tiles)
            {
                var piece = PieceOn(IndexOf(rank, file));

                if (piece != null && piece.Color != makerPiece.Color)
                {
                    ActionManager.EnqueueAction(
                        new ApplyEffect(new Bleeding(BleedingStack, piece), makerPiece)
                    );
                }
            }

            SetCooldown(Maker, ((IPieceWithSkill)makerPiece).TimeToCooldown);
        }
        
        List<(int rank, int file)> GetPath(int rank1, int file1, int rank2, int file2)    
        {
            var path = new List<(int, int)>();

            int dRank = rank2 - rank1;
            int dFile = file2 - file1;

            int steps = GCD(Math.Abs(dRank), Math.Abs(dFile));

            if (steps == 0) return path; // cùng 1 điểm

            int stepRank = dRank / steps;
            int stepFile = dFile / steps;

            int currentRank = rank1 + stepRank;
            int currentFile = file1 + stepFile;

            for (int i = 1; i < steps; i++)
            {
                path.Add((currentRank, currentFile));
                currentRank += stepRank;
                currentFile += stepFile;
            }

            return path;
        }

        int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}
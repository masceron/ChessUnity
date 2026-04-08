using System;
using System.Collections.Generic;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using MemoryPack;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class PorcupineCrabActive : Action, ISkills
    {
        private int BleedingStack;
        [MemoryPackConstructor]
        private PorcupineCrabActive()
        {
        }
        
        public PorcupineCrabActive(PieceLogic maker, int target, int Stack) : base(maker, target)
        {
            BleedingStack = Stack;
        }   
        
        protected override void ModifyGameState()
        {
            var makerPiece = GetMakerAsPiece();

            ActionManager.EnqueueAction(new NormalMove(makerPiece, GetTargetPos()));

            var (rank1, file1) = RankFileOf(GetFrom());
            var (rank2, file2) = RankFileOf(GetTargetPos());

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

            ActionManager.EnqueueAction(new CooldownSkill(makerPiece));
        }
        
        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }
    }
}
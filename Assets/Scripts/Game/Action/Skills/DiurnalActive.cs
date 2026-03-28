using System;
using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class DiurnalActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private DiurnalActive()
        {
        }

        public DiurnalActive(int maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new NormalMove(GetFrom(), GetTargetPos()));
        }
    }
}
using System;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class ThreadPipefishActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private ThreadPipefishActive()
        {
        }

        public ThreadPipefishActive(int maker, int target) : base(maker, target)
        {
            Maker = maker;
            Target = target;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new ThreadPipefishEffect(GetMaker(), GetTarget())));
        }
    }
}
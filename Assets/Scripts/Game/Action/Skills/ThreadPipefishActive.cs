using MemoryPack;
using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class ThreadPipefishActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private ThreadPipefishActive() { }

        public ThreadPipefishActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new ThreadPipefishEffect(PieceOn(Maker), PieceOn(Target))));
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}
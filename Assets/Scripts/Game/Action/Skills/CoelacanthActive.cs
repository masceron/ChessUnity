using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class CoelacanthActive : Action, ISkills
    {
        private int Duration;
        [MemoryPackConstructor]
        private CoelacanthActive()
        {
        }

        public CoelacanthActive(PieceLogic maker, PieceLogic target, int duration) : base(maker, target)
        {
            Duration = duration;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(Duration, GetTarget() as PieceLogic)));
        }
    }
}
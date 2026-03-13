using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class WhiteMonkfishActive : Action, ISkills
    {
        private int Duration;
        [MemoryPackConstructor]
        private WhiteMonkfishActive()
        {
        }

        public WhiteMonkfishActive(int maker, int target, int duration) : base(maker)
        {
            Target = target;
            Duration = duration;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return -15;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Leashed(PieceOn(Target), Target, Duration)));
        }
    }
}
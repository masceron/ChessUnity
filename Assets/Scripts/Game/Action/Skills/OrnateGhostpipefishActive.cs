using Game.Action.Internal;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class OrnateGhostpipefishActive : Action, ISkills
    {
        [MemoryPackConstructor]

        private OrnateGhostpipefishActive()
        {
        }
        [MemoryPackInclude]
        private int Duration;

        public OrnateGhostpipefishActive(PieceLogic maker, PieceLogic to, int duration) : base(maker, to)
        {
            Duration = duration;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var caller = GetMakerAsPiece();
            var pOnTarget = GetTargetAsPiece();
            if (caller == null || pOnTarget == null) return;

            var isSelfTarget = caller == pOnTarget;
            var effectDuration = isSelfTarget ? Duration * 2 : Duration;

            ActionManager.EnqueueAction(new ApplyEffect(new Ethereal(effectDuration, pOnTarget)));
            ActionManager.EnqueueAction(new CooldownSkill(caller));
        }
    }
}


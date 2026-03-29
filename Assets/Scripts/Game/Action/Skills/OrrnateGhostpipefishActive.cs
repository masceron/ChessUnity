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
    public partial class OrrnateGhostpipefishActive : Action, ISkills
    {
        [MemoryPackConstructor]

        private OrrnateGhostpipefishActive()
        {
        }
        private readonly int Duration;

        public OrrnateGhostpipefishActive(int maker, int to, int duration) : base(maker, to)
        {
            Duration = duration;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var caller = GetMaker() as PieceLogic;
            var pOnTarget = GetTarget();
            if (caller == null) return;

            if (GetMakerPos() == GetTargetPos())
            {
                if (caller.HasState(StateType.None))
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Ethereal(Duration * 2, caller)));
                }
                else
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Ethereal(Duration * 2, pOnTarget)));
                }
            }
            else
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Ethereal(Duration, pOnTarget)));
            }

            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)caller).TimeToCooldown);
        }
    }
}


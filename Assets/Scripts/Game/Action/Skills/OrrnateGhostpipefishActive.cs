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

        public OrrnateGhostpipefishActive(int maker, int to, int duration) : base(maker)
        {
            Target = to;
            Duration = duration;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var caller = PieceOn(Maker);
            var pOnTarget = PieceOn(Target);
            if (caller == null) return;

            if (Maker == Target)
            {
                if (caller.HasState(Effects.States.StateType.None))
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

            SetCooldown(Maker, ((IPieceWithSkill)caller).TimeToCooldown);
        }
    }
}


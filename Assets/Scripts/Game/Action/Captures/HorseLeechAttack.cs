using Game.Action.Internal;
using Game.Effects.Debuffs;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class HorseLeechAttack : Action, ICaptures
    {
        [MemoryPackConstructor]
        private HorseLeechAttack()
        {
        }

        public HorseLeechAttack(int maker, int target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(4, GetTarget()), GetMaker()));
            ActionManager.EnqueueAction(new KillPiece(GetFrom()));
        }
    }
}
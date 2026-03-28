using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class SirenHarpoonExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private SirenHarpoonExecute()
        {
        }

        public SirenHarpoonExecute(int maker, int target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Controlled(-1, GetTarget())));
            ActionManager.EnqueueAction(new ApplyEffect(new Pacified(1, GetTarget())));
        }
    }
}
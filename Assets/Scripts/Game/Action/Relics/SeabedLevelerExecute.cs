using Game.Managers;
using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class SeabedLevelerExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private SeabedLevelerExecute()
        {
        }

        public SeabedLevelerExecute(int maker, int target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            FormationManager.Ins.RemoveFormation(GetTargetPos());
        }
    }
}
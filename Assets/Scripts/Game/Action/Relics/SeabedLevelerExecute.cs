using MemoryPack;
using Game.Managers;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class SeabedLevelerExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private SeabedLevelerExecute() { }

        public SeabedLevelerExecute(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }

        protected override void ModifyGameState()
        {
            FormationManager.Ins.RemoveFormation(Target);
        }
    }
}
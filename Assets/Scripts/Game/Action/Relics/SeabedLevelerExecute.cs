using Game.Managers;
using Game.Tile;
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

        public SeabedLevelerExecute(Formation target) : base(null, target)
        {
        }

        protected override void ModifyGameState()
        {
            FormationManager.Ins.RemoveFormation(GetTargetAsFormation());
        }
    }
}
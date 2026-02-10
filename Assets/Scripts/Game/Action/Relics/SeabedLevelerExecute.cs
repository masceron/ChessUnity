using Game.Action.Internal;
using Game.Managers;

namespace Game.Action.Relics
{
    public class SeabedLevelerExecute : Action, IRelicAction
    {
        public SeabedLevelerExecute(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void ModifyGameState()
        {
            FormationManager.Ins.RemoveFormation(Target);
        }
    }
}
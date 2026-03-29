using Game.Action.Relics;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChaoticConstructor : RelicLogic
    {
        public ChaoticConstructor(RelicConfig config) : base(config)
        {
            type = config.Type;
            Color = config.Color;
            TimeCooldown = 2;
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown != 0) return;
            var action = new ChaoticConstructorAction();
            BoardViewer.Ins.ExecuteAction(action);
            BoardViewer.Selecting = -2;
            BoardViewer.SelectingFunction = 4;
        }

        public override void ActiveForAI()
        {
        }
    }
}
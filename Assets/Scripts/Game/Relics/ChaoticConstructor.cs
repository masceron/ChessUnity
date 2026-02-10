using Game.Action.Internal.Pending.Relic;
using Game.Action.Relics;
using Game.Common;
using Game.Managers;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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
            if (CurrentCooldown == 0)
            {
                var action = new ChaoticConstructorAction(-1);
                BoardViewer.Ins.ExecuteAction(action);
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }

        public override void ActiveForAI()
        {

        }
    }
}
using UnityEngine;
using UX.UI.Ingame;
using Game.Relics.Commons;
using UX.UI.Ingame.LedgerStone;
namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class LedgerStone : RelicLogic
    {
        public LedgerStone(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                var ui = Object.Instantiate(UIHolder.Ins.Get(IngameSubmenus.LedgerStoneUI)).GetComponent<LedgerStoneUI>();
                ui.Load(this);
            }
        }

        public override void ActiveForAI()
        {
        }
    }
}
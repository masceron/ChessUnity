using Game.Relics.Commons;
using UX.UI.Ingame;
using UX.UI.Ingame.HermosHornUI;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HermosHorn : RelicLogic
    {
        public HermosHorn(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown != 0) return;

            var ui = BoardViewer.Ins.GetOrInstantiateUI<HermosHornUI>(IngameSubmenus.HermosHornUI);
            ui.Load(this);
        }

        public override void ActiveForAI()
        {
            
        }
    }
}
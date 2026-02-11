using Game.Relics.Commons;
using UX.UI.Ingame;
using UX.UI.Ingame.CoralTome;

namespace Game.Relics
{
    public class CoralTome : RelicLogic
    {
        public CoralTome(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                var ui = BoardViewer.Ins.GetOrInstantiateUI<CoralTomePendingMenu>(IngameSubmenus.CoralTomeUI);

                ui.Load();
            }
        }
    }
}
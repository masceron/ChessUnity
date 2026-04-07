using Game.Relics.Commons;

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
                //Làm lại
                //var ui = BoardViewer.Ins.GetOrInstantiateUI<CoralTomeUI>(IngameSubmenus.CoralTomeUI);

                //ui.Load();
            }
        }
    }
}
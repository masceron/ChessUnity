using Game.Common;
using Game.Managers;
using Game.Relics.Commons;
using Game.Tile;
using UX.UI.Ingame;

namespace Game.Relics
{
    public class KelpBanner : RelicLogic
    {
        public KelpBanner(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                foreach (var formation in BoardUtils.GetFormation(FormationType.Kelp))
                {
                    TileManager.Ins.MarkAsMoveable(formation.Pos);
                    //Làm lại
                    //var pending = new KelpBannerPending(this);
                    //BoardViewer.ListOf.Add(pending);
                }

                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }
    }
}
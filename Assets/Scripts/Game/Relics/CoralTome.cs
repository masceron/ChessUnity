using Game.Relics.Commons;
using UnityEngine;
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
                var ui = Object.FindAnyObjectByType<CoralTomeUI>(FindObjectsInactive.Include);

                if (!ui)
                {
                    var canvas = Object.FindAnyObjectByType<BoardViewer>(FindObjectsInactive.Exclude);
                    ui = Object.Instantiate(UIHolder.Ins.Get(IngameSubmenus.CoralTomeUI), canvas.transform)
                        .GetComponent<CoralTomeUI>();
                }
                else
                {
                    ui.gameObject.SetActive(true);
                }

                ui.Load();
            }
        }
    }
}
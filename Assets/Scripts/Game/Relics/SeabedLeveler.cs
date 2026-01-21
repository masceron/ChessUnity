using Game.Common;
using Game.Effects.Others;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.SeabedLevelerUI;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeabedLeveler : RelicLogic
    {
        public readonly Charge charge;
        public SeabedLeveler(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
            charge = new Charge(0, Color);
            BoardUtils.AddEffectObserver(charge);
        }

        public override void Activate()
        {
            if (charge.Strength >= 0)
            {

                var ui = Object.FindAnyObjectByType<SeabedLevelerUI>(FindObjectsInactive.Include);

                if (!ui)
                {
                    var canvas = Object.FindAnyObjectByType<BoardViewer>(FindObjectsInactive.Exclude);
                    ui = Object.Instantiate(UIHolder.Ins.Get(IngameSubmenus.SeabedLevelerUI), canvas.transform)
                        .GetComponent<SeabedLevelerUI>();
                }
                else
                {
                    ui.gameObject.SetActive(true);
                }

                ui.Load();
            }

            if (CurrentCooldown > 0)
            {
                charge.Strength = 0;
            }
        }
        

    }
}
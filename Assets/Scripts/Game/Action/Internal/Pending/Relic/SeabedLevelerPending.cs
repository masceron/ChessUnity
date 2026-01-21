using Game.Action.Relics;
using Game.Common;
using Game.Effects;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Relics;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.SeabedLevelerUI;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class SeabedLevelerPending : Action, IPendingAble, System.IDisposable, IRelicAction
    {
        private SeabedLeveler seabedLeveler;

        public SeabedLevelerPending(SeabedLeveler sl) : base(-1)
        {
            seabedLeveler = sl;
        }

        public void Dispose()
        {
            seabedLeveler = null;
            BoardViewer.SelectingFunction = 0;
        }

        public void CompleteAction()
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

        protected override void ModifyGameState()
        {

        }

    }
}

using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.RustyParrotfishUI;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    public class RustyParrotfishPending : PendingAction
    {
        public RustyParrotfishPending(int maker) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }

        protected override void Animate()
        {

        }

        public override void CompleteAction()
        {
            Debug.Log("complete rusty parrotfish action");
            var ui = Object.FindAnyObjectByType<RustyParrotfishUI>(FindObjectsInactive.Include);

            if (!ui)
            {
                var canvas = Object.FindAnyObjectByType<BoardViewer>(FindObjectsInactive.Exclude);
                ui = Object.Instantiate(UIHolder.Ins.Get(IngameSubmenus.RustyParrotfishUI), canvas.transform)
                    .GetComponent<RustyParrotfishUI>();
            }
            else
            {
                ui.gameObject.SetActive(true);
            }

            ui.Load(Maker, Target);
        }
    }
}
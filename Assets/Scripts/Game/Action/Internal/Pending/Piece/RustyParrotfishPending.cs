using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.RustyParrotfishUI;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class RustyParrotfishPending : Action, ISkills, IPendingAble
    {
        public RustyParrotfishPending(int maker) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }

        protected override void Animate()
        {

        }

        protected override void ModifyGameState()
        {
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        int ISkills.AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }

        public void CompleteAction()
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
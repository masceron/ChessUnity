using Game.Board.Action.Skills;
using Game.Board.General;
using Game.UX.UI.Ingame.ThalassosResurrector;
using UnityEngine;

namespace Game.Board.Action.Internal.Pending
{
    public class ThalassosResurrectCandidate: Action, IPendingAble, IInternal, ISkills
    {
        public ThalassosResurrectCandidate(int maker, int pos) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)pos;
        }

        public void CompleteAction()
        {
            var selector = Object.FindAnyObjectByType<ThalassosResurrector>(FindObjectsInactive.Include);
            if (!selector)
            {
                var canvas = GameObject.Find("IngameUI");
                selector = Object.Instantiate(MatchManager.Ins.InputProcessor.thalassosResurrector, canvas.transform).GetComponent<ThalassosResurrector>();
            }
            else selector.gameObject.SetActive(true);

            selector.Load(Maker, Target);
        }

        protected override void ModifyGameState()
        {}
    }
}
using Game.Board.Action.Skills;
using Game.Board.General;
using Game.UX.UI.ThalassosResurrector;
using UnityEngine;

namespace Game.Board.Action.Internal.Pending
{
    public class ThalassosResurrectCandidate: Action, IPendingAble, IInternal, ISkills
    {
        public ThalassosResurrectCandidate(int caller, int pos) : base(caller, false)
        {
            From = (ushort)caller;
            To = (ushort)pos;
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

            selector.Load(Caller, To);
        }

        protected override void ModifyGameState()
        {}
    }
}
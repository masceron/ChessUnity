using Game.Action.Skills;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.ThalassosResurrector;

namespace Game.Action.Internal.Pending
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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
                var canvas = Object.FindAnyObjectByType<BoardViewer>(FindObjectsInactive.Exclude);
                selector = Object.Instantiate(UIHolder.Ins.Get(IngameSubmenus.ThalassosResurrector), canvas.transform).GetComponent<ThalassosResurrector>();
            }
            else selector.gameObject.SetActive(true);

            selector.Load(Maker, Target);
        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
        }

        protected override void ModifyGameState()
        {}
    }
}
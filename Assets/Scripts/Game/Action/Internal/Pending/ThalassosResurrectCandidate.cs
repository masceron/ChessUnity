using Game.Action.Skills;
using Game.Managers;
using UnityEngine;
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
using System.Linq;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.ThalassosResurrector;
using static Game.Common.BoardUtils;
using Game.AI;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ThalassosResurrectCandidate: Action, IPendingAble, IInternal, ISkills
    {
        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
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

        protected override void ModifyGameState()
        {}
    }
}
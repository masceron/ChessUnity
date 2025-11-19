using static Game.Common.BoardUtils;
using Game.Action.Internal.Pending;
using Game.Managers;
using UX.UI.Ingame;
using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineIguanaActive: Action, ISkills, IPendingAble
    {
        private readonly System.Action<bool> setIsActiveSkill;
        public MarineIguanaActive(int maker, System.Action<bool> setIsActiveSkill) : base(maker)
        {
            Target = (ushort)maker;
            this.setIsActiveSkill = setIsActiveSkill;
        }
        protected override void ModifyGameState()
        {
        }
        public void CompleteAction()
        {
            setIsActiveSkill(true);
            TileManager.Ins.Unselect(Target);
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
            BoardViewer.ListOf.Clear();

            var piece = PieceOn(Maker);
            var tempList = new List<Action>();
            piece.MoveList(tempList);
            // thêm captures vào ListOf luôn :))
            foreach (var action in tempList)
            {
                if (action is Captures.ICaptures)
                {
                    BoardViewer.ListOf.Add(action);
                    TileManager.Ins.MarkAsMoveable(action.Target);
                }
            }
            

            BoardViewer.SelectingFunction = 2;
        }
    }
}
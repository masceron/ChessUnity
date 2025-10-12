using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using Game.Action.Internal.Pending;
using UX.UI.Ingame;
using Game.Managers;
using System.Collections.Generic;

namespace Game.Action.Skills
{
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumilitasActive: Action, ISkills, IPendingAble
    {
        private readonly System.Func<int> getCount;
        private readonly System.Action<int> setCount;
        private int count;
        List<int> targeted = new List<int>();
        public HumilitasActive(int maker, int to,int count, System.Func<int> getCount, System.Action<int> setCount) : base(maker, false)
        {
            Target = (ushort)to;
            this.getCount = getCount;
            this.setCount = setCount;
            this.count = count;
        }
        protected override void ModifyGameState()
        {

        }
        private void MakeSkill(int target)
        {
            if(targeted.Contains(target))
            {
                return;
            }
            targeted.Add(target);
            TileManager.Ins.Unselect(target);

            ActionManager.EnqueueAction(new ApplyEffect(new Taunted(1, PieceOn(target))));
            setCount(getCount() - 1);
            if(getCount() <= 0)
            {
                BoardViewer.ExecuteActionStatic(this);
                setCount(2);
                targeted.Clear();
                return;
            }
        }
        public void CompleteAction()
        {
            MakeSkill(Target);
        }

    }
}

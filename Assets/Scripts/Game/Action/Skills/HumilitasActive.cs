using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using Game.Action.Internal.Pending;
using UX.UI.Ingame;
using Game.Managers;
using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumilitasActive: Action, ISkills, IPendingAble
    {
        private readonly System.Func<int> getCount;
        private readonly System.Action<int> setCount;
        private readonly System.Func<List<int>> getTargeted;

        public HumilitasActive(int maker, int to, int count, System.Func<int> getCount, System.Action<int> setCount,
                    System.Func<List<int>> getTargeted) : base(maker)
        {
            Target = (ushort)to;
            this.getCount = getCount;
            this.setCount = setCount;
            this.getTargeted = getTargeted;
        }
        protected override void ModifyGameState()
        {
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        private void MakeSkill(int target)
        {
            var targetedList = getTargeted();
            
            if(targetedList.Contains(target))
            {
                return;
            }
            targetedList.Add(target);
            TileManager.Ins.Unselect(target);

            ActionManager.EnqueueAction(new ApplyEffect(new Taunted(1, PieceOn(target))));
            setCount(getCount() - 1);
            if(getCount() <= 0)
            {
                BoardViewer.Ins.ExecuteAction(this); 
                setCount(2);
                targetedList.Clear();
                return;
            }
        }
        public void CompleteAction()
        {
            MakeSkill(Target);
        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
        }
    }
}

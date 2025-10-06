using Game.Piece.PieceLogic;
using Game.Action;
using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using Game.Action.Internal.Pending;
using UX.UI.Ingame;
using static UX.UI.Ingame.BoardViewer;
using UnityEngine;
using Game.Managers;
using System.Linq;
using System.Collections.Generic;

namespace Game.Action.Skills
{
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumilitasActive: Action, ISkills, IPendingAble
    {
        List<int> targeted = new List<int>();
        public HumilitasActive(int maker, int to, int count) : base(maker, false)
        {
            Target = (ushort)to;

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
            SetCount(GetCount() + 1);  
            if(GetCount() >= 2)
            {
                BoardViewer.ExecuteActionStatic(this);
                SetCount(0);
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

using System.Collections.Generic;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MegalodonActive: Action, ISkills, IPendingAble
    {
        private readonly System.Func<int> getCount;
        private readonly System.Action<int> setCount;
        private readonly System.Func<List<int>> getTargeted;
        public MegalodonActive(int maker, int to, int count, System.Func<int> getCount, System.Action<int> setCount, 
            System.Func<List<int>> getTargeted) : base(maker)
        {
            Target = (ushort)to;
            this.getCount = getCount;
            this.setCount = setCount;
            this.getTargeted = getTargeted;
        }

        private void MakeSkill(int target)
        {
            var targetedList = getTargeted();
            var Piece = PieceOn(Maker);
            var Pos = Piece.Pos;
            foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), AttackRange))
            {
                var idx = IndexOf(rank, file);
                var pOn = PieceOn(idx);
                if (pOn != null && pOn.Color != Color)
                {
                    if (targeted.Contains(idx)) continue;
                }
            }
        }

        public void SetCoolDown()
        {
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        
        protected override void ModifyGameState()
        {
            
        }

        public void CompleteAction()
        {
            MakeSkill(Target);
        }
    }
}

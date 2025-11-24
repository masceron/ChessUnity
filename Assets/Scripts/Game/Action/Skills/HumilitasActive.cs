using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using Game.Action.Internal.Pending;
using UX.UI.Ingame;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using System;
using Game.Common;
using Game.Effects.Traits;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumilitasActive: Action, ISkills, IPendingAble, System.IDisposable
    {
        private static PieceLogic FirstTarget;
        private static PieceLogic SecondTarget;
        public HumilitasActive(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {

            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        public void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);
            if (FirstTarget == null) 
            {
                FirstTarget = hovering;
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(FirstTarget.Pos), FileOf(FirstTarget.Pos), 5))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var piece = PieceOn(index);
                    if (piece == null || piece.Color != FirstTarget.Color) continue;
                    var newAction = new HumilitasActive(Maker, index);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(index);
                }
                return;
            }
            SecondTarget = hovering; 
            
            ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, FirstTarget)));
            ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, SecondTarget)));
            BoardViewer.Ins.ExecuteAction(this);
        }
        public void Dispose()
        {
            FirstTarget = null;
            SecondTarget = null;
            BoardViewer.SelectingFunction = 0; 
        }

        public void CompleteActionForAI()
        {
            // throw new System.NotImplementedException();
        }
    }
}

using System;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.AI;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EyeshadeSculpinActive : Action, ISkills, IAIAction, IPendingAble, IDisposable
    {
        private static PieceLogic FirstTarget;
        private static PieceLogic SecondTarget;
        
        public EyeshadeSculpinActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void ModifyGameState()
        {
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        public void CompleteActionForAI()
        {
            
        }

        public void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);
            if (FirstTarget == null)
            {
                FirstTarget = hovering;
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(FirstTarget.Pos), FileOf(FirstTarget.Pos), 4))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var piece = PieceOn(index);
                    if (piece == null || piece.Color != FirstTarget.Color) continue;
                    var newAction = new EyeshadeSculpinActive(Maker, index);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(index);
                }
                return;
            }
            SecondTarget = hovering;

            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(4, 1, FirstTarget)));
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(4, 1, SecondTarget)));
            BoardViewer.Ins.ExecuteAction(this);
        }
        
        public void Dispose()
        {
            FirstTarget = null;
            SecondTarget = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}
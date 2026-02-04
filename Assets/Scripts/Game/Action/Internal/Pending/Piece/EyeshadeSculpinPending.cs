using System;
using Game.Action.Internal.Pending;
using Game.Action.Skills;
using Game.AI;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EyeshadeSculpinPending : PendingAction, ISkills, IDisposable
    {
        public static PieceLogic FirstTarget;
        public static PieceLogic SecondTarget;
        
        public EyeshadeSculpinPending(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        public override void CompleteAction()
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
                    var newAction = new EyeshadeSculpinPending(Maker, index);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(index);
                }
                return;
            }
            SecondTarget = hovering;
            BoardViewer.Ins.ExecuteAction(new EyeshadeSculpinActive(Maker, FirstTarget, SecondTarget));
        }
        
        public void Dispose()
        {
            FirstTarget = null;
            SecondTarget = null;
            BoardViewer.SelectingFunction = 0;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }
    }
}
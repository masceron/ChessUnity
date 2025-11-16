using Game.Action.Internal;
using UX.UI.Ingame;
using Game.Action.Internal.Pending;
using Game.Managers;
using static Game.Common.BoardUtils;
using Game.Common;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineIguanaAttack: Action, IPendingAble, ISkills
    {
        private static PieceLogic FirstTarget;
        private static PieceLogic SecondTarget;
        public MarineIguanaAttack(int maker, int to) : base(maker, false)
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
                foreach (var (rankOff, fileOff) in MoveEnumerators.Around(RankOf(FirstTarget.Pos), FileOf(FirstTarget.Pos), 2))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var piece = PieceOn(index);
                    if (piece == null || piece.Color != FirstTarget.Color) continue;
                    var newAction = new MarineIguanaAttack(Maker, index);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(index);
                }
                return;
            }
            SecondTarget = hovering;
            TileManager.Ins.UnmarkAll();    
            BoardViewer.Ins.Unmark();
            ActionManager.EnqueueAction(new MarinelKill(Maker, FirstTarget.Pos, SecondTarget.Pos));
            FirstTarget = null;
            SecondTarget = null;
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
        }
    }
}
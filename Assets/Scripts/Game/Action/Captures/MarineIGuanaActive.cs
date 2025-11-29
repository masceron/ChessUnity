using Game.Action.Internal;
using UX.UI.Ingame;
using Game.Action.Internal.Pending;
using Game.Managers;
using static Game.Common.BoardUtils;
using Game.Common;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using System.Collections.Generic;
using System.Linq;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineIguanaActive: Action, IPendingAble, ISkills, System.IDisposable  
    {
        private static PieceLogic FirstTarget;
        private static PieceLogic SecondTarget;
        public MarineIguanaActive(int maker, int to) : base(maker)
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
                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(FirstTarget.Pos), FileOf(FirstTarget.Pos), 2))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var piece = PieceOn(index);
                    if (piece == null || piece.Color != FirstTarget.Color) continue;
                    var newAction = new MarineIguanaActive(Maker, index);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(index);
                }
                return;
            }
            SecondTarget = hovering; 
        
            ActionManager.EnqueueAction(new MarinelKill(Maker, FirstTarget.Pos, SecondTarget.Pos));
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
            var listPieces = new List<PieceLogic>();
            
            
        }
    }
}
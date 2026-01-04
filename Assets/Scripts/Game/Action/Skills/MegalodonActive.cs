using System;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
using Game.AI;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MegalodonActive: Action, ISkills, IPendingAble, IDisposable, IAIAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -40;
            return 0;
        }

        private static PieceLogic _firstTarget;
        private static PieceLogic _secondTarget;

        public MegalodonActive(int maker, int to) : base(maker)
        {
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(_firstTarget.Pos));
            ActionManager.EnqueueAction(new KillPiece(_secondTarget.Pos));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
            
            ResetTargets();
        }

        public void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);

            if (_firstTarget == null || _firstTarget.Color == hovering.Color)
            {
                _firstTarget = hovering;
                TileManager.Ins.UnmarkAll();
                TileManager.Ins.Select(_firstTarget.Pos);
                TileManager.Ins.MarkPieceInRange(Maker, !_firstTarget.Color, PieceOn(Maker).AttackRange());
                return;
            }

            _secondTarget = hovering;
            TileManager.Ins.UnmarkAll();
            
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
        }

        public void CompleteActionForAI()
        {
            throw new NotImplementedException();
        }

        private static void ResetTargets()
        {
            _firstTarget = null;
            _secondTarget = null;
        }
        
        public void Dispose()
        {
            ResetTargets();
            BoardViewer.SelectingFunction = 0;
        }

        
    }
}

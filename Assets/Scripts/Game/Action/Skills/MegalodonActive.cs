using System;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Common;
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

        public static PieceLogic FirstTarget;
        public static PieceLogic SecondTarget;
        private bool Color;
        public MegalodonActive(int maker, int to, bool color) : base(maker)
        {
            Color = color;
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);

            if (FirstTarget == null || FirstTarget.Color == hovering.Color)
            {
                FirstTarget = hovering;
                TileManager.Ins.UnmarkAll();
                TileManager.Ins.Select(FirstTarget.Pos);
                TileManager.Ins.MarkPieceInRange(Maker, !FirstTarget.Color, PieceOn(Maker).AttackRange);
                return;
            }

            SecondTarget = hovering;
            TileManager.Ins.UnmarkAll();
            
            ActionManager.ExecuteImmediately(new KillPiece(FirstTarget.Pos));
            ActionManager.ExecuteImmediately(new KillPiece(SecondTarget.Pos));
            
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            ResetTargets();
        }

        public void CompleteActionForAI()
        {
            throw new NotImplementedException();
        }

        private static void ResetTargets()
        {
            FirstTarget = null;
            SecondTarget = null;
        }
        
        public void Dispose()
        {
            ResetTargets();
            BoardViewer.SelectingFunction = 0;
        }

        
    }
}

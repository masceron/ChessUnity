using System;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.AI;
using Game.Effects.Buffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SpinsterWrasseActive : Action, ISkills, IPendingAble, IDisposable, IAIAction
    {
        public static PieceLogic FirstTarget;
        public static PieceLogic SecondTarget;
        private bool Color;
        public SpinsterWrasseActive(int maker, int to, bool color) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
            Color = color;
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
                TileManager.Ins.MarkPieceInRange(Maker, FirstTarget.Color, 5);
                return;
            } 
            
            SecondTarget = hovering;
            TileManager.Ins.UnmarkAll();

            ActionManager.ExecuteImmediately(new Purify(Maker, FirstTarget.Pos));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Adaptation(SecondTarget)));
            
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            ResetTargets();
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
        public void CompleteActionForAI()
        {
            throw new NotImplementedException();
        }
        
        
    }
}
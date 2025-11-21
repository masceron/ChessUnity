using System;
using System.Collections.Generic;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using UnityEngine.EventSystems;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MegalodonActive: Action, ISkills, IPendingAble, IDisposable
    {
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
            var hovering = BoardUtils.PieceOn(BoardViewer.HoveringPos);

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

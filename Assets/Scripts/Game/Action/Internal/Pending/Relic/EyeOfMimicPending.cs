using Game.Common;
using Game.Effects.Others;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UnityEngine;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class EyeOfMimicPending : Action, IPendingAble, System.IDisposable, IRelicAction
    {
        public static PieceLogic FirstTarget;
        public static PieceLogic SecondTarget;
        private EyeOfMimic eyeOfMimic;
        public EyeOfMimicPending(EyeOfMimic e, int maker, bool pos = false) : base(maker)
        {
            eyeOfMimic = e;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
        {
            var hovering = BoardUtils.PieceOn(BoardViewer.HoveringPos);

            if (FirstTarget == null || FirstTarget.Color == hovering.Color)
            {
                FirstTarget = hovering;
                TileManager.Ins.MarkIfDifferntColor(FirstTarget.Color);
                TileManager.Ins.Select(FirstTarget.Pos);
                return;
            }

            SecondTarget = hovering;
            Debug.Log(SecondTarget.Type);
            BoardViewer.Ins.ExecuteAction(this);
        }

        private static void ResetTargets()
        {
            FirstTarget = null;
            SecondTarget = null;
        }

        public void Dispose()
        {
            ResetTargets();
            eyeOfMimic = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {
            var ourSide = BoardUtils.OurSide();
            var source = FirstTarget.Color == ourSide ? FirstTarget : SecondTarget;
            var target = FirstTarget.Color == ourSide ? SecondTarget : FirstTarget;
            ActionManager.EnqueueAction(new ApplyEffect(new CopyCapturesMethod(source, target, 0), eyeOfMimic));
            ResetTargets();
            eyeOfMimic.SetCooldown();
            TileManager.Ins.UnmarkAll();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }
    }

}

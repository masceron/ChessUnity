
using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Common;
using Game.Managers;
using UX.UI.Ingame;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Relics.MangroveCharm
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class MangroveCharmPending : Action.Action, IPendingAble, System.IDisposable
    {
        public static PieceLogic FirstTarget;
        public static PieceLogic SecondTarget;
        private MangroveCharm mangroveCharm;
        public MangroveCharmPending(MangroveCharm e, int maker, bool pos = false) : base(maker, pos)
        {
            mangroveCharm = e;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
        {
            var hovering = BoardUtils.PieceOn(BoardViewer.HoveringPos);
            if (FirstTarget == null) 
            {
                FirstTarget = hovering;
                TileManager.Ins.UnmarkAll();
                TileManager.Ins.MarkNextEachPiece(FirstTarget.Color, FirstTarget.Pos);
                TileManager.Ins.Select(FirstTarget.Pos);
                return;
            }
            SecondTarget = hovering;
            TileManager.Ins.UnmarkAll();
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(FirstTarget)));
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(SecondTarget)));
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            mangroveCharm.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic();

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

            mangroveCharm = null;

            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {
        }
    }

}

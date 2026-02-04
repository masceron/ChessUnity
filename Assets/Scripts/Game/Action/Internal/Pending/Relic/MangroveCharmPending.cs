using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class MangroveCharmPending : PendingAction, System.IDisposable, IRelicAction
    {
        public static PieceLogic FirstTarget;
        public static PieceLogic SecondTarget;
        private bool Color;
        private MangroveCharm mangroveCharm;
        public MangroveCharmPending(MangroveCharm e, int Target, bool color) : base(-1)
        {
            mangroveCharm = e;
            Target = (ushort)Target;
            Maker = (ushort)Target;
            Color = color;
        }

        public override void CompleteAction()
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
            BoardViewer.Ins.ExecuteAction(new MangroveCharmExcute((ushort)FirstTarget.Pos, (ushort)SecondTarget.Pos));

            mangroveCharm.SetCooldown();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
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

        // protected override void ModifyGameState()
        // {
        //     var hovering = BoardUtils.PieceOn(BoardViewer.HoveringPos);
        //     if (FirstTarget == null) 
        //     {
        //         FirstTarget = hovering;
        //         TileManager.Ins.UnmarkAll();
        //         TileManager.Ins.MarkNextEachPiece(FirstTarget.Color, FirstTarget.Pos);
        //         TileManager.Ins.Select(FirstTarget.Pos);
        //         return;
        //     }
        //     SecondTarget = hovering;
        //     TileManager.Ins.UnmarkAll();
        //     ActionManager.EnqueueAction(new ApplyEffect(new Shield(FirstTarget), mangroveCharm));
        //     ActionManager.EnqueueAction(new ApplyEffect(new Shield(SecondTarget), mangroveCharm));
        //     BoardViewer.Selecting = -1;
        //     BoardViewer.SelectingFunction = 0;

        //     mangroveCharm.SetCooldown();
        //     MatchManager.Ins.InputProcessor.UpdateRelic();

        //     ResetTargets();
        // }

    }

}

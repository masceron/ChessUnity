using Game.Action.Relics;
using Game.Common;
using Game.Managers;
using Game.Piece;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    public class CoralTomePending : Action, System.IDisposable, IRelicAction
    {
        private CoralTome coralTome;
        private string pieceType;
        public CoralTomePending(CoralTome ct, int maker, string type, bool pos = false) : base(maker)
        {
            coralTome = ct;
            pieceType = type;
            Target = (ushort)maker;
            Maker = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(pieceType, MatchManager.Ins.GameState.OurSide , (ushort)Target)));

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            coralTome.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

        public void CompleteAction()
        {
            
        }

        public void Dispose()
        {
            coralTome = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}
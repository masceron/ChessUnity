using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    public class CoralTomePending : PendingAction, System.IDisposable
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

        // protected override void ModifyGameState()
        // {
        //     
        // }

        public override void CompleteAction()
        {
            BoardViewer.Ins.ExecuteAction(new CoralTomeAction(coralTome.Color, pieceType, Maker));
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            coralTome.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

        public void Dispose()
        {
            coralTome = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}
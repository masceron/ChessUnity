using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using UX.UI.Ingame;

namespace Game.Relics.StormCapacitor
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class StormCapacitorPending : Action.Action, IPendingAble, System.IDisposable
    {
        private readonly Tile.Tile thisTile;
        
        private readonly int size;
        private StormCapacitor stormCapacitor;
        public StormCapacitorPending(int maker, Tile.Tile hoveringTile, StormCapacitor sc, int size) : base(maker)
        {
            thisTile = hoveringTile;
            Maker = (ushort)maker;
            stormCapacitor = sc;
            this.size = size;
        }

        public void CompleteAction()
        {
            var pieces = BoardUtils.GetPiecesInSize(thisTile.rank, thisTile.file, size, thisTile.corner, _ => true);
            
            foreach (var piece in pieces)
            {
                if (piece == null || piece.Color == stormCapacitor.Color) continue;
                ActionManager.ExecuteImmediately(new ApplyEffect(new Stunned(2, piece)));

            }

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            stormCapacitor.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();

            Dispose();
        }

        public void Dispose()
        {
            BoardViewer.SelectingFunction = 0;
            stormCapacitor = null;

            Tile.Tile.OnPointEnterHandle = null;
        }

        protected override void ModifyGameState()
        {
            throw new System.NotImplementedException();
        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
        }
    }
}


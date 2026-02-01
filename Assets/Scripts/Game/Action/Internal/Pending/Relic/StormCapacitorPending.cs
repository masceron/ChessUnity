using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;
using Game.Action.Internal.Pending;
using Game.Action.Internal;
using Game.Tile;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StormCapacitorPending : PendingAction, System.IDisposable, IRelicAction, IInternal
    {
        private readonly Tile.Tile thisTile;
        private readonly int size;
        private StormCapacitor stormCapacitor;
        
        public StormCapacitorPending(int maker, Tile.Tile hoveringTile, StormCapacitor sc, int size) : base(maker)
        {
            thisTile = hoveringTile;
            Target = (ushort)maker;
            Maker = (ushort)maker;
            stormCapacitor = sc;
            this.size = size;
        }

        public override void CompleteAction()
        {
            if (stormCapacitor != null)
            {
                stormCapacitor.SetCooldown();
            }
            
            var excute = new StormCapacitorExcute(
                thisTile.rank, 
                thisTile.file, 
                size, 
                thisTile.corner, 
                stormCapacitor.Color
            );
            BoardViewer.Ins.ExecuteAction(excute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
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
    }
}


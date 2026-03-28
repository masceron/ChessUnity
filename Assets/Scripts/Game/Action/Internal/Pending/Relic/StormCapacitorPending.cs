using System;
using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StormCapacitorPending : PendingAction, IDisposable, IRelicAction
    {
        private readonly int _size;
        private readonly Tile.Tile _thisTile;
        private StormCapacitor _stormCapacitor;

        public StormCapacitorPending(int maker, Tile.Tile hoveringTile, StormCapacitor sc, int size) : base(maker)
        {
            _thisTile = hoveringTile;
            _stormCapacitor = sc;
            _size = size;
        }

        public void Dispose()
        {
            BoardViewer.SelectingFunction = 0;
            _stormCapacitor = null;
            Tile.Tile.OnPointEnterHandle = null;
        }

        protected override void CompleteAction()
        {
            _stormCapacitor?.SetCooldown();

            var execute = new StormCapacitorExecute(
                _thisTile.rank,
                _thisTile.file,
                _size,
                _thisTile.corner,
                _stormCapacitor is { Color: true }
            );
            CommitResult(execute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }
    }
}
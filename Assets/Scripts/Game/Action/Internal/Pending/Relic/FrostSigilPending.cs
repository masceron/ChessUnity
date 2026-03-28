using System;
using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrostSigilPending : PendingAction, IDisposable
    {
        private FrostSigil _frostSigil;

        public FrostSigilPending(int maker, FrostSigil fs) : base(maker)
        {
            _frostSigil = fs;
        }

        public void Dispose()
        {
            _frostSigil = null;
            Tile.Tile.OnPointEnterHandle = null;
        }

        protected override void CompleteAction()
        {
            _frostSigil.SetCooldown();

            var execute = new FrostSigilExecute(GetFrom(), _frostSigil.Color);
            CommitResult(execute);
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            TileManager.Ins.UnmarkAll();
            MatchManager.Ins.InputProcessor.UpdateRelic();

            Dispose();
        }
    }
}
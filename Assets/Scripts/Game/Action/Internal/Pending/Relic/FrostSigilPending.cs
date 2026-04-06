using System;
using Game.Action.Relics;
using Game.Action.Skills;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrostSigilPending : PendingAction, IDisposable, ILocaltionTarget
    {
        private FrostSigil _frostSigil;

        public FrostSigilPending(int target, FrostSigil fs) : base(fs.CommanderPiece, target)
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

            var execute = new FrostSigilExecute(GetMakerAsPiece(), Target, _frostSigil.Color);
            CommitResult(execute);
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            TileManager.Ins.UnmarkAll();
            MatchManager.Ins.InputProcessor.UpdateRelic();

            Dispose();
        }
    }
}
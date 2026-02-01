using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class FrostSigilPending : PendingAction, System.IDisposable
    {
        private FrostSigil frostSigil;
        public FrostSigilPending(int maker, FrostSigil fs) : base(maker)
        {
            Maker = (ushort)maker;
            frostSigil = fs;
        }

        public override void CompleteAction()
        {
            frostSigil.SetCooldown();

            var excute = new FrostSigilExcute(Maker, frostSigil.Color);
            BoardViewer.Ins.ExecuteAction(excute);
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            TileManager.Ins.UnmarkAll();
            MatchManager.Ins.InputProcessor.UpdateRelic();

            Dispose();
        }

        public void Dispose()
        {
            frostSigil = null;
            Tile.Tile.OnPointEnterHandle = null;
        }
    }
}


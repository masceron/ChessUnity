// Assets/Scripts/Game/Action/Internal/Pending/Relic/SeafoamPhialPending.cs

using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RayStingerPending : PendingAction, System.IDisposable
    {
        private RayStinger _rayStinger;

        public RayStingerPending(RayStinger seafoamPhial, int target) : base(target)
        {
            _rayStinger = seafoamPhial;
            Target = (ushort)target;
        }

        protected override void CompleteAction()
        {
            _rayStinger.SetCooldown();

            var execute = new RayStingerExecute(Target);
            CommitResult(execute);
            TileManager.Ins.UnmarkAll();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }
        public void Dispose()
        {
            _rayStinger = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}

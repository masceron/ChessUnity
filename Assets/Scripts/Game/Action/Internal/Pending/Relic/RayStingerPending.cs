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
        private RayStinger rayStinger;

        public RayStingerPending(RayStinger seafoamPhial, int target) : base(target)
        {
            rayStinger = seafoamPhial;
            Target = (ushort)target;
        }

        public override void CompleteAction()
        {
            rayStinger.SetCooldown();

            var excute = new RayStingerExcute(Target);
            BoardViewer.Ins.ExecuteAction(excute);
            TileManager.Ins.UnmarkAll();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }
        public void Dispose()
        {
            rayStinger = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}

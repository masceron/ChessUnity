// Assets/Scripts/Game/Action/Internal/Pending/Relic/SeafoamPhialPending.cs

using Game.Effects.Buffs;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
using Game.Action.Relics;
using Game.Effects.Debuffs;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RayStingerActive : Action, System.IDisposable, IRelicAction
    {
        private RayStinger rayStinger;

        public RayStingerActive(RayStinger seafoamPhial, int maker) : base(maker)
        {
            this.rayStinger = seafoamPhial;
            Maker = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(3, PieceOn(Maker))));
            ActionManager.EnqueueAction(new ApplyEffect(new Broken(2, PieceOn(Maker))));
            rayStinger.SetCooldown();
        }
        public void Dispose()
        {
            rayStinger = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}

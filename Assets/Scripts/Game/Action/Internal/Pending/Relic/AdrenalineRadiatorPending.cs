// Assets/Scripts/Game/Action/Internal/Pending/Relic/SeafoamPhialPending.cs

using Game.Effects.Buffs;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AdrenalineRadiatorPending : Action, System.IDisposable, IRelicAction
    {
        private AdrenalineRadiator relic;

        public AdrenalineRadiatorPending(AdrenalineRadiator relic, int maker) : base(maker)
        {
            this.relic = relic;
            Maker = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            PieceOn(Maker).SkillCooldown = 0;
            
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            relic.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }

        public void Dispose()
        {
            relic = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}

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
    public class SeafoamPhialPending : PendingAction, System.IDisposable, IRelicAction
    {
        private SeafoamPhial seafoamPhial;

        public SeafoamPhialPending(SeafoamPhial seafoamPhial, int maker, bool pos) : base(maker)
        {
            this.seafoamPhial = seafoamPhial;
            Maker = (ushort)maker;
        }

        public override void CompleteAction()
        {
            // ActionManager.EnqueueAction(new Purify(Maker, Maker));
            // ActionManager.EnqueueAction(new ApplyEffect(new Haste(3, 1, PieceOn(Maker))));
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            
            seafoamPhial.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }

        

        // protected override void ModifyGameState()
        // {
        //     ActionManager.EnqueueAction(new Purify(Maker, Maker));
        //     ActionManager.EnqueueAction(new ApplyEffect(new Haste(3, 1, PieceOn(Maker)), seafoamPhial));
            // BoardViewer.Selecting = -1;
            // BoardViewer.SelectingFunction = 0;
            //
            // seafoamPhial.SetCooldown();
            // MatchManager.Ins.InputProcessor.UpdateRelic(); 
        // }

        public void Dispose()
        {
            seafoamPhial = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}

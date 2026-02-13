// Assets/Scripts/Game/Action/Internal/Pending/Relic/SeafoamPhialPending.cs

using System;
using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeafoamPhialPending : PendingAction, IDisposable
    {
        private SeafoamPhial _seafoamPhial;

        public SeafoamPhialPending(SeafoamPhial seafoamPhial, int maker) : base(maker)
        {
            _seafoamPhial = seafoamPhial;
            Maker = maker;
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
            _seafoamPhial = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            CommitResult(new SeafoamPhialAction(Maker));
            _seafoamPhial.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }
    }
}
// Assets/Scripts/Game/Action/Internal/Pending/Relic/SeafoamPhialPending.cs

using System;
using Game.Action.Relics;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RayStingerPending : PendingAction, IDisposable
    {
        private RayStinger _rayStinger;

        public RayStingerPending(RayStinger seafoamPhial, PieceLogic target) : base(null, target)
        {
            _rayStinger = seafoamPhial;
        }

        public void Dispose()
        {
            _rayStinger = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            _rayStinger.SetCooldown();

            var execute = new RayStingerExecute(GetTargetPos());
            CommitResult(execute);
            TileManager.Ins.UnmarkAll();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }
    }
}
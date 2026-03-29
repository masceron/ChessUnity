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
    public class MethaneCasingPending : PendingAction, IDisposable
    {
        private MethaneCasing _methaneCasing;

        public MethaneCasingPending(MethaneCasing methaneCasing, PieceLogic target) : base(null, target)
        {
            _methaneCasing = methaneCasing;
        }

        public void Dispose()
        {
            _methaneCasing = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            _methaneCasing.SetCooldown();

            var execute = new MethaneCasingExecute(GetTargetPos());
            CommitResult(execute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            BoardViewer.Ins.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }
    }
}
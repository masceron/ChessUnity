using System;
using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    public class CoralTomePending : PendingAction, IDisposable
    {
        private readonly string _pieceType;
        private CoralTome _coralTome;

        public CoralTomePending(CoralTome ct, int maker, string type) : base(maker)
        {
            _coralTome = ct;
            _pieceType = type;
            Target = maker;
            Maker = maker;
        }

        public void Dispose()
        {
            _coralTome = null;
            BoardViewer.SelectingFunction = 0;
        }

        // protected override void ModifyGameState()
        // {
        //     
        // }

        protected override void CompleteAction()
        {
            CommitResult(new CoralTomeAction(_coralTome.Color, _pieceType, Maker));
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            _coralTome.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }
    }
}
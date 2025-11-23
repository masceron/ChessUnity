using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class FrostSigilPending : Action, IPendingAble, System.IDisposable
    {
        private Tile.Tile thisTile;

        private int probabilityBound = 25;

        private FrostSigil frostSigil;
        public FrostSigilPending(int maker, Tile.Tile hoveringTile, FrostSigil fs) : base(maker)
        {
            thisTile = hoveringTile;
            Maker = (ushort)maker;
            frostSigil = fs;
        }

        public void CompleteAction()
        {
            var pieces = BoardUtils.GetPiecesInRadius(thisTile.rank, thisTile.file, 1, _ => true);

            foreach (var piece in pieces)
            {
                if (piece == null || piece.Color == MatchManager.Ins.GameState.OurSide) continue;

                ActionManager.ExecuteImmediately(new ApplyEffect(new Slow(3, 1, piece)));

                if (MatchManager.Roll(probabilityBound))
                {
                    ActionManager.ExecuteImmediately(new ApplyEffect(new Bound(3, piece)));
                }
            }

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            frostSigil.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();

            Dispose();
        }

        public void Dispose()
        {
            BoardViewer.SelectingFunction = 0;
            frostSigil = null;

            Tile.Tile.OnPointEnterHandle = null;
        }

        protected override void ModifyGameState()
        {
            throw new System.NotImplementedException();
        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
        }
    }
}


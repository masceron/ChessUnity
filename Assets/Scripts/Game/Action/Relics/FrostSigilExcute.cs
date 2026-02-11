using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;

namespace Game.Action.Relics
{

    public class FrostSigilExcute : Action, IRelicAction
    {
        private readonly int radius = 3;
        private readonly bool ourSide;
        private readonly int probabilityBound = 25;

        public FrostSigilExcute(int maker, bool ourSide) : base(maker)
        {
            this.ourSide = ourSide;
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = BoardUtils.RankFileOf(Maker);
            var pieces = BoardUtils.GetPiecesInRadius(rank, file, radius, _ => true);

            foreach (var piece in pieces)
            {
                if (piece == null || piece.Color == ourSide) continue;

                ActionManager.EnqueueAction(new ApplyEffect(new Slow(3, 1, piece)));

                if (MatchManager.Roll(probabilityBound))
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Bound(3, piece)));
                }
            }
        }
    }
}

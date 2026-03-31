using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class FrostSigilExecute : Action, IRelicAction
    {
        private const int Radius = 3;

        private const int ProbabilityBound = 25;

        [MemoryPackInclude] private bool _ourSide;

        [MemoryPackConstructor]
        private FrostSigilExecute()
        {
        }

        public FrostSigilExecute(int maker, bool ourSide) : base(null, maker)
        {
            _ourSide = ourSide;
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = BoardUtils.RankFileOf(GetFrom());
            var pieces = BoardUtils.GetPiecesInRadius(rank, file, Radius, _ => true);

            foreach (var piece in pieces)
            {
                if (piece == null || piece.Color == _ourSide) continue;

                ActionManager.EnqueueAction(new ApplyEffect(new Slow(3, 1, piece)));

                if (MatchManager.Roll(ProbabilityBound))
                    ActionManager.EnqueueAction(new ApplyEffect(new Bound(3, piece)));
            }
        }
    }
}
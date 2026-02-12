using MemoryPack;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;

namespace Game.Action.Relics
{

    [MemoryPackable]
    public partial class FrostSigilExcute : Action, IRelicAction
    {
        private const int Radius = 3;
        
        [MemoryPackInclude]
        private readonly bool _ourSide;
        
        private const int ProbabilityBound = 25;

        public FrostSigilExcute(int maker, bool ourSide) : base(maker)
        {
            this._ourSide = ourSide;
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = BoardUtils.RankFileOf(Maker);
            var pieces = BoardUtils.GetPiecesInRadius(rank, file, Radius, _ => true);

            foreach (var piece in pieces)
            {
                if (piece == null || piece.Color == _ourSide) continue;

                ActionManager.EnqueueAction(new ApplyEffect(new Slow(3, 1, piece)));

                if (MatchManager.Roll(ProbabilityBound))
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Bound(3, piece)));
                }
            }
        }
    }
}

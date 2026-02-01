using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;


namespace Game.Piece.PieceLogic
{
    public class DuskyButterflyfish : Commons.PieceLogic
    {
        public DuskyButterflyfish(PieceConfig cfg) : base(cfg, PufferfishMoves.Quiets, PufferfishMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Extremophile(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new DuskyButterflyfishPassive(this)));
        }
    }
}


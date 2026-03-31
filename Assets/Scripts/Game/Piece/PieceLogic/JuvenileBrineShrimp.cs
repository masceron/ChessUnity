using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class JuvenileBrineShrimp : Commons.PieceLogic
    {
        private const int Strength1 = 1;
        private const int Strength2 = 1;
        private const int Duration1 = 10;
        private const int Duration2 = 10;
        public JuvenileBrineShrimp(PieceConfig cfg) : base(cfg, SmallChargingMoves.Quiets, None.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Haste(Duration1, Strength1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new LongReach(this, Duration2, Strength2)));
        }
    }
}
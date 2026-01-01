using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.SpecialAbility;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class PegasusSeamoth : Commons.PieceLogic
    {
        public PegasusSeamoth(PieceConfig cfg) : base(cfg, FlyingFishMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Camouflage(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new PegasusSeamothPassive(this)));
        }
    }
}
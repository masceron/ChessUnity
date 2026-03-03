using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Effects.Buffs;

namespace Game.Piece.PieceLogic
{
    public class BoneEatingWorm : Commons.PieceLogic
    {
        public BoneEatingWorm(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new BoneEatingWormPassive(3, 0, 1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Relentless(this, 1)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new HardenedShield(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Extremophile(this)));
        }
    }
}
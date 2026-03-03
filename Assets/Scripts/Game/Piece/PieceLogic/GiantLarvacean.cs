using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class GiantLarvacean : Commons.PieceLogic
    {
        public GiantLarvacean(PieceConfig cfg) : base(cfg, AmbushPredatorMoves.Quiets, AmbushPredatorMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new GiantLarvaceanPassive(1, 3, 4, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Demolisher(this)));
        }
    }
}
using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class BlueChromis : Commons.PieceLogic
    {
        public BlueChromis(PieceConfig cfg) : base(cfg, BluffingMoves.Quiets, BluffingMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new BlueChromisPassive(this)));
        }
    }
}
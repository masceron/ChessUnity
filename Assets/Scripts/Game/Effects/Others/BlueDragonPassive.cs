using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;

namespace Game.Effects.Others
{
    public class BlueDragonPassive : Effect
    {
        public BlueDragonPassive(PieceLogic piece) : base(-1, 1, piece, EffectName.BlueDragonPassive)
        {
        }

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action.Result == ActionResult.Succeed)
            {
                ActionManager.ExecuteImmediately(new Purify(Piece.Pos, Piece.Pos));
            }
        }
    }
}
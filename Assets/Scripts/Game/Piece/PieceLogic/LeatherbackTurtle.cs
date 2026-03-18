using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class LeatherbackTurtle : Commons.PieceLogic
    {
        public LeatherbackTurtle(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, RookMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Regenarative(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Consume(this)));
        }   
    }
}
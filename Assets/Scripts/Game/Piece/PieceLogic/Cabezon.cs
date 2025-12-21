using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.SpecialAbility;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class Cabezon : Commons.PieceLogic
    {
        public Cabezon(PieceConfig cfg) : base(cfg, FrontDefenderMoves.Quiets, FrontDefenderMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Shield(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new CabezonPassive(this)));
        }
        
    }
}
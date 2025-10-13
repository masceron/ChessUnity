using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Swarm
{
    public class TigerPrawn : PieceLogic
    {
        public TigerPrawn(PieceConfig cfg) : base(cfg, SmallChargingMoves.Quiets, SmallChargingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Extremophile(this)));
        }

        protected override void CustomBehaviors(List<Action.Action> list)
        {
            
        }
    }
}
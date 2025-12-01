using Game.Action;
using Game.Action.Internal;
using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    public class HorseLeechPassive : Effect
    {

        public HorseLeechPassive(PieceLogic piece) : base(-1, 1, piece, "effect_horse_leech_passive")
        { }

        public override void OnCallPieceAction(Action.Action action)
        {
            
        }
        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 0;
        }

    }
}
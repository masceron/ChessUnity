using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Piece;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    
    public class BlueRingedOctopusPassive : Effect, IEndTurnEffect, IPendingAble
    {
        
        public EndTurnEffectType EndTurnEffectType { get; }

        
        public BlueRingedOctopusPassive(PieceLogic piece) : base(-1, 1, piece, EffectName.LivingCoralPassive)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }
        private void SummonPiece()
        {
            
        }
        
        public void OnCallEnd(Action.Action action)
        {


        }

        public void CompleteAction()
        {
            throw new System.NotImplementedException();
        }
    }
}
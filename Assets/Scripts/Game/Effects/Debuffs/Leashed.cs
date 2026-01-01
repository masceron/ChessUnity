using Game.Piece.PieceLogic.Commons;
using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Leashed: Effect, IOnMoveGenEffect
    {
        public readonly int Position;
        public Leashed(PieceLogic piece, int position, sbyte duration) : base(duration, 1, piece, "effect_leashed")
        {
            Position = position;
        }

        public void OnCallMoveGen(List<Action.Action> actions)
        {
            if (actions == null || actions.Count == 0) return;
            
            actions.RemoveAll(action => 
                action.Maker == Piece.Pos && 
                Distance(action.Target, Position) > 3
            );
        }
        
        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 20;
        }
    
    }
}
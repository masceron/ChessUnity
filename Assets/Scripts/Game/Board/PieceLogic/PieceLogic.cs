using System.Collections.Generic;
using Game.Board.Effects;
using Game.Board.General;

namespace Game.Board.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class PieceLogic
    {
        public ushort Pos;
        public Color Color;
        public sbyte MoveRange;
        public sbyte AttackRange;
        
        public PieceRank Rank;
       
        public readonly List<Effect> Effects;

        protected PieceLogic(Color color, ushort pos, sbyte moveRange, sbyte attackRange, PieceRank rank)
        {
            Color = color;
            Pos = pos;
            Effects = new List<Effect>();
            MoveRange = moveRange;
            AttackRange = attackRange; 
            Rank = rank;
        }

        public virtual void PassTurn()
        {
            
        }
        public abstract List<Action.Action> MoveToMake();
    }
}
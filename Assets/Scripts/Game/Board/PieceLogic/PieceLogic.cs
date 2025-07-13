using System;
using System.Collections.Generic;
using Game.Board.Effects;
using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false), Serializable]
    public abstract class PieceLogic
    {
        public ushort pos;
        public Color color;
        public sbyte moveRange;
        public sbyte attackRange;
        
        public PieceRank pieceRank;
       
        public readonly List<Effect> Effects;

        protected PieceLogic(PieceConfig cfg)
        {
            color = cfg.Color;
            pos = cfg.Index;
            Effects = new List<Effect>();

            var info = MatchManager.AssetManager.PieceData[cfg.Type];
            moveRange = info.moveRange;
            attackRange = info.attackRange; 
            pieceRank = info.rank;
        }

        public virtual void PassTurn()
        {
            
        }
        public abstract List<Action.Action> MoveToMake();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Game.Board.Effects;
using Game.Board.Effects.Debuffs;
using Game.Board.General;
using Color = Game.Board.General.Color;

namespace Game.Board.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false), Serializable]
    public abstract class PieceLogic
    {
        public ushort pos;
        public Color color;

        public sbyte trueMoveRange;
        public sbyte effectiveMoveRange;
        public sbyte attackRange;
        
        public PieceRank pieceRank;
       
        public readonly List<Effect> Effects;
        public PieceType type;

        protected PieceLogic(PieceConfig cfg)
        {
            color = cfg.Color;
            pos = cfg.Index;
            Effects = new List<Effect>();
            type = cfg.Type;

            var info = MatchManager.assetManager.PieceData[cfg.Type];
            effectiveMoveRange = info.moveRange;
            trueMoveRange = info.moveRange;
            attackRange = info.attackRange; 
            pieceRank = info.rank;
        }

        public virtual void PassTurn()
        {
            
        }
        protected abstract List<Action.Action> MoveToMake();

        public List<Action.Action> MoveList()
        {
            return Effects.OfType<Stunned>().Any() ? new List<Action.Action>() : MoveToMake();
        }
    }
}
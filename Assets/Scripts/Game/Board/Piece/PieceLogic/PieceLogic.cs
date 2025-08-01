using System.Collections.Generic;
using Game.Board.Effects;
using Game.Board.General;
using Color = Game.Board.General.Color;

namespace Game.Board.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class PieceLogic
    {
        public ushort Pos;
        public Color Color;

        public sbyte TrueMoveRange;
        public sbyte EffectiveMoveRange;
        public sbyte AttackRange;
        public sbyte SkillCooldown;
        
        public readonly PieceRank PieceRank;
       
        public readonly List<Effect> Effects;
        public readonly PieceType Type;

        protected PieceLogic(PieceConfig cfg)
        {
            Color = cfg.Color;
            Pos = cfg.Index;
            Effects = new List<Effect>();
            Type = cfg.Type;

            var info = AssetManager.Ins.PieceData[cfg.Type];
            EffectiveMoveRange = info.moveRange;
            TrueMoveRange = info.moveRange;
            AttackRange = info.attackRange; 
            PieceRank = info.rank;
        }

        public void PassTurn()
        {
            var gameState = MatchManager.Ins.GameState;
            if (SkillCooldown > 0 && gameState.SideToMove != gameState.OurSide) SkillCooldown--;
        }
        protected abstract List<Action.Action> MoveToMake();

        public List<Action.Action> MoveList()
        {
            var list = MoveToMake();
            EventObserver.Notify(list);
            return list;
        }
    }
}
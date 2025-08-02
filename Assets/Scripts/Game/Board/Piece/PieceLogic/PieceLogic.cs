using System.Collections.Generic;
using Game.Board.Effects;
using Game.Board.General;
using static Game.Common.BoardUtils;

namespace Game.Board.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class PieceLogic
    {
        public ushort Pos;
        public bool Color;

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

            if (this is IPieceWithSkill pieceWithSkill)
            {
                SkillCooldown = 0;
                pieceWithSkill.TimeToCooldown = (sbyte)(info.normalSkillCooldown + 1);
            }
            else SkillCooldown = -1;
        }

        public void PassTurn()
        {
            if (SkillCooldown > 0 && SideToMove() != OurSide()) SkillCooldown--;
        }
        protected abstract List<Action.Action> MoveToMake();

        public List<Action.Action> MoveList()
        {
            var list = MoveToMake();
            NotifyOnMoveGen(list);
            return list;
        }
    }
}
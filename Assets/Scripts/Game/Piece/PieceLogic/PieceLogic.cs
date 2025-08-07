using System;
using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Data.Pieces;
using Game.Effects;
using Game.Managers;
using Game.Moves;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class PieceLogic
    {
        public ushort Pos;
        public bool Color;

        public sbyte MoveRange;
        public sbyte AttackRange;
        public sbyte SkillCooldown;
        
        public readonly PieceRank PieceRank;
       
        public readonly List<Effect> Effects;
        public readonly PieceType Type;

        protected PieceLogic(PieceConfig cfg, QuietsDelegate quiets = null, CapturesDelegate captures = null)
        {
            Color = cfg.Color;
            Pos = cfg.Index;
            Effects = new List<Effect>();
            Type = cfg.Type;

            var info = AssetManager.Ins.PieceData[cfg.Type];
            MoveRange = info.moveRange;
            AttackRange = info.attackRange;
            PieceRank = info.rank;

            if (this is IPieceWithSkill pieceWithSkill)
            {
                SkillCooldown = 0;
                pieceWithSkill.TimeToCooldown = (sbyte)(info.normalSkillCooldown != -1 ? info.normalSkillCooldown + 1 : -1);
            }
            else SkillCooldown = -1;
            
            Quiets = quiets;
            Captures = captures;
        }

        public void PassTurn()
        {
            if (SkillCooldown > 0) SkillCooldown--;
        }
        protected abstract void MoveToMake(List<Action.Action> list);

        public QuietsDelegate Quiets;
        protected readonly CapturesDelegate Captures;

        public void MoveList(List<Action.Action> list)
        {
            if (Effects.Any(e => e.EffectName == EffectName.Stunned)) return;
            
            MoveToMake(list);
            if (Quiets.GetInvocationList().GetLength(0) > 1)
            {
                list = list.Distinct(new ActionComparer()).ToList();
            }
            NotifyOnMoveGen(list);
        }

        public int GetMoveRange()
        {
            if (Effects.Any(e => e.EffectName == EffectName.Bound)) return 0;

            Effect slow;
            if ((slow = Effects.Find(e => e.EffectName == EffectName.Slow)) != null)
            {
                return Math.Max(MoveRange - slow.Strength, 1);
            }


            return MoveRange;
        }
    }
}
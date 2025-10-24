using System;
using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Effects;
using Game.Managers;
using Game.Movesets;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class PieceLogic
    {
        public ushort Pos;
        public bool Color;

        public readonly List<byte> MoveRange;
        public byte AttackRange;
        public sbyte SkillCooldown;
        public readonly PieceRank PieceRank;
        public readonly List<Effect> Effects;
        public readonly List<int> PreviousMoves;
        public readonly PieceType Type;
        private readonly bool hasSkill;

        private bool dead;
        private bool canUseSkill;
        public bool IsDead()
        {
            return dead;
        }

        public bool CanUseSkill
        {
            get => canUseSkill;
            set => canUseSkill = value;
        }

        public void Die()
        {
            dead = true;
        }

        protected PieceLogic(PieceConfig cfg, QuietsDelegate quiets = null, CapturesDelegate captures = null)
        {
            Color = cfg.Color;
            Pos = cfg.Index;
            Effects = new List<Effect>();
            PreviousMoves = new List<int>();
            Type = cfg.Type;

            var info = AssetManager.Ins.PieceData[cfg.Type];
            MoveRange = new List<byte> { info.moveRange };
            AttackRange = info.attackRange;
            PieceRank = info.rank;
            hasSkill = info.hasSkill;

            if (hasSkill)
            {
                canUseSkill = true;
            }

            if (this is IPieceWithSkill pieceWithSkill)
            {
                SkillCooldown = 0;
                pieceWithSkill.TimeToCooldown = (sbyte)(info.normalSkillCooldown != -1 ? info.normalSkillCooldown + 1 : -1);
            }
            else SkillCooldown = -1;
            
            Quiets = quiets;
            this.Captures = captures;
        }
        public void PassTurn()
        {
            if (SkillCooldown > 0) SkillCooldown--;
        }

        protected virtual void CustomBehaviors(List<Action.Action> list)
        {}

        public QuietsDelegate Quiets;
        public CapturesDelegate Captures;

        /// <summary>
        /// Thêm vào tham số list những Action có thể thực thi. Ví dụ: Moves, Captures, ISkills <br/>
        /// List này sau đó sẽ được hiển thị lên Board bởi BoardViewer
        /// </summary>
        /// <param name="list"></param>
        public void MoveList(List<Action.Action> list)
        {
            if (PieceRank == PieceRank.Construct) return;
            if (Effects.Any(e => e.EffectName == EffectName.Stunned)) return;
            var i = 0;

            Quiets(list, Pos, ref i);
            
            if (MoveRange.Count > 1)
            {
                list = list.Distinct(new ActionComparer()).ToList();
            }
            
            Captures(list, Pos);

            if (hasSkill && canUseSkill)
            {
                ((IPieceWithSkill)this).Skills(list);
            }
            
            CustomBehaviors(list);
            NotifyOnMoveGen(list);
        }

        public int GetMoveRange(ref int index)
        {
            int range = MoveRange[index++];
            if (Effects.Any(e => e.EffectName == EffectName.Bound)) return 0;

            if (range > MaxLength) return range;
            
            Effect movement;
            if ((movement = Effects.Find(e => e.EffectName == EffectName.Slow)) != null)
            {
                range -= movement.Strength;
            }
            if ((movement = Effects.Find(e => e.EffectName == EffectName.Haste)) != null)
            {
                range += movement.Strength;
            }
            
            return Math.Max(1, range);
        }
    }
}
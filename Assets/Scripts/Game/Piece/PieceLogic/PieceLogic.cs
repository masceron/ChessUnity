using System;
using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Augmentation;
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
        public bool IsDead()
        {
            return dead;
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

            if (this is IPieceWithSkill pieceWithSkill)
            {
                SkillCooldown = 0;
                pieceWithSkill.TimeToCooldown = (sbyte)(info.normalSkillCooldown != -1 ? info.normalSkillCooldown + 1 : -1);
            }
            else SkillCooldown = -1;
            
            Quiets = quiets;
            this.Captures = captures;

            if (cfg.AugmentationInfos != null && cfg.AugmentationInfos.Count > 0)
            {
                ApplyAugmentationEffects(cfg.AugmentationInfos);
            }

        }

        private void ApplyAugmentationEffects(List<Augmentation.Augmentation> augmentations)
        {
            // 1️⃣ Thêm các effect passive của từng augmentation
            foreach (var aug in augmentations)
            {
                aug.SetTarget(this);
                if (aug.PassiveEffects == null) continue;
                UnityEngine.Debug.Log($"[Augmentation] {Type} nhận hiệu ứng từ augmentation {aug.Type}");
                Effects.AddRange(aug.PassiveEffects);
            }

            var setCount = new Dictionary<AugmentationSetType, int>();
            foreach (var aug in augmentations)
            {
                if (aug.Set == null || aug.Set.Type == AugmentationSetType.None) continue;

                if (!setCount.ContainsKey(aug.Set.Type))
                    setCount[aug.Set.Type] = 0;

                setCount[aug.Set.Type]++;
            }

            foreach (var kv in setCount)
            {
                var setInfo = augmentations.FirstOrDefault(a => a.Set != null && a.Set.Type == kv.Key)?.Set;
                if (setInfo == null) continue;

                int count = kv.Value;

                if (setInfo.HaveBonus && count >= setInfo.RequiredPieces)
                {
                    UnityEngine.Debug.Log($"[Set Bonus] {Type} nhận bonus từ set {setInfo.Type} (x{count})");

                    if (setInfo.SetEffects != null)
                        Effects.AddRange(setInfo.SetEffects);
                }
            }
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

            if (hasSkill && !Effects.Any(e => e.EffectName == EffectName.Silenced))
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
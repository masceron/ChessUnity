using System;
using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Managers;
using Game.Movesets;
using Game.ScriptableObjects;
using Game.Tile;
using static Game.Common.BoardUtils;
using static Game.ScriptableObjects.PieceInfo;

namespace Game.Piece.PieceLogic.Commons
{

    // miễn nhiễm với Formation mang debuff
    // miễn nhiễm với Formation cụ thể nào đó   
    public enum ImmunityType
    {
        None,
        FormationDebuff,
        FormationSpecific,
    }

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class PieceLogic
    {
        public ushort Pos;
        public bool Color;
        public bool IsVisible = true;
        public readonly List<byte> MoveRange;
        public byte AttackRange;
        public sbyte SkillCooldown;
        public readonly PieceRank PieceRank;
        public readonly List<Effect> Effects;
        private HashSet<String> immuneEffectNames = new HashSet<String>();
        public readonly List<int> PreviousMoves;
        public readonly string Type;
        private readonly bool hasSkill;
        public readonly List<Augmentation.Augmentation> Augmentations;

        private bool dead;

        public readonly List<ImmunityType> Immunities;
        public readonly List<FormationType> SpecificFormations;
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
            Immunities = new List<ImmunityType>();
            SpecificFormations = new List<FormationType>();

            if (this is IPieceWithSkill pieceWithSkill)
            {
                SkillCooldown = 0;
                pieceWithSkill.TimeToCooldown = (sbyte)(info.normalSkillCooldown != -1 ? info.normalSkillCooldown + 1 : -1);
            }
            else SkillCooldown = -1;
            
            Quiets = quiets;
            Captures = captures;

            Augmentations = new List<Augmentation.Augmentation>();
            if (cfg.Augmentations != null)
            {
                Augmentations.AddRange(cfg.Augmentations);
            }

            if (Augmentations is not { Count: > 0 }) return;
            if (ValidAugmentation(Augmentations))
            {
                ApplyAugmentationEffects(Augmentations);
            }

        }

        private bool ValidAugmentation(List<Augmentation.Augmentation> augmentations)
        {
            if (IsDulicatedSlot(augmentations)) return false;

            var pieceInfo = AssetManager.Ins.PieceData[Type];
            foreach (var ag in augmentations)
            {
                if (!CanEquip(pieceInfo, ag)) 
                { 
                    //UnityEngine.Debug.Log($"Can not apply augmentation slot type {ag.Slot}");
                    return false; 
                }
            }
            return true;
        }
        bool IsDulicatedSlot(List<Augmentation.Augmentation> augmentations)
        {
            var slotSet = new HashSet<Augmentation.AugmentationSlot>();
            return augmentations.Any(ag => !slotSet.Add(ag.Slot));
        }
        bool CanEquip(PieceInfo piece, Augmentation.Augmentation augment)
        {
            return piece.availableSlots.HasFlag(
                (AugmentationSlotMask)(1 << (int)augment.Slot)
            );
        }

        private void ApplyAugmentationEffects(List<Augmentation.Augmentation> augmentations)
        {
            // 1️⃣ Thêm các effect passive của từng augmentation
            foreach (var aug in augmentations)
            {
                aug.SetTarget(this);
                if (aug.PassiveEffects == null) continue;
                aug.ApplyPassiveEffects();
            }

            var setCount = new Dictionary<AugmentationSetType, int>();
            foreach (var aug in augmentations)
            {
                if (aug.Set == null || aug.Set.Type == AugmentationSetType.None) continue;

                setCount.TryAdd(aug.Set.Type, 0);

                setCount[aug.Set.Type]++;
            }

            foreach (var kv in setCount)
            {
                var setInfo = augmentations.FirstOrDefault(a => a.Set != null && a.Set.Type == kv.Key)?.Set;
                if (setInfo == null) continue;

                var count = kv.Value;

                if (setInfo.HaveBonus && count >= setInfo.RequiredPieces)
                {
                    //UnityEngine.Debug.Log($"[Set Bonus] {Type} nhận bonus từ set {setInfo.Type} (x{count})");

                    if (setInfo.BonusEffects != null)
                    {
                        setInfo.ApplyBonusEffects();
                    }
                }
            }
        }

        public bool HasAugmentation(Augmentation.AugmentationName augmentationName)
        {
            return Augmentations.Any(a => a.Name == augmentationName);
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
        /// isPlayer là để phân biệt người dùng hay AI
        /// excludeEmptyTile = false thì sẽ lấy cả những ô không có target, mục đích là để lấy enemySnapshot để đánh giá điểm cho action của Maker
        /// excludeEmptyTile = true thì sẽ là logic thông thường, để mark những ô có thể đi/ăn/skill
        /// </summary>
        /// <param name="list"></param>
        public void MoveList(List<Action.Action> list, bool isPlayer = true, bool excludeEmptyTile = true)
        {
            if (PieceRank == PieceRank.Construct) return;
            if (Effects.Any(e => e.EffectName == "effect_stunned")) return;
            if (Effects.Any(e => e.EffectName == "effect_frenzied")) return;
            var i = 0;

            Quiets(list, Pos, ref i, isPlayer);
            
            if (MoveRange.Count > 1)
            {
                list = list.Distinct(new ActionComparer()).ToList();
            }
            
            Captures(list, Pos, excludeEmptyTile);

            if (hasSkill && Effects.All(e => e.EffectName != "effect_silenced") && Effects.All(e => e.EffectName == "effect_illusion"))
            {
                ((IPieceWithSkill)this).Skills(list, isPlayer, excludeEmptyTile);
            }
            
            CustomBehaviors(list);
            NotifyOnMoveGen(list);
        }

        public int GetMoveRange(ref int index)
        {
            int range = MoveRange[index++];
            if (Effects.Any(e => e.EffectName == "effect_bound")) return 0;

            if (range > MaxLength) return range;
            
            /*Effect movement;
            if ((movement = Effects.Find(e => e.EffectName == "effect_slow")) != null)
            {
                range -= movement.Strength;
            }
            if ((movement = Effects.Find(e => e.EffectName == "effect_haste")) != null)
            {
                range += movement.Strength;
            }

            if ((movement = Effects.Find(e => e.EffectName == EffectName.TidalRetinaPassive)) != null)
            {
                range += movement.Strength;
            }*/

            foreach (var e in Effects)
            {
                if (e is IMoveRangeModifier modifier)
                {
                    range = modifier.ModifyMoveRange(range);
                }
            }

            return Math.Max(1, range);
        }

        public bool IsImmuneTo(Formation formation, Effect appliedEffect = null)
        {
            if (formation == null) return false;
            
            foreach (var immunityEffect in Effects)
            {
                if (immunityEffect is IImmunity immunity)
                {
                    if (appliedEffect != null && immunity.CheckImmunity(formation.GetFormationType(), appliedEffect)) {
                        return true;
                    }
                }
            }
            if (Immunities.Contains(ImmunityType.FormationDebuff) && appliedEffect?.Category == EffectCategory.Debuff) {
                return true;
            }
            if (Immunities.Contains(ImmunityType.FormationSpecific) && SpecificFormations.Contains(formation.GetFormationType())) {
                return true;
            }

            return false;
        }

        public bool IsImmuneToEffect(String appliedEffectName)
        {
            if (immuneEffectNames.Contains(appliedEffectName)) return true;
            return false;
        }

        public void ImmuneEffect(String effectName)
        {
            immuneEffectNames.Add(effectName);
        }

        public int GetValueForAI()
        {
            int value = RankToValue(PieceRank);
            value += AssetManager.Ins.PieceData[Type].baseValue;

            foreach (var effect in Effects)
            {
                value += effect.GetValueForAI();
            }

            foreach (var aug in Augmentations)
            {
                value += aug.GetValueForAI();
            }
            
            value += GetQuitesValue();

            return value;
        }

        public int GetQuitesValue()
        {
            int i = 0;
            int value = Quiets(new List<Action.Action>(), Pos, ref i, isPlayer: true);
            return value;
        }

        public int GetCapturesValue()
        {
            return Captures(new List<Action.Action>(), Pos, excludeEmptyTile: false);
        }

        private int RankToValue(PieceRank rank)
        {
            switch (rank)
            {
                case PieceRank.Swarm:     return 50;
                case PieceRank.Common:    return 70;
                case PieceRank.Elite:     return 120;
                case PieceRank.Champion:  return 200;
                case PieceRank.Commander: return 1000;
                case PieceRank.Construct: return 150;
                case PieceRank.Summoned:  return 300;
                default: return 0;
            }
        }

    }
}
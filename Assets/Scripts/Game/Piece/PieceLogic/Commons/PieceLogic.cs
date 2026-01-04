using System;
using System.Collections.Generic;
using System.Linq;
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
        private byte moveRange;
        private byte attackRange;
        public sbyte SkillCooldown;
        public readonly PieceRank PieceRank;
        public readonly List<Effect> Effects;
        public readonly List<int> PreviousMoves;
        public readonly string Type;
        private readonly bool hasSkill;
        public readonly List<Augmentation.Augmentation> Augmentations;
        public readonly List<ImmunityType> Immunities;
        public readonly List<FormationType> SpecificFormations;

        public byte MoveRange()
        {
            return moveRange;
        }
        
        public byte AttackRange()
        {
            return attackRange;
        }

        public void SetMoveRange(byte newRange)
        {
            moveRange = newRange;
        }

        public void SetAttackRange(byte newRange)
        {
            attackRange = newRange;
        }

        protected PieceLogic(PieceConfig cfg, QuietsDelegate quiets = null, CapturesDelegate captures = null)
        {
            Color = cfg.Color;
            Pos = cfg.Index;
            Effects = new List<Effect>();
            PreviousMoves = new List<int>();
            Type = cfg.Type;

            var info = AssetManager.Ins.PieceData[cfg.Type];
            moveRange =  info.moveRange;
            attackRange = info.attackRange;
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

            Quiets(list, Pos, isPlayer);
            Captures(list, Pos, excludeEmptyTile);

            if (hasSkill)
            {
                if (Effects.Any(e => e.EffectName == "effect_silenced") ||
                    Effects.Any(e => e.EffectName == "effect_illusion") ||
                        Effects.Any(e => e.EffectName == "effect_crown_of_silence_passive"))
                {
                    return; 
                }
                ((IPieceWithSkill)this).Skills(list, isPlayer, excludeEmptyTile);
            }
            
            NotifyOnMoveGen(this, list);
        }

        public int GetMoveRange()
        {
            int range = moveRange;
            if (Effects.Any(e => e.EffectName == "effect_bound")) return 0;

            if (range > MaxLength) return range;

            foreach (var e in Effects)
            {
                if (e is IMoveRangeModifier modifier)
                {
                    range = modifier.ModifyMoveRange(range);
                }
            }

            return Math.Max(1, range);
        }

        public int GetAttackRange()
        {
            int range = attackRange;
            if (Effects.Any(e => e.EffectName == "effect_pacified")) return 0;

            if (range > MaxLength) return range;

            foreach (var e in Effects)
            {
                if (e is IAttackRangeModifier modifier)
                {
                    range = modifier.ModifyAttackRange(range);
                }
            }

            return Math.Max(1, range);
        }

        public bool IsImmuneToFormationEffect(FormationType formationType, Effect appliedEffect)
        {
            if (formationType == FormationType.None || appliedEffect == null) return false;
            
            if (Immunities.Contains(ImmunityType.FormationDebuff) && 
                appliedEffect.Category == EffectCategory.Debuff)
            {
                return true;
            }

            if (Immunities.Contains(ImmunityType.FormationSpecific) && 
                SpecificFormations.Contains(formationType))
            {
                return true;
            }
            
            return false;
        }
        
        public void AddImmunity(ImmunityType immunityType)
        {
            if (!Immunities.Contains(immunityType))
            {
                Immunities.Add(immunityType);
            }
        }

        public void RemoveImmunity(ImmunityType immunityType)
        {
            Immunities.Remove(immunityType);
        }

        public void AddSpecificFormation(FormationType formationType)
        {
            if (!SpecificFormations.Contains(formationType))
            {
                SpecificFormations.Add(formationType);
            }
        }
        public void RemoveSpecificFormation(FormationType formationType)
        {
            SpecificFormations.Remove(formationType);
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
            var value = Quiets(new List<Action.Action>(), Pos, isPlayer: true);
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
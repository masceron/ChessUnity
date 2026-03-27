using System;
using System.Collections.Generic;
using Game.Augmentation;
using Game.Augmentation.Set;
using Game.Common;
using Game.Effects;
using Game.Effects.States;
using Game.Managers;
using Game.Movesets;
using Game.ScriptableObjects;
using Game.Tile;
using Game.Triggers;
using ZLinq;
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
        FormationSpecific
    }

    public enum SkillStat
    {
        Duration,
        Target,
        Unit,
        Range,
        Chance,
        Counter,
        Cooldown,
        Radius,
        Stack,
        Strength,
        Number
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class PieceLogic
    {
        public readonly int ID;
        private bool _hasSkill;
        public readonly List<Augmentation.Augmentation> Augmentations;
        public readonly List<Effect> Effects;
        public readonly List<ImmunityType> Immunities;
        public readonly PieceRank PieceRank;
        public readonly List<int> PreviousMoves;
        private readonly UDictionary<SkillStat, List<int>> SkillStats;
        public readonly List<FormationType> SpecificFormations;
        public readonly string Type;
        private int _attackRange;
        private int _moveRange;
        public CapturesDelegate Captures;
        public bool Color;
        public bool IsVisible = true;
        public int Pos;

        public QuietsDelegate Quiets;
        public int SkillCooldown;

        /// <summary>State hiện tại của quân cờ. Mặc định là <see cref="StateType.None"/>.</summary>
        public StateType CurrentState { get; private set; } = StateType.None;

        protected PieceLogic(PieceConfig cfg, QuietsDelegate quiets = null, CapturesDelegate captures = null)
        {
            Color = cfg.Color;
            Pos = cfg.Index;
            Effects = new List<Effect>();
            PreviousMoves = new List<int>();
            Type = cfg.Type;
            ID = NextPieceID();

            var info = AssetManager.Ins.PieceData[cfg.Type];
            _moveRange = info.moveRange;
            _attackRange = info.attackRange;
            PieceRank = info.rank;
            _hasSkill = info.hasSkill;
            Immunities = new List<ImmunityType>();
            SpecificFormations = new List<FormationType>();

            if (this is IPieceWithSkill pieceWithSkill)
            {
                SkillStats = new UDictionary<SkillStat, List<int>>();
                pieceWithSkill.TimeToCooldown = info.normalSkillCooldown != -1 ? info.normalSkillCooldown + 1 : -1;
            }
            else
            {
                SkillCooldown = -1;
            }

            Quiets = quiets;
            Captures = captures;

            Augmentations = new List<Augmentation.Augmentation>();
            if (cfg.AugmentationNames != null)
                foreach (var augmentationName in cfg.AugmentationNames)
                    Augmentations.Add(AugmentationHelper.NameToNewAugmentation(augmentationName));

            if (Augmentations is not { Count: > 0 }) return;
            if (ValidAugmentation(Augmentations)) ApplyAugmentationEffects(Augmentations);
        }

        public int MoveRange()
        {
            return _moveRange;
        }

        public int AttackRange()
        {
            return _attackRange;
        }

        public void SetMoveRange(byte newRange)
        {
            _moveRange = newRange;
        }

        public void SetAttackRange(int newRange)
        {
            _attackRange = newRange;
        }

        public void SetHasSkill(bool hasSkill)
        {
            _hasSkill = hasSkill;
        }

        private bool ValidAugmentation(List<Augmentation.Augmentation> augmentations)
        {
            if (IsDulicatedSlot(augmentations)) return false;

            var pieceInfo = AssetManager.Ins.PieceData[Type];
            return augmentations.All(ag => CanEquip(pieceInfo, ag));
        }

        private bool IsDulicatedSlot(List<Augmentation.Augmentation> augmentations)
        {
            var slotSet = new HashSet<AugmentationSlot>();
            return augmentations.Any(ag => !slotSet.Add(ag.Slot));
        }

        private bool CanEquip(PieceInfo piece, Augmentation.Augmentation augment)
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
            foreach (var aug in augmentations.Where(aug => aug.Set != null && aug.Set.Type != AugmentationSetType.None))
            {
                setCount.TryAdd(aug.Set.Type, 0);

                setCount[aug.Set.Type]++;
            }

            foreach (var setInfo in from kv in setCount
                     let setInfo = augmentations.FirstOrDefault(a => a.Set != null && a.Set.Type == kv.Key)?.Set
                     where setInfo != null
                     let count = kv.Value
                     where setInfo.HaveBonus && count >= setInfo.RequiredPieces
                     where setInfo.BonusEffects != null
                     select setInfo)
                setInfo.ApplyBonusEffects();
        }

        public bool HasAugmentation(AugmentationName augmentationName)
        {
            return Augmentations.Any(a => a.Name == augmentationName);
        }

        public Augmentation.Augmentation GetAugmentation(AugmentationName augmentationName)
        {
            return Augmentations.FirstOrDefault(a => a.Name == augmentationName);
        }

        public void PassTurn()
        {
            if (SkillCooldown > 0) SkillCooldown--;
        }

        /// <summary>
        ///     Thêm vào tham số list những Action có thể thực thi. Ví dụ: Moves, Captures, ISkills <br />
        ///     List này sau đó sẽ được hiển thị lên Board bởi BoardViewer
        ///     isPlayer là để phân biệt người dùng hay AI
        ///     excludeEmptyTile = false thì sẽ lấy cả những ô không có target, mục đích là để lấy enemySnapshot để đánh giá điểm
        ///     cho action của Maker
        ///     excludeEmptyTile = true thì sẽ là logic thông thường, để mark những ô có thể đi/ăn/skill
        /// </summary>
        /// <param name="list"></param>
        public void MoveList(List<Action.Action> list, bool isPlayer = true, bool excludeEmptyTile = true)
        {
            if (PieceRank == PieceRank.Construct) return;
            if (Effects.Any(e => e.EffectName == "effect_stunned")) return;
            if (Effects.Any(e => e.EffectName == "effect_frenzied")) return;

            Quiets(list, Pos, isPlayer);
            Captures(list, Pos, excludeEmptyTile);

            if (_hasSkill)
            {
                if (Effects.Any(e =>
                        e.EffectName is "effect_illusion" or "effect_silenced" or "effect_crown_of_silence_passive"))
                    return;

                ((IPieceWithSkill)this).Skills(list, isPlayer, excludeEmptyTile);
            }

            NotifyOnMoveGen(this, list);
        }

        public int GetMoveRange()
        {
            var range = _moveRange;
            if (Effects.Any(e => e.EffectName == "effect_bound")) return 0;

            if (range > MaxLength) return range;

            foreach (var e in Effects)
                if (e is IMoveRangeModifierTrigger modifier)
                    range = modifier.ModifyMoveRange(range);

            return Math.Max(1, range);
        }

        public int GetAttackRange()
        {
            var range = _attackRange;
            //if (Effects.Any(e => e.EffectName == "effect_pacified")) return 0;

            if (range > MaxLength) return range;

            foreach (var e in Effects)
                if (e is IAttackRangeModifier modifier)
                    range = modifier.ModifyAttackRange(range);

            return Math.Max(1, range);
        }

        public bool IsImmuneToFormationEffect(FormationType formationType, Effect appliedEffect)
        {
            if (formationType == FormationType.None || appliedEffect == null) return false;

            if (Immunities.Contains(ImmunityType.FormationDebuff) &&
                appliedEffect.Category == EffectCategory.Debuff)
                return true;

            if (Immunities.Contains(ImmunityType.FormationSpecific) &&
                SpecificFormations.Contains(formationType))
                return true;

            return false;
        }

        public void AddImmunity(ImmunityType immunityType)
        {
            if (!Immunities.Contains(immunityType)) Immunities.Add(immunityType);
        }

        public void RemoveImmunity(ImmunityType immunityType)
        {
            Immunities.Remove(immunityType);
        }

        public void AddSpecificFormation(FormationType formationType)
        {
            if (!SpecificFormations.Contains(formationType)) SpecificFormations.Add(formationType);
        }

        public void RemoveSpecificFormation(FormationType formationType)
        {
            SpecificFormations.Remove(formationType);
        }

        // ── State Management ──────────────────────────────────────────────────────

        /// <summary>
        ///     Gán State Effect mới cho quân cờ này. Nếu đang có State cũ, sẽ tự xóa trước.
        ///     Đảm bảo <b>chỉ có 1 State</b> tại 1 thời điểm.
        /// </summary>
        public void SetState(StateEffect stateEffect)
        {
            ClearState();
            CurrentState = stateEffect.StateType;
        }

        /// <summary>Xóa State hiện tại, trả về <see cref="StateType.None"/>.</summary>
        public void ClearState()
        {
            var existing = Effects.Find(e => e is IStateful);
            if (existing != null) {
                Effects.Remove(existing);
                BoardUtils.RemoveObserver(existing);
            }
            CurrentState = StateType.None;
        }

        /// <summary>Kiểm tra quân cờ có đang ở State chỉ định không.</summary>
        public bool HasState(StateType type) => CurrentState == type;

        public int GetValueForAI()
        {
            var value = RankToValue(PieceRank);
            value += AssetManager.Ins.PieceData[Type].baseValue;

            foreach (var effect in Effects) value += effect.GetValueForAI();

            foreach (var aug in Augmentations) value += aug.GetValueForAI();

            value += GetQuitesValue();

            return value;
        }

        public int GetQuitesValue()
        {
            var value = Quiets(new List<Action.Action>(), Pos, true);
            return value;
        }

        public int GetCapturesValue()
        {
            return Captures(new List<Action.Action>(), Pos, false);
        }

        private int RankToValue(PieceRank rank)
        {
            switch (rank)
            {
                case PieceRank.Swarm: return 50;
                case PieceRank.Common: return 70;
                case PieceRank.Elite: return 120;
                case PieceRank.Champion: return 200;
                case PieceRank.Commander: return 1000;
                case PieceRank.Construct: return 150;
                case PieceRank.Summoned: return 300;
                default: return 0;
            }
        }

        /// <summary>
        /// Trả về stat vĩnh viễn, chưa qua Effect 
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int GetRawStat(SkillStat stat, int num = 1)
        {
            if (!SkillStats.TryGetValue(stat, out var skillStat)) return 0;

            return skillStat[num - 1];
        }

        /// <summary>
        ///     Trả về stat sau khi đã qua sự chỉnh sửa của các Effect
        /// </summary>
        /// <param name="stat">Loại stat</param>
        /// <param name="num">Số thứ tự của stat</param>
        /// <returns></returns>
        public int GetStat(SkillStat stat, int num = 1)
        {
            if (!SkillStats.TryGetValue(stat, out var skillStat)) return 0;

            var finalStat = skillStat[num - 1];
            foreach (var effect in Effects)
                if (effect is ISkillStatModifierTrigger modifier)
                    finalStat += modifier.Modify(stat);

            return finalStat;
        }

        public void SetStat(SkillStat stat, int value, int num = 1)
        {
            if (!SkillStats.ContainsKey(stat)) SkillStats.Add(stat, new List<int>());

            var lst = SkillStats[stat];
            while (lst.Count < num) lst.Add(0);

            SkillStats[stat][num - 1] = value;
        }
    }
}
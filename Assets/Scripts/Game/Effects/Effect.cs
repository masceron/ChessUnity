using System.Collections.Generic;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;
using UX;

namespace Game.Effects
{
    /*
     *  The effect queue at the end of plies must look like the following:
     *  EndTurn..., RealmInfluence, RegionalEffect, StartTurn...
     */

    public enum EffectCategory : byte
    {
        Debuff,
        Buff,
        Trait,
        Condition,
        Augmentation,
        SpecialAbility,
        Skill,
        State
    }

    public enum EffectStack : byte
    {
        Stackable,
        NonStackable,
        Additive
    }

    public enum EffectStat
    {
        Target,
        Unit,
        Radius,
        Strength,
        Chance,
        Duration, // Đây là biến động, cần phân biệt với Duration - thời gian tồn tại của Effect
        Counter,
        Range,
        Number,
        Stack
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Effect : Observer
    {
        public readonly EffectCategory Category;
        public readonly string EffectName;
        public readonly UDictionary<EffectStat, List<int>> Stats;
        public int Duration;
        public PieceLogic Piece;
        public int Strength;

        protected Effect(int duration, int strength, PieceLogic piece, string name)
        {
            Piece = piece;
            EffectName = name;
            Color = Piece.Color;
            Duration = duration;

            if (AssetManager.Ins == null)
            {
                Debug.LogError($"[Effect] AssetManager.Ins is null while creating effect '{name}'.");
                throw new System.NullReferenceException($"AssetManager.Ins is null while creating effect '{name}'.");
            }

            if (AssetManager.Ins.EffectData == null)
            {
                Debug.LogError($"[Effect] EffectData is null while creating effect '{name}'.");
                throw new System.NullReferenceException($"EffectData is null while creating effect '{name}'.");
            }

            if (!AssetManager.Ins.EffectData.TryGetValue(name, out var info) || info == null)
            {
                var pieceType = piece != null ? piece.Type : "<null-piece>";
                Debug.LogError($"[Effect] Missing EffectData for key '{name}' (piece: '{pieceType}'). Please add this key to EffectsData.");
                throw new System.Collections.Generic.KeyNotFoundException($"Missing EffectData key '{name}' for piece '{pieceType}'.");
            }

            Category = info.category;

            Stats = new UDictionary<EffectStat, List<int>>();
            if (strength != -1) SetStat(EffectStat.Strength, strength);
        }

        public string Description()
        {
            return Localizer.GetText("effect_description",
                AssetManager.Ins.EffectData[EffectName].key + "_description",
                new object[] { this });
        }

        public virtual int GetValueForAI()
        {
            return 0;
        }

        public int GetRawStat(EffectStat stat, int num = 1)
        {
            return !Stats.TryGetValue(stat, out var stat1) ? 0 : stat1[num - 1];
        }

        public int GetStat(EffectStat stat, int num = 1)
        {
            if (!Stats.TryGetValue(stat, out var statin))
            {
                Debug.LogError("[Effect] You call GetStat of a EffectStat that doesn't exist");
                return 0;
            }
            var finalStat = statin[num - 1];
            foreach (var effect in Piece.Effects)
                if (effect is IEffectStatModifierTrigger modifier)
                    finalStat += modifier.Modify(stat);

            return finalStat;
        }

        public void SetStat(EffectStat stat, int value, int num = 1)
        {
            if (value < 0) value = 0;
            if (!Stats.ContainsKey(stat)) Stats.Add(stat, new List<int>());
            var lst = Stats[stat];
            while (lst.Count < num) lst.Add(0);
            Stats[stat][num - 1] = value;
        }
        public virtual int GetNumeral() => Strength;
    }
}
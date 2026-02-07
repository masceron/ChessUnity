using System.Collections.Generic;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX;

namespace Game.Effects
{
    
    /*
     *  The effect queue at the end of plies must look like the following:
     *  EndTurn..., RealmInfluence, RegionalEffect, StartTurn...
     */
    
    public enum EffectCategory: byte 
    {
        Debuff, Buff, Trait, Condition, Augmentation, SpecialAbility, Skill
    }

    public enum EffectStack : byte
    {
        Stackable, NonStackable, Additive
    }
    public enum EffectStat
    {
        Target,
        Unit,
        Radius,
        Strength,
        Chance,
    }

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Effect : Observer
    {
        public readonly string EffectName;
        public sbyte Duration;
        public sbyte Strength;
        public PieceLogic Piece;
        public readonly EffectCategory Category;
        private readonly UDictionary<EffectStat, List<int>> Stats;
        protected Effect(sbyte duration, sbyte strength, PieceLogic piece, string name)
        {
            Piece = piece;
            EffectName = name;
            
            var info = AssetManager.Ins.EffectData[name];
            Category = info.category;
            priority = info.priority;

            Stats = new();
            if (strength != -1)
            {
                SetStat(EffectStat.Strength, strength);
            }
        }

        public string Description()
        {
            return Localizer.GetText("effect_description",
                AssetManager.Ins.EffectData[EffectName].key + "_description",
                new object[]{this});
        }

        public virtual int GetValueForAI(){ return 0; }

        public int GetRawStat(EffectStat stat, int num = 1)
        {
            if (!Stats.ContainsKey(stat)) { return 0; }
            return Stats[stat][num - 1];
        }
        public int GetStat(EffectStat stat, int num = 1)
        {
            if (!Stats.ContainsKey(stat)) { return 0; }
            int finalStat = Stats[stat][num - 1];
            foreach (Effect effect in Piece.Effects)
            {
                if (effect is IEffectStatModifier modifier)
                {
                    finalStat += modifier.Modify(stat);
                }
            }
            return finalStat;
        }
        public void SetStat(EffectStat stat, int value, int num = 1)
        {
            if (!Stats.ContainsKey(stat))
            {
                Stats.Add(stat, new());
            }
            List<int> lst = Stats[stat];
            while (lst.Count < num)
            {
                lst.Add(0);
            }
            Stats[stat][num - 1] = value;
        }
    }
}
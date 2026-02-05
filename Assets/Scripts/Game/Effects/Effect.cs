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
        Duration,
        Strength,
        Chance,
    }

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Effect : Observer
    {
        public readonly string EffectName;
        public sbyte Duration
        {
            get {
                if (!Stats.ContainsKey(EffectStat.Duration)){ return -1; }
                return Stats[EffectStat.Duration];
            }
            set
            {
                Stats[EffectStat.Duration] = value;
            }
        }
        public sbyte Strength
        {
            get {
                if (!Stats.ContainsKey(EffectStat.Strength)){ return -1; }
                return Stats[EffectStat.Strength];
            }
            set
            {
                Stats[EffectStat.Strength] = value;
            }
        }
        public PieceLogic Piece;
        public readonly EffectCategory Category;
        public readonly Dictionary<EffectStat, sbyte> Stats;
        protected Effect(sbyte duration, sbyte strength, PieceLogic piece, string name)
        {
            Piece = piece;
            EffectName = name;
            
            var info = AssetManager.Ins.EffectData[name];
            Category = info.category;
            priority = info.priority;

            Stats = new();
            if (duration != -1)
            {
                Stats.Add(EffectStat.Duration, duration);
            }
            if (strength != -1)
            {
                Stats.Add(EffectStat.Strength, strength);
            }
        }

        public string Description()
        {
            return Localizer.GetText("effect_description",
                AssetManager.Ins.EffectData[EffectName].key + "_description",
                new object[]{this});
        }

        public virtual int GetValueForAI(){ return 0; }
        
        public int Get(EffectStat stat)
        {
            if (!Stats.ContainsKey(stat)){ return -1; } // Không tồn tại
            int finalStat = Stats[stat];
            foreach (Effect effect in Piece.Effects)
            {
                if (effect is IEffectStatModifier modifier)
                {
                    finalStat += modifier.Modify(stat);
                }
            }
            return finalStat;
        }
    }
}
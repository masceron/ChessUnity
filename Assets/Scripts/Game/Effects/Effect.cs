using System.Collections.Generic;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX;

namespace Game.Effects
{
    public enum ObserverPriority: byte
    {
        //Effect does not have a trigger
        None,
        
        // Priorities of effect trigger when an action is taken.
        AfterAction, DefenderAction, AttackerAction, Kill,
        
        //Priorities of effect trigger when ending a ply.
        //Effects on the start of turn have to run after effects at the end of turn.
        
        StartTurnBuff, StartTurnDebuff, StartTurnKill, StartTurnMove,
        
        RegionalEffect, RealmInfluence,
        
        EndturnBuff, EndturnDebuff, EndturnKill, EndturnMove,
    }
    
    /*
     *  The effect queue at the end of plies must look like the following:
     *  EndTurn..., RealmInfluence, RegionalEffect, StartTurn...
     */
    
    public enum EffectCategory: byte 
    {
        Debuff, Buff, Trait, Condition, Augmentation, SpecialAbility
    }

    public enum EffectStack : byte
    {
        Stackable, NonStackable, Additive
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Effect: IComparer<Effect>
    {
        public readonly string EffectName;
        public sbyte Duration;
        public sbyte Strength;
        public PieceLogic Piece;
        public readonly EffectCategory Category;
        public bool disabled = false;
        private readonly ObserverPriority priority;

        protected Effect(sbyte duration, sbyte strength, PieceLogic piece, string name)
        {
            Duration = duration;
            Strength = strength;
            Piece = piece;
            EffectName = name;
            
            var info = AssetManager.Ins.EffectData[name];
            priority = info.priority;
            Category = info.category;
        }

        public string Description()
        {
            return Localizer.GetText("effect_description",
                AssetManager.Ins.EffectData[EffectName].key + "_description",
                new object[]{this});
        }

        public virtual int GetValueForAI(){ return 0; }
        public int Compare(Effect x, Effect y)
        {
            return -y!.priority.CompareTo(x!.priority);
        }
    }
}
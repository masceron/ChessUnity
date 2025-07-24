using System.Collections.Generic;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects
{
    
    public enum ObserverPriority: byte
    {
        Low, AfterAction, DefenderAction, AttackerAction, Kill
    }
    
    public enum EffectCategory: byte 
    {
        Debuff, Buff, Trait, Condition
    }

    public enum EffectStack : byte
    {
        Stackable, NonStackable
    }

    public enum EffectType
    {
        Carapace,
        Evasion,
        Surpass,
        Slow,
        Blinded,
        Ambush,
        VelkarisMarked,
        SirenDebuffer,
        VelkarisMarker,
        Demolisher,
        Vengeful,
        Stunned,
        Shield,
        HardenedShield,
        Piercing,
        SlayersCoin,
        SnappingStrike,
        ArchelonDraw
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Effect: Comparer<Effect>
    {
        public sbyte Duration;
        public sbyte Strength;
        public readonly PieceLogic Piece;
        public readonly EffectType EffectName;
        
        public readonly ObserverType ObserverType;
        private readonly ObserverPriority priority;

        protected Effect(sbyte duration, sbyte strength, PieceLogic piece, EffectType type)
        {
            Duration = duration;
            Strength = strength;
            Piece = piece;
            EffectName = type;
            
            var info = MatchManager.assetManager.EffectData[type];
            ObserverType = info.activeWhen;
            priority = info.priority;
        }

        public virtual void OnApply()
        {
            
        }

        public virtual void OnRemove()
        {
            
        }

        public virtual void OnCall(Action.Action action)
        {
            
        }

        public override int Compare(Effect x, Effect y)
        {
            return -x!.priority.CompareTo(y!.priority);
        }

        public abstract string Description();
    }
}
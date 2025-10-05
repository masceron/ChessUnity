using System.Collections.Generic;
using Game.Managers;
using Game.Piece.PieceLogic;
using UX;

namespace Game.Effects
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
        Stackable, NonStackable, Additive
    }

    public enum EffectName
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
        ElectricEelVengeful,
        Stunned,
        Shield,
        HardenedShield,
        Piercing,
        SlayersCoin,
        SnappingStrike,
        ArchelonDraw,
        ThalassosShielder,
        Dominator,
        Poison,
        SwordfishAttack,
        Bleeding,
        Bound,
        LionfishVengeful,
        MorayEelCamouflage,
        Camouflage,
        Taunted,
        Consume,
        Solitary,
        Extremophile,
        Haste,
        RemoraMarked,
        HourglassJelly,
        FreeMovement,
        DestroyEnemyWhenMove,
        SeaTurtleCountdown,
        FrenziedVeteran,
        TrueBite,
        CopyCatureMethod,
        ClownFishPassive,
        LivingCoralPassive
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Effect: Comparer<Effect>
    {
        public sbyte Duration;
        public sbyte Strength;
        public readonly PieceLogic Piece;
        public readonly EffectName EffectName;
        public readonly EffectCategory Category;
        
        public readonly ObserverActivateWhen ObserverActivateWhen;
        private readonly ObserverPriority priority;

        protected Effect(sbyte duration, sbyte strength, PieceLogic piece, EffectName name)
        {
            Duration = duration;
            Strength = strength;
            Piece = piece;
            EffectName = name;
            
            var info = AssetManager.Ins.EffectData[name];
            ObserverActivateWhen = info.activeWhen;
            priority = info.priority;
            Category = info.category;
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

        public virtual void OnCallMoveGen(List<Action.Action> actions)
        {
            
        }

        public override int Compare(Effect x, Effect y)
        {
            return -x!.priority.CompareTo(y!.priority);
        }

        public string Description()
        {
            return Localizer.GetText("effect_description", AssetManager.Ins.EffectData[EffectName].key + "_description", new object[]{this});
        }
    }
}
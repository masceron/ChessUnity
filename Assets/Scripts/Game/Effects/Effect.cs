using System;
using System.Collections.Generic;
using Game.Managers;
using Game.Piece.PieceLogic;
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
        Debuff, Buff, Trait, Condition, Augmentation
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
        CopyCaptureMethod,
        ClownFishPassive,
        LivingCoralPassive,
        PureMinded,
        Relentless,
        DeathDefiance,
        ChamberedNautilusHunger,
        EpauletteSharkPurify,
        DiurnalAmbush,
        Infected,
        Construct,
        UndyingDevotion,
        OneMoreTurn,
        FractureZonePassive,
        BioluminescentBeaconPassive,
        SunfishPassive,
        DormantFossilPassive,
        BlueRingedOctopusPassive,
        QuickReflex,
        ContagionCorpsePassive,
        NocturnalRangeBuff,
        HammerOysterPassive,
        EntanglingTentacles,
        Silenced,
        Charge, 
        KelpForestPassive,
        BottlenoseDolphinPassive,
        Controlled,
        PollutedRockPassive,
        TidalRetinaPassive,
        MelibePassive,
        BlueDragonPassive,
        Sanity,
        Marked,
        Fear,
        Frenzied,
        NativeGround,
        SlimeheadPassive,
        Adaptation,
        RayTailPassive,
        HumboldtSquidPassive,
        Frienzied,
        BlackSwallowerVengeful,
        KillPieceAfterSwitchTurn,
        ArcherfishAccuracyPassive,
        CoffinFishVengeful,
        SnipeEelPassive
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Effect: Comparer<Effect>
    {
        public sbyte Duration;
        public sbyte Strength;
        public PieceLogic Piece;
        public readonly EffectName EffectName;
        public readonly EffectCategory Category;
        
        public readonly ObserverActivateWhen ObserverActivateWhen;
        public readonly ObserverPriority Priority;

        protected Effect(sbyte duration, sbyte strength, PieceLogic piece, EffectName name)
        {
            Duration = duration;
            Strength = strength;
            Piece = piece;
            EffectName = name;
            
            var info = AssetManager.Ins.EffectData[name];
            ObserverActivateWhen = info.activeWhen;
            Priority = info.priority;
            Category = info.category;
        }

        public virtual void OnApply()
        {
            
        }

        public virtual void OnRemove()
        {
            
        }

        public virtual void OnCallPieceAction(Action.Action action)
        {
            
        }

        public virtual void OnCallRelicAction(Action.Action action)
        {
            
        }

        public virtual void OnCallMoveGen(List<Action.Action> actions)
        {
            
        }

        public override int Compare(Effect x, Effect y)
        {
            return -x!.Priority.CompareTo(y!.Priority);
        }

        public string Description()
        {
            return Localizer.GetText("effect_description",
                AssetManager.Ins.EffectData[EffectName].key + "_description",
                new object[]{this});
        }
    }
}
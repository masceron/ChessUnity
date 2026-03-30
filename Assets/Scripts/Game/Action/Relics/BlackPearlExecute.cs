using Game.Action.Internal;
using Game.Effects;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UnityEngine;
using ZLinq;
using Random = System.Random;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class BlackPearlExecute : Action, IRelicAction
    {
        [MemoryPackInclude] private bool _color;

        [MemoryPackConstructor]
        private BlackPearlExecute()
        {
        }

        public BlackPearlExecute(int target, bool color) : base(null, target)
        {
            _color = color;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(GetTargetAsPiece().Color == _color
                ? new ApplyEffect(GetRandomBuffEffect())
                : new ApplyEffect(GetRandomDebuffEffect()));
        }

        private Effect GetRandomBuffEffect()
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();

            var random = new Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];
            Debug.Log("Selected Effect Name: " + selectedEffectName);
            return CreateEffectFromName(selectedEffectName, GetTargetAsPiece());
        }

        private Effect GetRandomDebuffEffect()
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Debuff)
                .Select(kvp => kvp.Key)
                .ToArray();

            var random = new Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];
            return CreateEffectFromName(selectedEffectName, GetTargetAsPiece());
        }

        private static Effect CreateEffectFromName(string effectName, PieceLogic piece)
        {
            var randomDuration = new Random().Next(6, 8);
            return effectName switch
            {
                "effect_shield" => new Shield(piece),
                "effect_carapace" => new Carapace(randomDuration, piece),
                "effect_haste" => new Haste(randomDuration, 1, piece),
                "effect_piercing" => new Piercing(randomDuration, piece),
                "effect_hardened_shield" => new HardenedShield(piece),
                "effect_true_bite" => new TrueBite(-1, piece),
                "effect_camouflage" => new Camouflage(piece),


                // Debuffs
                "effect_slow" => new Slow(randomDuration, 1, piece),
                "effect_blinded" => new Blinded(randomDuration, 50, piece),
                "effect_stunned" => new Stunned(randomDuration, piece),
                "effect_poison" => new Poison(randomDuration, piece),
                "effect_bleeding" => new Bleeding(5, piece),
                "effect_bound" => new Bound(randomDuration, piece),
                "effect_taunted" => new Taunted(randomDuration, piece),
                _ => null
            };
        }
    }
}
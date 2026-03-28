using Game.Action.Internal;
using Game.Common;
using Game.Effects;
using Game.Effects.Buffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UnityEngine;
using ZLinq;
using Random = System.Random;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class CommonPearlExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private CommonPearlExecute()
        {
        }


        public CommonPearlExecute(int target) : base(-1, target)
        {
        }

        protected override void ModifyGameState()
        {
            var effect = GetRandomBuffEffect(GetTarget());
            if (effect == null) return;
            ActionManager.EnqueueAction(new ApplyEffect(effect));
        }

        public Effect GetRandomBuffEffect(PieceLogic piece)
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();

            var random = new Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];
            Debug.Log("Selected Effect Name: " + selectedEffectName);
            return CreateEffectFromName("effect_shield", piece);
        }

        private static Effect CreateEffectFromName(string effectName, PieceLogic piece)
        {
            var randomDuration = new Random().Next(2, 6);

            // TODO: Add more effects missing
            return effectName switch
            {
                "effect_shield" => new Shield(piece),
                "effect_carapace" => new Carapace(randomDuration, piece),
                "effect_haste" => new Haste(randomDuration, 1, piece),
                "effect_piercing" => new Piercing(randomDuration, piece),
                "effect_hardened_shield" => new HardenedShield(piece),
                "effect_true_bite" => new TrueBite(-1 ,piece),
                "effect_camouflage" => new Camouflage(piece),

                _ => null
            };
        }
    }
}
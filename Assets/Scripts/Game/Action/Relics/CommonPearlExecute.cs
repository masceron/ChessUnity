using Game.Action.Internal;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Action.Relics
{

    public class CommonPearlExecute : Action, IRelicAction
    {

        public CommonPearlExecute(int target) : base(target)
        {
            Target = target;
        }

        protected override void ModifyGameState()
        {
            var effect = GetRandomBuffEffect(BoardUtils.PieceOn(Target));
            if (effect == null)
            {
                return; 
            }
            ActionManager.EnqueueAction(new ApplyEffect(effect));
        }

        public Effect GetRandomBuffEffect(PieceLogic piece)
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();
            
            var random = new System.Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];
            UnityEngine.Debug.Log("Selected Effect Name: " + selectedEffectName);
            return CreateEffectFromName("effect_shield", piece);
        }

        private static Effect CreateEffectFromName(string effectName, PieceLogic piece)
        {
            var randomDuration = (sbyte)new System.Random().Next(2, 6);
            
            // TODO: Add more effects missing
            return effectName switch
            {
                "effect_shield" => new Effects.Buffs.Shield(piece),
                "effect_carapace" => new Effects.Buffs.Carapace(randomDuration, piece),
                "effect_haste" => new Effects.Buffs.Haste(randomDuration, 1, piece),
                "effect_piercing" => new Effects.Buffs.Piercing(randomDuration, piece),
                "effect_hardened_shield" => new Effects.Buffs.HardenedShield(piece),
                "effect_true_bite" => new Effects.Buffs.TrueBite(piece),
                "effect_camouflage" => new Effects.Buffs.Camouflage(piece),

                _ => null
            };
        }
    }
}

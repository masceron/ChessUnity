using Game.Piece.PieceLogic.Commons;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using Game.Managers;
using ZLinq;

namespace Game.Effects
{
    public static class EffectFactory
    {
        public static Effect CreateRandomEffect(PieceLogic pieceLogic)
        {
            var debuffs = AssetManager.Ins.EffectData
                    .Where(kvp => kvp.Value.category == EffectCategory.Debuff)
                    .Select(kvp => kvp.Key)
                    .ToArray();
                    
                var random = new System.Random();
            var selectedEffectName = debuffs[random.Next(debuffs.Length)];
            
            return EffectFactory.CreateEffect(selectedEffectName, 5, 1, pieceLogic);
        }
        public static Effect CreateEffect(string effectName, int duration, int strength, PieceLogic piece)
        {
            return effectName switch
            {
                // Buffs 
                "effect_carapace" => new Carapace(duration, piece),
                "effect_hardened_shield" => new HardenedShield(piece),
                "effect_piercing" => new Piercing(duration, piece),
                "effect_shield" => new Shield(piece),
                "effect_camouflage" => new Camouflage(piece, strength),
                "effect_haste" => new Haste(duration, strength, piece),

                // Traits 
                "effect_evasion" => new Evasion(duration, 25, piece),
                "effect_construct" => new Construct(piece),
                "effect_demolisher" => new Demolisher(piece),
                "effect_consume" => new Consume(piece),
                "effect_surpass" => new Surpass(piece),
                "effect_ambush" => new Ambush(piece),
                "effect_quick_reflex" => new QuickReflex(piece),

                // Debuffs
                "effect_slow" => new Slow(strength, duration, piece),
                "effect_blinded" => new Blinded(duration, 25, piece),
                "effect_stunned" => new Stunned(duration, piece),
                "effect_poison" => new Poison(duration, piece),
                "effect_bleeding" => new Bleeding(duration, piece),
                "effect_bound" => new Bound(duration, piece),
                "effect_taunted" => new Taunted(duration, piece),

                _ => new Shield(piece)
            };
        }
    }

}

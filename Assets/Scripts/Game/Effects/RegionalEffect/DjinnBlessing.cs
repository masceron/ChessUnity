using Game.Managers;
using UnityEngine;
using Game.Action.Internal;
using Game.Action;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Effects.RegionalEffect
{
    public class DjinnBlessing: RegionalEffect
    {
        private int isActive;
        public DjinnBlessing() : base(RegionalEffectType.DjinnBlessing)
        {
            isActive = 1;
        }

        private static Effect CreateEffect(string effectName, sbyte duration, sbyte strength, PieceLogic piece)
        {

            return effectName switch
            {
                // Buffs 
                "effect_carapace" => new Buffs.Carapace(duration, piece),
                "effect_hardened_shield" => new Buffs.HardenedShield(piece),
                "effect_piercing" => new Buffs.Piercing(duration, piece),
                "effect_shield" => new Buffs.Shield(piece),
                "effect_camouflage" => new Buffs.Camouflage(piece, strength),
                "effect_haste" => new Buffs.Haste(duration, strength, piece),
                
                // Traits 
                "effect_evasion" => new Traits.Evasion(duration, 25, piece),
                "effect_construct" => new Traits.Construct(piece),
                "effect_demolisher" => new Traits.Demolisher(piece),
                "effect_consume" => new Traits.Consume(piece),
                "effect_surpass" => new Traits.Surpass(piece),
                "effect_ambush" => new Traits.Ambush(piece),
                "effect_quick_reflex" => new Traits.QuickReflex(piece),

                // Debuffs
                "effect_slow" => new Debuffs.Slow(strength, duration, piece),
                "effect_blinded" => new Debuffs.Blinded(duration, 25, piece),
                "effect_stunned" => new Debuffs.Stunned(duration, piece),
                "effect_poison" => new Debuffs.Poison(duration, piece),
                "effect_bleeding" => new Debuffs.Bleeding(duration, piece),
                "effect_bound" => new Debuffs.Bound(duration, piece),
                "effect_taunted" => new Debuffs.Taunted(duration, piece),

                _ => new Buffs.Shield(piece)
            };
        }
        protected override void ApplyEffect(int currentTurn)
        {
            Debug.Log("isActive: " + isActive);
            if (isActive == 3)
            {
                var board = MatchManager.Ins.GameState.PieceBoard;
                
                var validPieces = board.Where(piece => piece != null && piece.PieceRank != PieceRank.Commander).ToList();

                if (validPieces.Count > 0) {
                    var randomInd = Random.Range(0, validPieces.Count);
                    sbyte duration = 5;
                    sbyte strength = 1;
                    var roll = Random.Range(1, 101);
                    Debug.Log("roll: " + roll + " " + validPieces[randomInd].Type);
                    switch (roll)
                    {
                        case <= 45:
                            ActionManager.EnqueueAction(new ApplyEffect(GetRandomBuffEffect(validPieces[randomInd], duration, strength)));
                            break;
                        case <= 90:
                            ActionManager.EnqueueAction(new ApplyEffect(GetRandomDebuffEffect(validPieces[randomInd], duration, strength)));
                            break;
                        default:
                            ActionManager.EnqueueAction(new KillPiece(validPieces[randomInd].Pos));
                            break;
                    }
                }
                isActive = 0;
            }
            isActive++;
        }
        private static Effect GetRandomDebuffEffect(PieceLogic piece, sbyte duration, sbyte strength)
        {
            var debuffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Debuff)
                .Select(kvp => kvp.Key)
                .ToArray();
            
            var random = new System.Random();
            var selectedEffectName = debuffEffects[random.Next(debuffEffects.Length)];
            return CreateEffect(selectedEffectName, duration, strength, piece);
        }
        private Effect GetRandomBuffEffect(PieceLogic piece, sbyte duration, sbyte strength)
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();
            
            var random = new System.Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];
            return CreateEffect(selectedEffectName, duration, strength, piece);
        }

    }
}
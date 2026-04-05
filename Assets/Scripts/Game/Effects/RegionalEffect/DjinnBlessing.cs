using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using ZLinq;

namespace Game.Effects.RegionalEffect
{
    public class DjinnBlessing : FieldEffect
    {
        private int isActive;

        public DjinnBlessing() : base(RegionalEffectType.DjinnBlessing)
        {
            isActive = 1;
        }

        private static Effect CreateEffect(string effectName, int duration, int strength, PieceLogic piece)
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

        protected override void ApplyEffect(int currentTurn)
        {
            Debug.Log("isActive: " + isActive);
            if (isActive == 3)
            {
                var board = BoardUtils.PieceBoard();

                var validPieces = board.Where(piece => piece != null && piece.PieceRank != PieceRank.Commander)
                    .ToList();

                if (validPieces.Count > 0)
                {
                    var randomInd = Random.Range(0, validPieces.Count);
                    var duration = 5;
                    var strength = 1;
                    var roll = Random.Range(1, 101);
                    Debug.Log("roll: " + roll + " " + validPieces[randomInd].Type);
                    switch (roll)
                    {
                        case <= 45:
                            ActionManager.EnqueueAction(
                                new ApplyEffect(GetRandomBuffEffect(validPieces[randomInd], duration, strength)));
                            break;
                        case <= 90:
                            ActionManager.EnqueueAction(
                                new ApplyEffect(GetRandomDebuffEffect(validPieces[randomInd], duration, strength)));
                            break;
                        default:
                            ActionManager.EnqueueAction(new KillPiece(validPieces[randomInd]));
                            break;
                    }
                }

                isActive = 0;
            }

            isActive++;
        }

        private static Effect GetRandomDebuffEffect(PieceLogic piece, int duration, int strength)
        {
            var debuffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Debuff)
                .Select(kvp => kvp.Key)
                .ToArray();

            var random = new System.Random();
            var selectedEffectName = debuffEffects[random.Next(debuffEffects.Length)];
            return CreateEffect(selectedEffectName, duration, strength, piece);
        }

        private Effect GetRandomBuffEffect(PieceLogic piece, int duration, int strength)
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
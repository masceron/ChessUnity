using System.Linq;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UnityEngine;
using UX.UI.Ingame;
using Game.AI;
using Game.Action.Relics;


namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlackPearlPending : Action, System.IDisposable, IPendingAble, IRelicAction
    {
        private BlackPearl blackPearl;
        
        public BlackPearlPending(BlackPearl cp, int maker, bool pos = false) : base(maker, pos)
        {
            blackPearl = cp;
            Target = (ushort)maker;
        }

        private Effect GetRandomBuffEffect()
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();
            
            var random = new System.Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];
            Debug.Log("Selected Effect Name: " + selectedEffectName);
            return CreateEffectFromName(selectedEffectName, BoardUtils.PieceOn(Target));
        }

        private Effect GetRandomDebuffEffect()
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Debuff)
                .Select(kvp => kvp.Key)
                .ToArray();
            
            var random = new System.Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];
            return CreateEffectFromName(selectedEffectName, BoardUtils.PieceOn(Target));
        }

        private static Effect CreateEffectFromName(string effectName, PieceLogic piece)
        {
            var randomDuration = (sbyte)new System.Random().Next(6, 8);
            return effectName switch
            {
                "effect_shield" => new Effects.Buffs.Shield(piece),
                "effect_carapace" => new Effects.Buffs.Carapace(randomDuration, piece),
                "effect_haste" => new Effects.Buffs.Haste(randomDuration, 1, piece),
                "effect_piercing" => new Effects.Buffs.Piercing(randomDuration, piece),
                "effect_hardened_shield" => new Effects.Buffs.HardenedShield(piece),
                "effect_true_bite" => new Effects.Buffs.TrueBite(piece),
                "effect_camouflage" => new Effects.Buffs.Camouflage(piece),


                // Debuffs
                "effect_slow" => new Effects.Debuffs.Slow(randomDuration, 1, piece),
                "effect_blinded" => new Effects.Debuffs.Blinded(randomDuration, 50, piece),
                "effect_stunned" => new Effects.Debuffs.Stunned(randomDuration, piece),
                "effect_poison" => new Effects.Debuffs.Poison(randomDuration, piece),
                "effect_bleeding" => new Effects.Debuffs.Bleeding(5, piece),
                "effect_bound" => new Effects.Debuffs.Bound(randomDuration, piece),
                "effect_taunted" => new Effects.Debuffs.Taunted(randomDuration, piece),
                _ => null
            };
        }
        public void CompleteAction()
        {
            ActionManager.ExecuteImmediately(BoardUtils.PieceOn(Target).Color == blackPearl.Color
                ? new ApplyEffect(GetRandomBuffEffect())
                : new ApplyEffect(GetRandomDebuffEffect()));


            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            blackPearl.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();

        }

        public void Dispose()
        {
            blackPearl = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {
        }
    }
}
using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic;
using UX.UI.Ingame;
using UnityEngine;

namespace Game.Relics.Pearl
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlackPearlPending : Game.Action.Action, System.IDisposable, IPendingAble
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

        private Effect CreateEffectFromName(EffectName effectName, PieceLogic piece)
        {
            sbyte randomDuration = (sbyte)new System.Random().Next(6, 8);
            return effectName switch
            {
                EffectName.Shield => new Effects.Buffs.Shield(piece),
                EffectName.Carapace => new Effects.Buffs.Carapace(randomDuration, piece),
                EffectName.Haste => new Effects.Buffs.Haste(randomDuration, 1, piece),
                EffectName.Piercing => new Effects.Buffs.Piercing(randomDuration, piece),
                EffectName.HardenedShield => new Effects.Buffs.HardenedShield(piece),
                EffectName.TrueBite => new Effects.Buffs.TrueBite(piece),
                EffectName.Camouflage => new Effects.Buffs.Camouflage(piece),


                // Debuffs
                EffectName.Slow => new Effects.Debuffs.Slow(randomDuration, 1, piece),
                EffectName.Blinded => new Effects.Debuffs.Blinded(randomDuration, 50, piece),
                EffectName.Stunned => new Effects.Debuffs.Stunned(randomDuration, piece),
                EffectName.Poison => new Effects.Debuffs.Poison(randomDuration, piece),
                EffectName.Bleeding => new Effects.Debuffs.Bleeding(5, piece),
                EffectName.Bound => new Effects.Debuffs.Bound(randomDuration, piece),
                EffectName.Taunted => new Effects.Debuffs.Taunted(randomDuration, piece),
                _ => new Effects.Buffs.Shield(piece)
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
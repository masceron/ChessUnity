using System.Linq;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CommonPearlPending : Action, System.IDisposable, IPendingAble
    {
        private CommonPearl commonPearl;
        
        public CommonPearlPending(CommonPearl cp, int maker, bool pos = false) : base(maker, pos)
        {
            commonPearl = cp;
            Target = (ushort)maker;
            Maker = (ushort)maker;
        }
        public Effect GetRandomBuffEffect()
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();
            
            var random = new System.Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];
            
            return CreateEffectFromName(selectedEffectName, BoardUtils.PieceOn(Target));
        }

        private static Effect CreateEffectFromName(string effectName, PieceLogic piece)
        {
            var randomDuration = (sbyte)new System.Random().Next(2, 6);

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
        public void CompleteAction()
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(GetRandomBuffEffect()));

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            commonPearl.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

        public void Dispose()
        {
            commonPearl = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {
        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
        }
    }
}
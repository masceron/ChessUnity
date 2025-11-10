using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic;
using UX.UI.Ingame;

namespace Game.Relics.Pearl
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CommonPearlPending : Game.Action.Action, System.IDisposable, IPendingAble
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

        private Effect CreateEffectFromName(EffectName effectName, PieceLogic piece)
        {
            sbyte randomDuration = (sbyte)new System.Random().Next(2, 6);

            return effectName switch
            {
                EffectName.Shield => new Effects.Buffs.Shield(piece),
                EffectName.Carapace => new Effects.Buffs.Carapace(randomDuration, piece),
                EffectName.Haste => new Effects.Buffs.Haste(randomDuration, 1, piece),
                EffectName.Piercing => new Effects.Buffs.Piercing(randomDuration, piece),
                EffectName.HardenedShield => new Effects.Buffs.HardenedShield(piece),
                EffectName.TrueBite => new Effects.Buffs.TrueBite(piece),
                EffectName.Camouflage => new Effects.Buffs.Camouflage(piece),
                _ => new Effects.Buffs.Shield(piece)
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
    }
}
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
    public class BlackPearlPending : Game.Action.Action, System.IDisposable, IPendingAble
    {
        private BlackPearl blackPearl;
        
        public BlackPearlPending(BlackPearl cp, int maker, bool pos = false) : base(maker, pos)
        {
            blackPearl = cp;
            Target = (ushort)maker;
            Maker = (ushort)maker;
        }

        private Effect GetRandomBuffEffect()
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();
            
            var random = new System.Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];
            
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
            return effectName switch
            {
                EffectName.Shield => new Effects.Buffs.Shield(piece),
                EffectName.Carapace => new Effects.Buffs.Carapace(2, piece),
                EffectName.Haste => new Effects.Buffs.Haste(2, 1, piece),
                EffectName.Piercing => new Effects.Buffs.Piercing(2, piece),
                EffectName.HardenedShield => new Effects.Buffs.HardenedShield(piece),
                EffectName.TrueBite => new Effects.Buffs.TrueBite(piece),
                EffectName.Camouflage => new Effects.Buffs.Camouflage(piece),


                // Debuffs
                EffectName.Slow => new Effects.Debuffs.Slow(2, 1, piece),
                EffectName.Blinded => new Effects.Debuffs.Blinded(2, 50, piece),
                EffectName.Stunned => new Effects.Debuffs.Stunned(1, piece),
                EffectName.Poison => new Effects.Debuffs.Poison(2, piece),
                EffectName.Bleeding => new Effects.Debuffs.Bleeding(piece),
                EffectName.Bound => new Effects.Debuffs.Bound(2, piece),
                EffectName.Taunted => new Effects.Debuffs.Taunted(2, piece)
                // _ => new Effects.Buffs.Shield(piece)
            };
        }
        public void CompleteAction()
        {
            
            if(BoardUtils.PieceOn(Target).Color == blackPearl.Color)
            {
                ActionManager.ExecuteImmediately(new ApplyEffect(GetRandomBuffEffect()));
                ActionManager.ExecuteImmediately(new ApplyEffect(GetRandomBuffEffect()));
            }
            else
            {
                ActionManager.ExecuteImmediately(new ApplyEffect(GetRandomDebuffEffect()));
                ActionManager.ExecuteImmediately(new ApplyEffect(GetRandomDebuffEffect()));
            }
            //TODO: Sửa lại thành EnqueueAction.

            TileManager.Ins.UnmarkAll();


            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            blackPearl.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic();

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
using Game.Action;
using Game.Managers;
using UX.UI.Ingame;
using Game.Action.Skills;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Common;
using Game.Effects;
using System.Linq;
using Game.Piece.PieceLogic;

namespace Game.Relics
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
        public Effect GetRandomDebuffEffect()
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
                EffectName.Shield => new Game.Effects.Buffs.Shield(piece),
                EffectName.Carapace => new Game.Effects.Buffs.Carapace(2, piece),
                EffectName.Haste => new Game.Effects.Buffs.Haste(2, 1, piece),
                EffectName.Piercing => new Game.Effects.Buffs.Piercing(2, piece),
                EffectName.HardenedShield => new Game.Effects.Buffs.HardenedShield(piece),
                EffectName.TrueBite => new Game.Effects.Buffs.TrueBite(piece),
                EffectName.Camouflage => new Game.Effects.Buffs.Camouflage(piece),


                // Debuffs
                EffectName.Slow => new Game.Effects.Debuffs.Slow(2, 1, piece),
                EffectName.Blinded => new Game.Effects.Debuffs.Blinded(2, 50, piece),
                EffectName.Stunned => new Game.Effects.Debuffs.Stunned(1, piece),
                EffectName.Poison => new Game.Effects.Debuffs.Poison(2, piece),
                EffectName.Bleeding => new Game.Effects.Debuffs.Bleeding(piece),
                EffectName.Bound => new Game.Effects.Debuffs.Bound(2, piece),
                EffectName.Taunted => new Game.Effects.Debuffs.Taunted(2, piece),
                _ => new Game.Effects.Buffs.Shield(piece)
            };
        }
        public void CompleteAction()
        {
            if(BoardUtils.PieceOn(Target).Color == blackPearl.Color)
            {
                ActionManager.EnqueueAction(new ApplyEffect(GetRandomBuffEffect()));
            }
            else
            {
                ActionManager.EnqueueAction(new ApplyEffect(GetRandomDebuffEffect()));
            }

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
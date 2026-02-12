using UX.UI.Ingame;
using UX.UI.Ingame.DeathDefianceUI;
using Game.Piece.PieceLogic.Commons;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DeathDefiance: Effect, IBeforePieceActionEffect, IAfterPieceActionEffect
    {
        private int _deathDefianceCount;
        public DeathDefiance(PieceLogic piece, int deathDefianceCount) : base(-1, 1, piece, "effect_death_defiance")
        {
            _deathDefianceCount = deathDefianceCount;
        }

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.Target != Piece.Pos || action.Maker == action.Target) return;
            if (action.Result != ResultFlag.Success) return;  
            if (Piece.Effects.Any(e => e.EffectName == "effect_shield") 
                || Piece.Effects.Any(e => e.EffectName == "effect_carapace") 
                || Piece.Effects.Any(e => e.EffectName == "effect_hardened_shield")) return;
            if (_deathDefianceCount <= 0) return;  

            action.Result = ResultFlag.SurvivedHit;
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.Target != Piece.Pos) return;
            if (action.Result != ResultFlag.SurvivedHit) return;  

            if (_deathDefianceCount <= 0) return;
            
            var ui = BoardViewer.Ins.GetOrInstantiateUI<DeathDefianceUI>(IngameSubmenus.DeathDefianceUI);
            ui.Load(Piece.Pos, this);  
            _deathDefianceCount--;
        }

        public void OnEffectChosen(string effectName)
        {
            var piece = Piece;
            var effect = DeathDefianceUI.CreateEffectStatic(effectName, piece);
            ActionManager.EnqueueAction(new ApplyEffect(effect, piece));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 80;
        }
    }
}
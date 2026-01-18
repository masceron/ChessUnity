// Assets/Scripts/Game/Action/Internal/Pending/Relic/SeafoamPhialPending.cs

using Game.Managers;
using static Game.Common.BoardUtils;
using Game.Action.Relics;
using Game.Relics.Commons;
using UX.UI.Ingame.HermosHorn;
using UnityEngine;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HermosHornActive : Action, System.IDisposable, IRelicAction
    {
        private RelicLogic hermosHorn;
        private bool isFirstOption;
        public HermosHornActive(RelicLogic methaneCasing, bool isFirstOption) : base(-1)
        {
            this.hermosHorn = methaneCasing;
            this.isFirstOption = isFirstOption;
        }

        protected override void ModifyGameState()
        {
            if (isFirstOption)
            {
                foreach(var piece in PieceBoard())
                {
                    if (piece == null || piece.Color == hermosHorn.Color) { continue; }
                    foreach(var effect in piece.Effects)
                    {
                        if (effect.EffectName == "effect_shortreach")
                        {
                            effect.Strength++;
                        }
                    }
                }
            }
            else
            {
                foreach(var piece in PieceBoard())
                {
                    if (piece == null || piece.Color != hermosHorn.Color) { continue; }
                    foreach(var effect in piece.Effects)
                    {
                        if (effect.EffectName == "effect_long_reach")
                        {
                            effect.Strength++;
                        }
                    }
                }
            }
            hermosHorn.SetCooldown();
            Object.Destroy(HermosHornUI.Ins.gameObject);
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }

        public void Dispose()
        {
            hermosHorn = null;
        }
    }
}

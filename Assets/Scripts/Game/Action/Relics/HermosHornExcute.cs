using Game.Common;
using UnityEngine;

namespace Game.Action.Relics
{
    public class HermosHornExcute : Action, IRelicAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="relicColor">Bên của relic để phân biệt bên nào dùng relic</param>
        /// <param name="isFirstOption">Option 1 là tăng strength ShortReach của tất cả quân địch. Option 2 là tăng strength của tất cả các quân đồng minh</param>
        public HermosHornExcute(bool relicColor, bool isFirstOption) : base(-1)
        {
            this.isFirstOption = isFirstOption;
            this.relicColor = relicColor;
        }

        private bool isFirstOption;
        private bool relicColor;

        protected override void ModifyGameState()
        {
            if (isFirstOption)
            {
                foreach(var piece in BoardUtils.PieceBoard())
                {
                    if (piece == null || piece.Color != relicColor) { continue; }
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
                foreach(var piece in BoardUtils.PieceBoard())
                {
                    if (piece == null || piece.Color == relicColor) { continue; }
                    foreach(var effect in piece.Effects)
                    {
                        if (effect.EffectName == "effect_long_reach")
                        {
                            effect.Strength++;
                        }
                    }
                }
            }
        }

    }
}

using MemoryPack;
using Game.Common;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class HermosHornExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private HermosHornExecute() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relicColor">Bên của relic để phân biệt bên nào dùng relic</param>
        /// <param name="isFirstOption">Option 1 là tăng strength ShortReach của tất cả quân địch. Option 2 là tăng strength của tất cả các quân đồng minh</param>
        public HermosHornExecute(bool relicColor, bool isFirstOption) : base(-1)
        {
            _isFirstOption = isFirstOption;
            _relicColor = relicColor;
        }

        [MemoryPackInclude]
        private bool _isFirstOption;
        [MemoryPackInclude]
        private bool _relicColor;

        protected override void ModifyGameState()
        {
            if (_isFirstOption)
            {
                foreach(var piece in BoardUtils.PieceBoard())
                {
                    if (piece == null || piece.Color != _relicColor) { continue; }
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
                    if (piece == null || piece.Color == _relicColor) { continue; }
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

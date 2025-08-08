using Game.Managers;
using Game.Piece.PieceLogic;

namespace Game.Effects.Buffs
{
    public class Haste: Effect
    {
        public Haste(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, EffectName.Haste)
        {}

        public override string Description()
        {
            return string.Format(AssetManager.Ins.EffectData[EffectName].description, Strength);
        }
    }
}
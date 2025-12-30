using Game.Piece.PieceLogic.Commons;
using Game.Tile;


namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Adaptation: Effect
    {
        public Adaptation(PieceLogic piece) : base(-1, 1, piece, "effect_adaptation")
        {}

        public override void OnApply()
        {
            base.OnApply();
            Piece.AddImmunity(ImmunityType.FormationDebuff);
        }
        
        public override void OnRemove()
        {
            base.OnRemove();
            Piece.RemoveImmunity(ImmunityType.FormationDebuff);
        }

    }
}
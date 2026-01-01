using Game.Piece.PieceLogic.Commons;


namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Adaptation: Effect, IOnApply, IOnRemove
    {
        public Adaptation(PieceLogic piece) : base(-1, 1, piece, "effect_adaptation")
        {}

        public void OnApply()
        {
            Piece.AddImmunity(ImmunityType.FormationDebuff);
        }
        
        public void OnRemove()
        {
            Piece.RemoveImmunity(ImmunityType.FormationDebuff);
        }

    }
}
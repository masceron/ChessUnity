namespace Game.Piece.PieceLogic.Commons{

    public interface IPieceWithDoubleSelectionSkill : IPieceWithSkill{
        public int firstSelection{ get; set; }
    }
}
namespace Game.Piece.PieceLogic{

    public interface IPieceWithDoubleSelectionSkill : IPieceWithSkill{
        public int firstSelection{ get; set; }
    }
}
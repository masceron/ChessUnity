using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    public class Pacified : Effect
    {

        public Pacified(PieceLogic piece) : base(-1, 1, piece, "effect_pacified")
        {
            
        }
        
        public override void OnCallPieceAction(Action.Action action)
        {
            
        }

        public override void OnRemove()
        {
            
        }
    }
}
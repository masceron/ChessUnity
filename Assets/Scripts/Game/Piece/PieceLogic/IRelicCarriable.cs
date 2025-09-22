using Game.Relics;

namespace Game.Piece.PieceLogic
{
    public interface IRelicCarriable
    {
        RelicLogic CarriedRelic { get; set; }

        void RelicUse()
        {
            if (CarriedRelic == null) return;
            
            CarriedRelic.Activate();
            CarriedRelic = null;
        }
    }
}
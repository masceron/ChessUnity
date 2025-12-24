using Game.Common;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;

namespace Game.Relics.Commons
{
    public abstract class RelicLogic
    {
        public string type;
        protected sbyte TimeCooldown;
        public sbyte CurrentCooldown { get; protected set; }
        public bool Color; // false for white, true for black

        public PieceLogic CommanderPiece
        {
            get
            {
                foreach (PieceLogic p in BoardUtils.PieceBoard())
                {
                    if (p == null) continue;
                    if (p.PieceRank != PieceRank.Commander) continue;
                    if (p.Color != Color) continue;
                    return p;
                }
                return null;
            }
        }

        protected RelicLogic(RelicConfig cfg)
        {
            Color = cfg.Color;
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }

        public abstract void Activate();

        public void SetCooldown()
        {
            CurrentCooldown = TimeCooldown;
        }

        public void PassTurn()
        {
            if (CurrentCooldown > 0) CurrentCooldown--;
        }

        public virtual void ActiveForAI()
        {
            
        }
    }

    public static class RelicMaker
    {
        public static RelicLogic Get(RelicConfig cfg)
        {
            return RelicFactory.CreateLogicInstance(cfg.Type, cfg);
        }
    }
}
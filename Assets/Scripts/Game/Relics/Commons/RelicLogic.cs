using System.Collections.Generic;
using Game.Common;
using Game.Piece.PieceLogic.Commons;

namespace Game.Relics.Commons
{
    public abstract class RelicLogic
    {
        public bool Color; // false for white, true for black
        public int TimeCooldown;
        public string Type;
        private bool _accumulation;

        protected RelicLogic(RelicConfig cfg)
        {
            Color = cfg.Color;
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }

        private int _currentCountDown;

        public int CurrentCooldown
        {
            get => _currentCountDown;
            set
            {
                _currentCountDown = value;
                _accumulation = false;
            }
        }

        public PieceLogic CommanderPiece => BoardUtils.GetCommanderOf(Color);

        public abstract void Activate(List<Action.Action> actions);

        public void PassTurn()
        {
            if (CurrentCooldown <= 0) return;

            if (!_accumulation) _accumulation = true;
            else
            {
                _accumulation = false;
                CurrentCooldown -= 1;
            }
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
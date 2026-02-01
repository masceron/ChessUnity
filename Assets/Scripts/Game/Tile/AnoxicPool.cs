using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using System.Linq;
using Game.Piece.PieceLogic.Commons;
using Game.Managers;

namespace Game.Tile
{
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AnoxicPool : Formation, ISubscriber
    {
        private int turnsOnTile; 
        
        public AnoxicPool(bool haveDuration, bool color) : base(color)
        {
            HaveDuration = haveDuration;
            turnsOnTile = 0;
            MatchManager.Ins.GameState.Subscribers.Add(this);
        }

        public override FormationType GetFormationType()
        {
            return FormationType.AnoxicPool;
        }

        public override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            turnsOnTile = 0;
            
            if (piece != null && piece.Color != Color)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(3, 1, piece), FormationType.AnoxicPool));
            }
        }

        public override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
            turnsOnTile = 0;
        }

        public override int GetValueForAI()
        {
            return -10;
        }
        
        public void OnCall(Action.Action action) { }
        
        public void OnCallEnd(bool endOfSide)
        {
            if (PieceOnFormation == null)
            {
                return;
            }
            
            if (endOfSide != PieceOnFormation.Color)
            {
                return;
            }
            
            if (PieceOnFormation.Color == Color)
            {
                return;
            }
            
            turnsOnTile++;
            
            if (turnsOnTile > 3)
            {
                var hasPacified = PieceOnFormation.Effects.Any(e => e.EffectName == "effect_pacified");
                if (hasPacified)
                {
                    return;
                }
                ActionManager.EnqueueAction(new ApplyEffect(new Pacified(3, PieceOnFormation), FormationType.AnoxicPool));
            }
        }
    }
}
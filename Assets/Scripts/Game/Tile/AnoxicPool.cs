using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Triggers;

namespace Game.Tile
{
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AnoxicPool : Formation, IEndTurnTrigger
    {
        private int _turnsOnTile;

        public new EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Debuff;

        public EndTurnEffectType EndTurnEffectType{ get; }
        public AnoxicPool(bool haveDuration, bool color) : base(color)
        {
            HaveDuration = haveDuration;
            _turnsOnTile = 0;
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public override FormationType GetFormationType()
        {
            return FormationType.AnoxicPool;
        }

        protected override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            _turnsOnTile = 0;
            
            if (piece != null && piece.Color != Color)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(3, 1, piece), FormationType.AnoxicPool));
            }
        }

        protected override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
            _turnsOnTile = 0;
        }

        public override int GetValueForAI()
        {
            return -10;
        }
        
        
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (PieceOnFormation == null)
            {
                return;
            }
            
            if (PieceOnFormation.Color == Color)
            {
                return;
            }
            
            _turnsOnTile++;
            
            if (_turnsOnTile > 3)
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
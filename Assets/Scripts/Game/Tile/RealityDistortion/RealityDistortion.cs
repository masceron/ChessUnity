using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Managers;
using Game.Effects;

namespace Game.Tile.RealityDistortion
{
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RealityDistortion : Formation, IStartTurnEffect
    {
        
        public RealityDistortion(bool haveDuration, bool color) : base(color)
        {
            HaveDuration = haveDuration;
        }

        public override FormationType GetFormationType()
        {
            return FormationType.RealityDistortion;
        }

        public override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
        }

        public override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
        }

        public override int GetValueForAI()
        {
            return -10;
        }
        
        public void OnCallStart(Action.Action lastMainAction)
        {
            if (RealityDistortionManager.Ins == null) return;
            RealityDistortionManager.Ins.OnTurnStart(Color);
        }
        
        public StartTurnEffectType StartTurnEffectType { get; set; } 
            = StartTurnEffectType.StartOfAnyTurn;
    }
}
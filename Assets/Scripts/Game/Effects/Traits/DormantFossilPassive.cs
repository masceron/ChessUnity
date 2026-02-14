using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UX.UI.Ingame;
using UX.UI.Ingame.DormantFossil;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DormantFossilPassive : Effect, IEndTurnTrigger
    {
        private const byte TurnsToActive = 15;
        private byte _numTurns = TurnsToActive;

        public DormantFossilPassive(PieceLogic piece) : base(-1, -1, piece, "effect_dormant_fossil_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            _numTurns--;
            if (_numTurns == 0) ActivePassive();
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Other;

        public EndTurnEffectType EndTurnEffectType { get; }

        private void ActivePassive()
        {
            var ui = BoardViewer.Ins.GetOrInstantiateUI<DormantFossilUI>(IngameSubmenus.DormantFossilUI);
            ui.Load(Piece.Pos);
        }
    }
}
using Game.Action;
using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.Piece.PieceLogic.Commons;
using Game.Common;
using ZLinq;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumbugDamselFishPassive : Effect, IStartTurnEffect
    {
        public HumbugDamselFishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_humbug_damsel_fish_passive")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfAllyTurn;
        }

        public void OnCallStart(Action.Action lastMainAction)
        {
            var evasion = 1;
            var listPieces = SkillRangeHelper.GetActiveAllyPieceInRadius(Piece.Pos, 4);
            foreach (var piece in listPieces)
            {
                if (PieceOn(piece).Type == "piece_humbug_damsel_fish")
                {
                    evasion += 5;
                } else if (PieceOn(piece).Effects.Any(e => e.EffectName == "effect_evasion"))
                {
                    evasion += 2;
                }
            }
            var existingEvasion = Piece.Effects.OfType<Evasion>().FirstOrDefault();
            if (existingEvasion != null)
            {
                existingEvasion.Strength = evasion;
            }
            else
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, evasion, Piece)));
            }
        }

        public StartTurnEffectType StartTurnEffectType { get; }
    }
}

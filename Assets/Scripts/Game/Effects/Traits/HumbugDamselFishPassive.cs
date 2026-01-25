using Game.Action;
using Game.Action.Internal;
using Game.Action.Captures;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using System.Linq;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Buffs;
using Game.Effects;
using Game.Common;

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
            int evasion = 1;
            var (rank, file) = RankFileOf(Piece.Pos);
            foreach (var (nRank, nFile) in MoveEnumerators.AroundUntil(rank, file, 4))
            {
                var piece = PieceOn(IndexOf(nRank, nFile));
                if (piece == null || piece.Color != Piece.Color) continue;
                if (piece.Type == "piece_humbug_damsel_fish")
                {
                    evasion += 5;
                } else if (piece.Effects.Any(e => e.EffectName == "effect_evasion"))
                {
                    evasion += 2;
                }
            }
            var existingEvasion = Piece.Effects.OfType<Evasion>().FirstOrDefault();
            if (existingEvasion != null)
            {
                existingEvasion.Probability = evasion;
            }
            else
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, evasion, Piece)));
            }
        }

        public StartTurnEffectType StartTurnEffectType { get; }
    }
}

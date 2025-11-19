using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    
    public class BlueRingedOctopusPassive : Effect, IEndTurnEffect
    {
        public BlueRingedOctopusPassive(PieceLogic piece) : base(-1, 1, piece, "effect_blue_ringed_octopus_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public void OnCallEnd(Action.Action action)
        {
            var rank = RankOf(Piece.Pos);
            var file = FileOf(Piece.Pos);

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 1))
            {
                var pos = IndexOf(rankOff, fileOff);
                ActionManager.ExecuteImmediately(new ApplyEffect(new Poison(1, PieceOn(pos))));
            }
        }
        
        public EndTurnEffectType EndTurnEffectType { get; }
    }
}
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
        private int LastPos = -1;
        public BlueRingedOctopusPassive(PieceLogic piece) : base(-1, 1, piece, "effect_blue_ringed_octopus_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public void OnCallEnd(Action.Action action)
        {
            if (LastPos == Piece.Pos) return;
            if (LastPos != -1)
            {
                foreach(var (rank, file) in MoveEnumerators.AroundUntil(RankOf(LastPos), FileOf(LastPos), 3))
                {
                    Poisoned[IndexOf(rank, file)] = false;
                }
            }
            
            foreach(var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Piece.Pos), FileOf(Piece.Pos), 3))
            {
                Poisoned[IndexOf(rank, file)] = true;
            }
            
            LastPos = Piece.Pos;
        }
        
        public EndTurnEffectType EndTurnEffectType { get; }
    }
}
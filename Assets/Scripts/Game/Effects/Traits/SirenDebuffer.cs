using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenDebuffer: Effect, IEndTurnEffect
    {
        public SirenDebuffer(PieceLogic p) : base(-1, 1, p, "effect_siren_debuffer")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
            CalculateEffectRange(p.Pos);
        }

        private void CalculateEffectRange(int pos)
        {
            var (rank, file) = RankFileOf(pos);
            
            rankStart = ClampUp(rank - 4);
            rankEnd = ClampDown(rank + 4);
            fileStart = ClampUp(file - 4);
            fileEnd = ClampDown(file + 4);
        }

        private int rankStart;
        private int rankEnd;
        private int fileStart;
        private int fileEnd;

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (lastMainAction.Maker == Piece.Pos)
            {
                CalculateEffectRange(Piece.Pos);
            }

            for (var r = rankStart; r <= rankEnd; r++)
            {
                var rowIndex = RowIndex(r);
                for (var f = fileStart; f <= fileEnd; f++)
                {
                    var index = rowIndex + f;
                    var pOn = PieceOn(index);
                    if (pOn == null) continue;
                    
                    if (pOn.Color != Piece.Color && pOn.Effects.All(e => e.EffectName != "effect_slow"))
                    {
                        ActionManager.EnqueueAction(new SirenDebuff(Piece.Pos, Piece.Pos, (ushort)index));
                    }
                }
            }
        }

        public EndTurnEffectType EndTurnEffectType { get; set; }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 80;
        }
    }
}
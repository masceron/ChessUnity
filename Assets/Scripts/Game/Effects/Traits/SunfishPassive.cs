using Game.Piece.PieceLogic;
using Game.Managers;
using Game.Action.Internal;
using Game.Effects.Buffs;
using static Game.Common.BoardUtils;
using Game.Action;
using Game.Action.Skills;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SunfishPassive: Effect, IEndTurnEffect 
    {
        private bool active;
        private bool check;
        public SunfishPassive(PieceLogic piece) : base(-1, 1, piece, EffectName.SunfishPassive)
        {
            active = false;
            check = true;
        }

        public void OnCallEnd(Action.Action action)
        {
            if (check != MatchManager.Ins.GameState.IsDay) 
            {
                check = MatchManager.Ins.GameState.IsDay;
                active = !active;
                MakeActive();
            }
        }

        private void MakeActive()
        {
            var (rank, file) = RankFileOf(Piece.Pos);
            var caller = PieceOn(Piece.Pos);

            for (var rankOff = rank - 1; rankOff <= rank + 1; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;
                
                for (var fileOff = file - 1; fileOff <= file + 1; fileOff++)
                {
                    if (rankOff == rank && fileOff == file) continue;
                    var p = PieceOn(IndexOf(rankOff, fileOff));
                    if (p == null || p.Color != caller.Color) continue;

                    ActionManager.EnqueueAction(new ApplyEffect(new Shield(p)));
                }
            }
        }

        public EndTurnEffectType EndTurnEffectType { get; }
    }
}
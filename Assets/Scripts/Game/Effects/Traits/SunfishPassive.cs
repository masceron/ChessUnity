using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Triggers;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SunfishPassive : Effect, IEndTurnTrigger
    {
        private bool _wasNight;

        public SunfishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_sunfish_passive")
        {
            _wasNight = !MatchManager.Ins.GameState.IsDay;
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            switch (_wasNight)
            {
                case true when MatchManager.Ins.GameState.IsDay:
                    Debug.Log("SunfishPassive: MakeActive");
                    MakeActive();
                    _wasNight = false;
                    break;
                case false when !MatchManager.Ins.GameState.IsDay:
                    _wasNight = true;
                    break;
            }
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Buff;

        public EndTurnEffectType EndTurnEffectType { get; }

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

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 30;
        }
    }
}
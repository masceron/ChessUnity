using System;
using Game.Board.Action;
using Game.Board.General;
using Game.Board.Interaction;

namespace Game.Board.Effects
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Evasion: Effect
    {
        public Evasion(sbyte duration, sbyte strength, PieceLogic.PieceLogic piece) : base(duration, strength, piece, ObserverType.Captures, 3)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.Success == ActionResult.UNKNOWN)
            {
                if (action.GetType() == typeof(NormalCapture))
                {
                    var rankDiff = Math.Abs(action.From / InteractionManager.MaxFile -
                                            action.To / InteractionManager.MaxFile);
                    var fileDiff = Math.Abs(action.From % InteractionManager.MaxFile -
                                            action.To % InteractionManager.MaxFile);
                    if (Math.Max(rankDiff, fileDiff) >= 3) {
                        action.Success = MatchManager.Roll(Strength) ? ActionResult.SUCCEED : ActionResult.FAILED;
                        return;
                    }
                }
                action.Success = ActionResult.SUCCEED;
            }
        }
    }
}
using Game.Action;
using System.Collections.Generic;
using static Game.Common.BoardUtils;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Frenzied : Effect, IEndTurnTrigger
    {
        private List<Action.Action> _list;
        public Frenzied(PieceLogic piece, int duration = -1) : base(duration, 1, piece, "effect_frenzied")
        {
            _list = new List<Action.Action>();
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
            Duration = duration;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Kill;

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (!IsAlive(Piece) || PieceOn(Piece.Pos) != Piece) return;
            _list.Clear();
            Piece.Color = !Piece.Color;
            Piece.Captures(_list, Piece.Pos, excludeEmptyTile: true);
            Piece.Quiets(_list, Piece.Pos, isPlayer: true);
            if (_list.Count > 1)
            {
                _list = _list.Distinct(new ActionComparer()).ToList();
            }
            
            var captureTargets = _list.OfType<ICaptures>()
                .Select(c => ((Action.Action)c).Target)
                .ToList();
            var moveTargets = _list.OfType<IQuiets>()
                .Select(c => ((Action.Action)c).Target)
                .ToList();
            
            if (captureTargets.Count > 0)
            {

                var nearestTarget = captureTargets
                    .OrderBy(c => Distance(Piece.Pos, c))
                    .FirstOrDefault();

                if (nearestTarget < 0) return;
                var piece = PieceOn(Piece.Pos);
                if (piece != null && piece.Effects.Any(e => e.EffectName == "effect_snapping_strike"))
                {
                    ActionManager.EnqueueAction(new FrenziedCaptureDontMove(Piece.Pos, nearestTarget));
                }
                else
                {
                    ActionManager.EnqueueAction(new FrenziedCapture(Piece.Pos, nearestTarget));
                }
            } else if (moveTargets.Count > 0)
            {

                var random = new System.Random();
                var randomIndex = random.Next(0, moveTargets.Count);
                var randomTarget = moveTargets[randomIndex];
                ActionManager.EnqueueAction(new FrenziedMove(Piece.Pos, randomTarget));
            }

        }
    }
}
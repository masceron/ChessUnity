using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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

            // Pha 1: lấy positions
            var capturePositions = new List<int>();
            Piece.Captures(capturePositions, Piece.Pos);

            var quietPositions = new List<int>();
            Piece.Quiets(quietPositions, Piece.Pos);

            // Pha 2: tạo Actions
            foreach (var pos in capturePositions)
            {
                var target = PieceOn(pos);
                if (target != null && target.Color != Piece.Color)
                    _list.Add(new NormalCapture(Piece, target));
            }

            foreach (var pos in quietPositions)
                _list.Add(new NormalMove(Piece, pos));

            if (capturePositions.Count > 0)
            {
                var nearestTarget = capturePositions
                    .OrderBy(c => Distance(Piece.Pos, c))
                    .FirstOrDefault();

                if (nearestTarget < 0) return;
                var piece = PieceOn(Piece.Pos);
                var nearestPieceTarget = PieceOn(nearestTarget);
                if (piece != null && piece.Effects.Any(e => e.EffectName == "effect_snapping_strike"))
                    ActionManager.EnqueueAction(new FrenziedCaptureDontMove(Piece, nearestPieceTarget));
                else
                    ActionManager.EnqueueAction(new FrenziedCapture(Piece, nearestPieceTarget));
            }
            else if (quietPositions.Count > 0)
            {
                var random = new Random();
                var randomIndex = random.Next(0, quietPositions.Count);
                var randomTarget = quietPositions[randomIndex];
                ActionManager.EnqueueAction(new FrenziedMove(Piece, randomTarget));
            }
        }
    }
}
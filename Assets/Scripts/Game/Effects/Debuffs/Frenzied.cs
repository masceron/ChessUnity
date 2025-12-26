using Game.Action;
using System.Collections.Generic;
using System.Linq;
using static Game.Common.BoardUtils;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Frenzied : Effect, IEndTurnEffect
    {
        private List<Action.Action> list;
        public Frenzied(PieceLogic piece) : base(-1, 1, piece, "effect_frenzied")
        {
            list = new List<Action.Action>();
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
            
        }

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (Piece.IsDead() || PieceOn(Piece.Pos) != Piece) return;
            list.Clear();
            Piece.Color = !Piece.Color;
            Piece.Captures(list, Piece.Pos, excludeEmptyTile: true);
            Piece.Quiets(list, Piece.Pos, isPlayer: true);
            if (list.Count > 1)
            {
                list = list.Distinct(new ActionComparer()).ToList();
            }
            
            var captureTargets = list.OfType<ICaptures>()
                .Select(c => ((Action.Action)c).Target)
                .ToList();
            var moveTargets = list.OfType<IQuiets>()
                .Select(c => ((Action.Action)c).Target)
                .ToList();
            
            if (captureTargets.Count > 0)
            {

                var nearestTarget = captureTargets
                    .OrderBy(c => Distance(Piece.Pos, c))
                    .FirstOrDefault();
                
                if (nearestTarget >= 0)
                {
                    var piece = PieceOn(Piece.Pos);
                    if (piece != null && piece.Effects.Any(e => e.EffectName == "effect_snapping_strike"))
                    {
                        ActionManager.EnqueueAction(new FrenziedCaptureDontMove(Piece.Pos, nearestTarget));
                    }
                    else
                    {
                        ActionManager.EnqueueAction(new FrenziedCapture(Piece.Pos, nearestTarget));
                    }
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
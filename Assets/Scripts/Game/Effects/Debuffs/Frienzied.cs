using Game.Piece.PieceLogic;
using Game.Action;
using System.Collections.Generic;
using System.Linq;
using static Game.Common.BoardUtils;
using Game.Action.Captures;
using Game.Action.Quiets;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Frienzied : Effect, IEndTurnEffect
    {
        private List<Action.Action> list;
        public Frienzied(PieceLogic piece) : base(-1, 1, piece, EffectName.Frienzied)
        {
            list = new List<Action.Action>();
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
            
        }

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (Piece.IsDead() || PieceOn(Piece.Pos) != Piece) return;
            list.Clear();
            Piece.Captures(list, Piece.Pos);
            Piece.Color = !Piece.Color;
            Piece.Captures(list, Piece.Pos);
            Piece.Color = !Piece.Color;
            var i = 0;
            Piece.Quiets(list, Piece.Pos, ref i);
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
                   new FrienziedCapture(Piece.Pos, nearestTarget).CompleteAction();
                }
            } else if (moveTargets.Count > 0)
            {

                var random = new System.Random();
                var randomIndex = random.Next(0, moveTargets.Count);
                var randomTarget = moveTargets[randomIndex];
                new FrienziedMove(Piece.Pos, randomTarget).CompleteAction();
            }

        }
    }
}
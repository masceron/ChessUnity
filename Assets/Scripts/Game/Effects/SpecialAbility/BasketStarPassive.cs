using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class BasketStarPassive : Effect, IAfterPieceActionTrigger, IStartTurnTrigger
    {
        private bool isPreviousTurnNight = false;
        public BasketStarPassive(PieceLogic piece) : base(-1, 1, piece, "effect_basket_star_passive")
        {
            SetStat(EffectStat.Radius, 2);
            SetStat(EffectStat.Duration, 1, 1);
            SetStat(EffectStat.Duration, 10, 2);
        }
        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Debuff;
        StartTurnTriggerPriority IStartTurnTrigger.Priority => StartTurnTriggerPriority.Buff;
        public StartTurnEffectType StartTurnEffectType => StartTurnEffectType.StartOfAnyTurn;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not IQuiets) { return; }
            PieceLogic movedPiece = PieceOn(action.Maker);
            if (movedPiece == null) { return; }
            
            int radius = GetStat(EffectStat.Radius);
            
            // Case 1: Enemy piece moved into radius
            if (movedPiece != Piece && movedPiece.Color != Piece.Color)
            {
                if (Distance(movedPiece.Pos, Piece.Pos) > radius) { return; }

                int prevPos = movedPiece.PreviousMoves.Count > 0 ? movedPiece.PreviousMoves[movedPiece.PreviousMoves.Count - 1] : -1;
                if (prevPos != -1 && Distance(prevPos, Piece.Pos) <= radius) { return; }

                ActionManager.EnqueueAction(new ApplyEffect(new Bound(GetStat(EffectStat.Duration, 1), movedPiece)));
            }
            // Case 2: BasketStar moved, check if enemies enter its radius
            else if (Piece.PreviousMoves.Count > 0)
            {
                int oldPos = Piece.PreviousMoves[Piece.PreviousMoves.Count - 1];
                
                for (int i = 0; i < BoardSize; i++)
                {
                    if (!IsActive(i)) { continue; }
                    
                    PieceLogic enemy = PieceOn(i);
                    if (enemy == null || enemy.Color == Piece.Color) { continue; }
                    
                    int distToNewPos = Distance(i, Piece.Pos);
                    int distToOldPos = Distance(i, oldPos);
                    
                    // Enemy is within radius of new position but was not within radius of old position
                    if (distToNewPos <= radius && distToOldPos > radius)
                    {
                        ActionManager.EnqueueAction(new ApplyEffect(new Bound(GetStat(EffectStat.Duration, 1), enemy)));
                    }
                }
            }
        }
        public void OnCallStart(Action.Action lastMainAction)
        {
            if (isPreviousTurnNight && IsDay())
            {
                ActionManager.EnqueueAction(new ApplyEffect(new HardenedShield(Piece, 1, GetStat(EffectStat.Duration, 2))));
            }
            isPreviousTurnNight = IsDay() == false;
        }
    }


}
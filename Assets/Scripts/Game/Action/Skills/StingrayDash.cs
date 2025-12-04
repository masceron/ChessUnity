using Game.Action.Internal;
using Game.AI;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StingrayDash: Action, ISkills, IAIAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null) return 0;
            return pieceAI.Color != maker.Color ? -30 : 0;
        }
        public StingrayDash(int maker, int to) : base(maker, true)
        {
            Target = (ushort)to;
        }
        
        protected override void Animate()
        {
            PieceManager.Ins.Move(Maker, Target);
        }

        protected override void ModifyGameState()
        {
            var (rankFrom, fileFrom) = RankFileOf(Maker);
            var (rankTo, fileTo) = RankFileOf(Target);
            var board = PieceBoard();
            var caller = board[Maker];

            var rankDir = rankTo == rankFrom ? 0 : rankTo > rankFrom ? 1 : -1;
            var fileDir = fileTo == fileFrom ? 0 : fileTo > fileFrom ? 1 : -1;

            while (rankFrom != rankTo || fileFrom != fileTo)
            {
                rankFrom += rankDir;
                fileFrom += fileDir;

                var p = board[IndexOf(rankFrom, fileFrom)];
                if (p == null || p.Color == caller.Color) continue;
                
                ActionManager.EnqueueAction(new ApplyEffect(new Slow(1, 1, p)));
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, p)));
            }
            
            MatchManager.Ins.GameState.Move(Maker, Target);

            Maker = Target;
            
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        
        public void CompleteActionForAI()
        {
            throw new System.NotImplementedException();
        }
    
    }
}
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;
using Game.Managers;
using System;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HourglassJellyActive: Action, ISkills
    {
        private ushort destination;
        public HourglassJellyActive(int maker, int target) : base(maker, true)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
            PieceLogic piece = PieceOn(target);
            destination = (ushort)piece.PreviousMoves[Math.Max(0, piece.PreviousMoves.Count - 5)];
        }
        protected override void Animate()
        {
            PieceManager.Ins.Move(Target, destination);
            var destinationPiece = PieceOn(destination);
            if (destinationPiece != null){
                PieceManager.Ins.Destroy(destination);
            }
        }
        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Maker);
            var target = PieceOn(Target);
            var destinationPiece = PieceOn(destination);
            if (destinationPiece != null){
                MatchManager.Ins.GameState.Destroy(destination);
            }
            MatchManager.Ins.GameState.Move(Target, destination);
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}
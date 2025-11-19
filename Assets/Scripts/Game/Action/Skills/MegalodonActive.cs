using System.Collections.Generic;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MegalodonActive: Action, ISkills
    {
        private List<PieceLogic> availablePieces = new List<PieceLogic>();
        public MegalodonActive(int maker) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }

        private bool CanActive()
        {
            var (rank, file) = RankFileOf(Maker);
            var caller = PieceOn(Maker);
            var range = caller.AttackRange;
            var color = caller.Color;
            var push = !color ? -1 : 1;

            bool hasAlly = false;
            bool hasEnemy = false;

            foreach (var piece in availablePieces)
            {
                if (piece.Color == color) hasAlly = true;
                else hasEnemy = true;
                
                if (hasAlly && hasEnemy) return true;
            }

            return false;

        }

        public void SetCoolDown()
        {
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Maker);
            var caller = PieceOn(Maker);
            var range = caller.AttackRange;
            var color = caller.Color;
            var push = !color ? -1 : 1;

            for (var nRank = rank - (range - 1) * push; nRank != rank + (range + 1) * push; nRank += push)
            {
                if (!VerifyBounds(nRank)) continue;
                for (var nFile = file - range * push; nFile != file + range * push; nFile += push)
                {
                    if (!VerifyBounds(nFile)) continue;
                    var idx = IndexOf(nRank, nFile);
                    if (!IsActive(idx)) continue;

                    var piece = PieceOn(idx);

                    if (piece == null) continue;

                    availablePieces.Add(piece);
                }
            }

            if (!CanActive()) return;

            foreach (var piece in availablePieces)
            {
                if (piece.Color == color) continue;
                var pending = new MegalodonPending(this, caller.Pos, piece.Pos);
                BoardViewer.ListOf.Add(pending);
            }

            BoardViewer.Selecting = -2;
            BoardViewer.SelectingFunction = 4;
        }
            
    }
}

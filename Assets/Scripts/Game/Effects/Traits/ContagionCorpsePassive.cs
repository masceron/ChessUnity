using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using ZLinq;

namespace Game.Effects.Traits
{
    /// <summary>
    /// Contagion Corpse Passive Effect
    /// 
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ContagionCorpsePassive : Effect
    {
        private const int Radius = 2;

        public ContagionCorpsePassive(PieceLogic piece) : base(-1, 1, piece, "effect_contagion_corpse_passive")
        {
            HandlePassive();
        }

        private void HandlePassive()
        {

            var inRadius = MoveEnumerators.AroundUntil(BoardUtils.RankOf(Piece.Pos), BoardUtils.FileOf(Piece.Pos), Radius);
            var posInRadius = inRadius
                .Select(pos => BoardUtils.IndexOf(pos.Item1, pos.Item2))
                .ToList();

            var saprolegnia = new Saprolegnia(false, Piece.Color);

            foreach (var pos in posInRadius)
            {
                if (FormationManager.Ins.GetFormation(pos) != null || pos == Piece.Pos) continue;
                FormationManager.Ins.SetFormation(pos, saprolegnia);
                if (BoardUtils.PieceOn(pos) != null)
                {
                    FormationManager.Ins.TriggerEnter(pos);
                }
            }
        }
    }
}


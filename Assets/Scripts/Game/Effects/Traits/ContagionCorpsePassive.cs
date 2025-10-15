using System.Linq;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic;
using Game.Tile;
using UnityEngine;
namespace Game.Effects.Traits
{
    /// <summary>
    /// Contagion Corpse Passive Effect
    /// 
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ContagionCorpsePassive : Effect
    {
        private int radius = 2;
        public ContagionCorpsePassive(PieceLogic piece) : base(-1, 1, piece, EffectName.ContagionCorpsePassive)
        {
            HandlePassive();
        }

        private void HandlePassive()
        {

            var inRadius = MoveEnumerators.AroundUntil(BoardUtils.RankOf(Piece.Pos), BoardUtils.FileOf(Piece.Pos), radius);
            var posInRadius = inRadius
                .Select(pos => BoardUtils.IndexOf(pos.Item1, pos.Item2))
                .ToList();

            Saprolegnia saprolegnia = new Saprolegnia(false, Piece.Color);

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


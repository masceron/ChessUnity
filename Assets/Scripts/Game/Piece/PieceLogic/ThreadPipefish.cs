using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using System.Collections.Generic;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ThreadPipefish : Commons.PieceLogic, IPieceWithSkill
    {
        public ThreadPipefish(PieceConfig cfg) : base(cfg, UpDoorMoves.Quiets, UpDoorMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    foreach (var (nRank, nFile) in MoveEnumerators.AroundUntil(rank, file, 6))
                    {
                        var piece = PieceOn(IndexOf(nRank, nFile));

                        if (piece == null || piece.Color != Color) continue;

                        list.Add(new ThreadPipefishActive(Pos, IndexOf(nRank, nFile)));
                    }
                }
                else
                {
                    if (excludeEmptyTile)
                    {
                        var listPieces = new List<(Commons.PieceLogic, int)>();
                        foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 6))
                        {
                            var idx = IndexOf(rank, file);
                            var pOn = PieceOn(idx);
                            if (pOn != null && pOn.Color == Color)
                            {
                                if (pOn.Effects != null && pOn.Effects.Any(e => e.EffectName == "effect_extremophile")) continue;

                                var numberOfBuff = 0;
                                foreach (var effect in pOn.Effects)
                                {
                                    if (effect.EffectName == "effect_truebite" || effect.EffectName == "effect_momentum" || effect.EffectName == "effect_piercing")
                                    {
                                        numberOfBuff++;
                                    }
                                }

                                if (numberOfBuff > 0)
                                    listPieces.Add((pOn, numberOfBuff));
                            }
                        }

                        // neu khong co quan nao
                        if (listPieces.Count == 0) return;
                        // neu co dung mot quan
                        if (listPieces.Count == 1)
                        {
                            ActionManager.EnqueueAction(new ApplyEffect(new ThreadPipefishEffect(PieceOn(Pos), listPieces[0].Item1)));
                            return;
                        }

                        // neu co nhieu quan           
                        listPieces.Sort((a, b) =>
                            b.Item2
                                .CompareTo(a.Item2));

                        var randomIdx = UnityEngine.Random.Range(0, listPieces.Count);
                        ActionManager.EnqueueAction(new ApplyEffect(new ThreadPipefishEffect(PieceOn(Pos), listPieces[randomIdx].Item1)));

                    }
                }
            };
        }
        
        
        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
    
    
}
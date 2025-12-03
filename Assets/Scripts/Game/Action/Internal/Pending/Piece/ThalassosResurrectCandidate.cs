using System.Linq;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.ThalassosResurrector;
using static Game.Common.BoardUtils;
using Game.AI;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ThalassosResurrectCandidate: Action, IPendingAble, IInternal, ISkills, IAIAction
    {
        public ThalassosResurrectCandidate(int maker, int pos) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)pos;
        }

        public void CompleteAction()
        {
            var selector = Object.FindAnyObjectByType<ThalassosResurrector>(FindObjectsInactive.Include);
            if (!selector)
            {
                var canvas = Object.FindAnyObjectByType<BoardViewer>(FindObjectsInactive.Exclude);
                selector = Object.Instantiate(UIHolder.Ins.Get(IngameSubmenus.ThalassosResurrector), canvas.transform).GetComponent<ThalassosResurrector>();
            }
            else selector.gameObject.SetActive(true);

            selector.Load(Maker, Target);
        }

        public void CompleteActionForAI()
        {
            Debug.Log("[Thalassos] CompleteActionForAI");
            // Resolve maker piece
            var makerPiece = PieceOn(Maker);
            if (makerPiece == null) return;
            bool myColor = makerPiece.Color;

            // captured list for the maker's side
            var capturedList = myColor ? MatchManager.Ins.GameState.BlackCaptured : MatchManager.Ins.GameState.WhiteCaptured;
            if (capturedList == null || capturedList.Count == 0) return;

            // Filter candidates: only Common or Swarm rank
            var candidates = capturedList.ToList()
                .Where(cfg =>
                {
                    try
                    {
                        var info = AssetManager.Ins.PieceData[cfg.Type];
                        return info.rank == PieceRank.Common || info.rank == PieceRank.Swarm;
                    }
                    catch
                    {
                        return false;
                    }
                })
                .ToList();
            if (candidates.Count == 0) return;

            // Find empty squares around Maker within radius 1
            var (r0, f0) = RankFileOf(Maker);
            var emptySquares = new System.Collections.Generic.List<int>();
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int df = -1; df <= 1; df++)
                {
                    int rr = r0 + dr;
                    int ff = f0 + df;
                    if (!VerifyBounds(rr) || !VerifyBounds(ff)) continue;
                    int idx = IndexOf(rr, ff);
                    if (!IsActive(idx)) continue;
                    if (PieceOn(idx) == null) emptySquares.Add(idx);
                }
            }
            if (emptySquares.Count == 0) return;

            // Pick best candidate
            var bestScore = candidates.Max((p) => PieceMaker.Get(p).GetValueForAI());
            var top = candidates.Where(c => PieceMaker.Get(c).GetValueForAI() == bestScore).ToList();
            var chosenPiece = top.Count == 1 ? top[0] : top[UnityEngine.Random.Range(0, top.Count)];

            // Pick random empty square
            var chosenSquare = emptySquares[UnityEngine.Random.Range(0, emptySquares.Count)];

            // Spawn piece immediately
            MatchManager.Ins.InputProcessor.ExecuteAction(new ThalassosResurrect(Maker, chosenSquare, chosenPiece.Type));

            // Remove the resurrected piece from captured list
            var toRemove = capturedList.FirstOrDefault(c => c.Type == chosenPiece.Type && c.Color == chosenPiece.Color);
            if (toRemove != null)
            {
                capturedList.Remove(toRemove);
            }
            
        }

        protected override void ModifyGameState()
        {}
    }
}
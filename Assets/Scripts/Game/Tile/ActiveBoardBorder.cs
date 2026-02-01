using UnityEngine;
using System.Collections;
using Game.Managers;
using Game.Common;
using System.Collections.Generic;
using ZLinq;

namespace Game.Tile
{
    public class ActiveBoardBorder : MonoBehaviour
    {
        [Header("Board Settings")]
        public int boardWidth => BoardUtils.MaxLength;
        public int boardHeight => BoardUtils.MaxLength;
        public BitArray ActiveBoard => MatchManager.Ins.GameState.ActiveBoard;

        public float heightOffset = 1.2f;

        [Header("Border Settings")]
        public float lineWidth = 0.1f;
        public UnityEngine.Color lineColor = UnityEngine.Color.blue;
        public bool tightBorder; // use Minmax or Tight Outer Border

        [Header("Middle Line Settings")]
        public UnityEngine.Color middleLineColor = UnityEngine.Color.red;
        public float middleLineWidth = 0.05f;

        [Header("LineRender")]
        [SerializeField] private LineRenderer borderRenderer;
        [SerializeField] private LineRenderer midLineRenderer;
        public Material lineMaterial;

        private int minX, maxX, minY, maxY;
        private float borderZMin = float.PositiveInfinity, borderZMax = float.NegativeInfinity;
        private float borderXMin = float.PositiveInfinity, borderXMax = float.NegativeInfinity;

        void Awake()
        {
            SetupLine(borderRenderer, lineWidth, lineColor, true);
            SetupLine(midLineRenderer, middleLineWidth, middleLineColor, false);
        }

        void SetupLine(LineRenderer lr, float width, UnityEngine.Color color, bool loop)
        {
            lr.useWorldSpace = true;
            lr.loop = loop;
            lr.startWidth = width;
            lr.endWidth = width;
            lr.material = lineMaterial ?? new Material(Shader.Find("Sprites/Default"));
            lr.startColor = color;
            lr.endColor = color;
        }

        public void UpdateBorder()
        {
            if (ActiveBoard == null || ActiveBoard.Length == 0)
            {
                borderRenderer.positionCount = 0;
                midLineRenderer.positionCount = 0;
                return;
            }

            // compute min/max for middle line (kept)
            minX = boardWidth; maxX = -1; minY = boardHeight; maxY = -1;
            for (var x = 0; x < boardWidth; x++)
            {
                for (var y = 0; y < boardHeight; y++)
                {
                    var index = IndexOf(x, y);
                    if (ActiveBoard[index])
                    {
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;
                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                }
            }

            if (maxX == -1 || maxY == -1)
            {
                borderRenderer.positionCount = 0;
                midLineRenderer.positionCount = 0;
                return;
            }

            // reset extents
            borderXMin = float.PositiveInfinity; borderXMax = float.NegativeInfinity;
            borderZMin = float.PositiveInfinity; borderZMax = float.NegativeInfinity;

            if (!tightBorder)
                DrawMinMaxBorder();
            else
                DrawTightOuterBorder_CellBased();

            DrawMiddleLine();
        }

        private void DrawMinMaxBorder()
        {
            var half = 0.5f;
            var corners = new Vector3[]
            {
                new Vector3(minX - half, heightOffset, minY - half),
                new Vector3(minX - half, heightOffset, maxY + 1 - half),
                new Vector3(maxX + 1 - half, heightOffset, maxY + 1 - half),
                new Vector3(maxX + 1 - half, heightOffset, minY - half)
            };

            borderRenderer.loop = true;
            borderRenderer.positionCount = corners.Length;
            borderRenderer.SetPositions(corners);

            borderXMin = corners.Min(c => c.x);
            borderXMax = corners.Max(c => c.x);
            borderZMin = corners.Min(c => c.z);
            borderZMax = corners.Max(c => c.z);
        }

        // === NEW: cell-based exposed-edge approach then stitch edges into loop ===
        private void DrawTightOuterBorder_CellBased()
        {
            // Step 1: collect all exposed edges from each active tile.
            // We represent vertex coords multiplied by 2 (integers) to avoid halves.
            // Each edge stored as undirected canonical pair (min,max).
            var edgeSet = new HashSet<(Vector2Int a, Vector2Int b)>();

            for (var x = 0; x < boardWidth; x++)
            {
                for (var y = 0; y < boardHeight; y++)
                {
                    if (!IsActive(x, y)) continue;

                    var bl = new Vector2Int(2 * x - 1, 2 * y - 1);
                    var tl = new Vector2Int(2 * x - 1, 2 * y + 1);
                    var tr = new Vector2Int(2 * x + 1, 2 * y + 1);
                    var br = new Vector2Int(2 * x + 1, 2 * y - 1);

                    // add exposed edges: if neighbor tile exists and active -> that edge is internal, ignored.
                    if (!IsActive(x - 1, y)) ToggleEdge(edgeSet, tl, bl); // left
                    if (!IsActive(x + 1, y)) ToggleEdge(edgeSet, br, tr); // right
                    if (!IsActive(x, y + 1)) ToggleEdge(edgeSet, tr, tl); // top
                    if (!IsActive(x, y - 1)) ToggleEdge(edgeSet, bl, br); // bottom
                }
            }

            if (edgeSet.Count == 0)
            {
                borderRenderer.positionCount = 0;
                return;
            }

            // Step 2: build adjacency map for vertices from remaining edges
            var adj = new Dictionary<Vector2Int, List<Vector2Int>>();
            foreach (var e in edgeSet)
            {
                AddAdj(adj, e.a, e.b);
                AddAdj(adj, e.b, e.a);
            }

            // Step 3: walk edges to build the outer loop(s).
            // We'll attempt to build the outermost loop first (start from leftmost-bottommost vertex).
            var loops = new List<List<Vector2Int>>();
            var usedEdges = new HashSet<(Vector2Int, Vector2Int)>(); // directed visited
            var allVertices = adj.Keys.ToList();

            // sort starts so we pick a likely outer start first
            var startCandidates = allVertices.OrderBy(v => v.x).ThenBy(v => v.y).ToList();

            foreach (var start in startCandidates)
            {
                // if all incident edges already used, skip
                var allUsed = true;
                foreach (var nb in adj[start])
                {
                    if (!usedEdges.Contains((start, nb)) && !usedEdges.Contains((nb, start)))
                    {
                        allUsed = false; break;
                    }
                }
                if (allUsed) continue;

                // pick an initial previous such that we approach start from "outside" (a point slightly left)
                var prev = new Vector2Int(start.x - 1, start.y);
                var curr = start;

                var loop = new List<Vector2Int>();
                var safety = 0;
                while (true)
                {
                    safety++;
                    if (safety > edgeSet.Count * 4) break; // safety break

                    loop.Add(curr);

                    // choose next neighbor:
                    var next = ChooseNextNeighbor_KeepStraightThenAngle(adj, curr, prev, usedEdges);
                    if (next == new Vector2Int(int.MinValue, int.MinValue)) break;

                    // mark this directed edge visited
                    usedEdges.Add((curr, next));

                    prev = curr;
                    curr = next;

                    if (curr == start) break; // closed
                }

                if (loop.Count >= 3)
                {
                    var simplified = SimplifyColinear(loop);
                    // canonical orientation CCW
                    if (PolygonArea(simplified) < 0) simplified.Reverse();
                    loops.Add(simplified);
                }
            }

            // Step 4: choose the largest area loop as outer border (user wanted the outer outline)
            if (loops.Count == 0)
            {
                borderRenderer.positionCount = 0;
                return;
            }

            var outer = loops.OrderByDescending(l => Mathf.Abs(PolygonArea(l))).First();

            // Step 5: convert to world positions (divide coords by 2)
            var positions = new List<Vector3>(outer.Count);
            foreach (var v in outer)
            {
                var wx = v.x / 2f;
                var wz = v.y / 2f;
                positions.Add(new Vector3(wx, heightOffset, wz));
            }

            borderRenderer.loop = true;
            borderRenderer.positionCount = positions.Count;
            borderRenderer.SetPositions(positions.ToArray());

            borderXMin = positions.Min(p => p.x);
            borderXMax = positions.Max(p => p.x);
            borderZMin = positions.Min(p => p.z);
            borderZMax = positions.Max(p => p.z);
        }

        // Toggle undirected edge: add if missing, remove if present (internal edges cancel)
        private void ToggleEdge(HashSet<(Vector2Int, Vector2Int)> set, Vector2Int a, Vector2Int b)
        {
            var key = (a.x < b.x || (a.x == b.x && a.y <= b.y)) ? (a, b) : (b, a);
            if (set.Contains(key)) set.Remove(key);
            else set.Add(key);
        }

        private void AddAdj(Dictionary<Vector2Int, List<Vector2Int>> adj, Vector2Int from, Vector2Int to)
        {
            if (!adj.TryGetValue(from, out var list))
            {
                list = new List<Vector2Int>();
                adj[from] = list;
            }
            if (!list.Contains(to)) list.Add(to);
        }

        // Prefer continuing straight (collinear) if possible; otherwise pick the neighbor with smallest CCW angle from incoming dir.
        private Vector2Int ChooseNextNeighbor_KeepStraightThenAngle(Dictionary<Vector2Int, List<Vector2Int>> adj, Vector2Int curr, Vector2Int prev, HashSet<(Vector2Int, Vector2Int)> usedEdges)
        {
            if (!adj.TryGetValue(curr, out var neighbors) || neighbors.Count == 0)
                return new Vector2Int(int.MinValue, int.MinValue);

            // compute incoming direction vector (curr - prev)
            var inVec = new Vector2Int(curr.x - prev.x, curr.y - prev.y);

            // candidate neighbors that still have that edge unused
            var candidates = new List<Vector2Int>();
            foreach (var n in neighbors)
            {
                if (usedEdges.Contains((curr, n)) || usedEdges.Contains((n, curr))) // if edge already used in either direction, prefer not to reuse
                    continue;
                candidates.Add(n);
            }
            if (candidates.Count == 0)
            {
                // if all edges used, allow reuse to try close loop
                candidates.AddRange(neighbors);
            }

            // check for exact straight continuation (colinear and same direction)
            foreach (var n in candidates)
            {
                var outVec = new Vector2Int(n.x - curr.x, n.y - curr.y);
                if (IsColinearAndSameDir(inVec, outVec))
                    return n;
            }

            // else choose by smallest CCW angle from incoming direction
            var incomingAngle = Mathf.Atan2(inVec.y, inVec.x);
            var bestDelta = float.MaxValue;
            var best = candidates[0];

            foreach (var n in candidates)
            {
                var outVec = new Vector2Int(n.x - curr.x, n.y - curr.y);
                var candAngle = Mathf.Atan2(outVec.y, outVec.x);
                var delta = candAngle - incomingAngle;
                while (delta < 0) delta += Mathf.PI * 2f;
                while (delta >= Mathf.PI * 2f) delta -= Mathf.PI * 2f;
                if (delta < bestDelta)
                {
                    bestDelta = delta;
                    best = n;
                }
            }

            return best;
        }

        private bool IsColinearAndSameDir(Vector2Int a, Vector2Int b)
        {
            var cross = (long)a.x * b.y - (long)a.y * b.x;
            if (cross != 0) return false;
            // dot > 0 => same direction
            var dot = (long)a.x * b.x + (long)a.y * b.y;
            return dot > 0;
        }

        private List<Vector2Int> SimplifyColinear(List<Vector2Int> poly)
        {
            var res = new List<Vector2Int>();
            var n = poly.Count;
            for (var i = 0; i < n; i++)
            {
                var prev = poly[(i - 1 + n) % n];
                var curr = poly[i];
                var next = poly[(i + 1) % n];

                var v1 = new Vector2Int(curr.x - prev.x, curr.y - prev.y);
                var v2 = new Vector2Int(next.x - curr.x, next.y - curr.y);

                var cross = (long)v1.x * v2.y - (long)v1.y * v2.x;
                if (cross != 0)
                    res.Add(curr);
            }
            return res.Count > 0 ? res : new List<Vector2Int>(poly);
        }

        private float PolygonArea(List<Vector2Int> poly)
        {
            long sum = 0;
            var n = poly.Count;
            for (var i = 0; i < n; i++)
            {
                var a = poly[i];
                var b = poly[(i + 1) % n];
                sum += (long)a.x * b.y - (long)b.x * a.y;
            }
            return sum * 0.5f;
        }

        private bool IsActive(int x, int y)
        {
            if (!BoardUtils.VerifyBounds(x) || !BoardUtils.VerifyBounds(y))
                return false;
            return ActiveBoard[IndexOf(x, y)];
        }

        private void DrawMiddleLine()
        {
            // middle X: center of the border extents (so midline sits inside the border)
            float midX;
            if (float.IsPositiveInfinity(borderXMin) || float.IsPositiveInfinity(borderXMax))
            {
                // fallback: center of min/max indices
                var half = 0.5f;
                midX = (minX + maxX + 1) / 2f - half;
            }
            else
            {
                midX = (borderXMin + borderXMax) * 0.5f;
            }

            var start = new Vector3(midX, heightOffset, borderZMin);
            var end = new Vector3(midX, heightOffset, borderZMax);

            midLineRenderer.positionCount = 2;
            midLineRenderer.SetPosition(0, start);
            midLineRenderer.SetPosition(1, end);
        }

        private int IndexOf(int x, int y)
        {
            return BoardUtils.IndexOf(x, y);
        }
    }
}

using UnityEngine;
using System.Collections;
using Game.Managers;
using Game.Common;

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

        [Header("Middle Line Settings")]
        public UnityEngine.Color middleLineColor = UnityEngine.Color.red;
        public float middleLineWidth = 0.05f;

        [SerializeField] private LineRenderer borderRenderer;
        [SerializeField] private LineRenderer midLineRenderer;

        private int minX, maxX, minY, maxY;

        void Awake()
        {
            borderRenderer.useWorldSpace = true;
            borderRenderer.loop = true;
            borderRenderer.startWidth = lineWidth;
            borderRenderer.endWidth = lineWidth;
            borderRenderer.material = new Material(Shader.Find("Sprites/Default"));
            borderRenderer.startColor = lineColor;
            borderRenderer.endColor = lineColor;

            midLineRenderer.useWorldSpace = true;
            midLineRenderer.loop = false;
            midLineRenderer.startWidth = middleLineWidth;
            midLineRenderer.endWidth = middleLineWidth;
            midLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            midLineRenderer.startColor = middleLineColor;
            midLineRenderer.endColor = middleLineColor;
        }

        void Start()
        {
            UpdateBorder();
        }

        public void UpdateBorder()
        {
            if (ActiveBoard == null || ActiveBoard.Length == 0)
            {
                borderRenderer.positionCount = 0;
                midLineRenderer.positionCount = 0;
                return;
            }

            minX = boardWidth; maxX = -1; minY = boardHeight; maxY = -1;

            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    int index = IndexOf(x, y);
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

            float half = 0.5f;

            Vector3[] corners = new Vector3[4];
            corners[0] = new Vector3(minX - half, heightOffset, minY - half);
            corners[1] = new Vector3(minX - half, heightOffset, maxY + 1 - half);
            corners[2] = new Vector3(maxX + 1 - half, heightOffset, maxY + 1 - half);
            corners[3] = new Vector3(maxX + 1 - half, heightOffset, minY - half);

            borderRenderer.positionCount = corners.Length;
            borderRenderer.SetPositions(corners);

            DrawMiddleLine();
        }

        private void DrawMiddleLine()
        {
            float half = 0.5f;
            float midX = (minX + maxX + 1) / 2f - half;

            Vector3 start = new Vector3(midX, heightOffset, minY - half);
            Vector3 end = new Vector3(midX, heightOffset, maxY + 1 - half);

            midLineRenderer.positionCount = 2;
            midLineRenderer.SetPosition(0, start);
            midLineRenderer.SetPosition(1, end);
        }

        private int IndexOf(int x, int y)
        {
            return y * boardWidth + x;
        }
    }
}
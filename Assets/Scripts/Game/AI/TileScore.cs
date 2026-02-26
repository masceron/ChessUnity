using TMPro;
using UnityEngine;

namespace Game.AI
{
    public class TileScore : MonoBehaviour
    {
        [SerializeField] private int score;
        [SerializeField] private TMP_Text scoreText;

        public void SetScore(int s)
        {
            score = s;
            scoreText.text = score.ToString();
        }

        public void SetPosition(int rank, int file)
        {
            transform.localPosition = new Vector3(rank - 20f, file - 20f, -0.2f);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
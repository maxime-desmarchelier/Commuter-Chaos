using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script
{
    public class ScoreBoardManager : MonoBehaviour
    {
        private string _nextLevel;

        [SerializeField] private GameObject oneStar;
        [SerializeField] private GameObject twoStar;
        [SerializeField] private GameObject threeStar;
        [SerializeField] private TMP_Text textResult;

        // Start is called before the first frame update
        private void Start(){
            _nextLevel = GameController.Instance.GetNextLevel();


            var score = (GameController.Instance.Score / GameController.Instance.NbFraudster) * 100;

            if (score >= 0)
            {
                oneStar.SetActive(true);
                textResult.text = ("YOU FAILED");
            }

            if (score >= 33)
            {
                twoStar.SetActive(true);
                textResult.text = ("YOU FAILED");
            }

            if (score >= 66)
            {
                threeStar.SetActive(true);
                textResult.text = ("YOU PASSED");
            }

            if (_nextLevel == null) GameObject.Find("NEXT").SetActive(false);
            if (score < 66) GameObject.Find("NEXT").SetActive(false);
        }

        // Update is called once per frame
        private void Update(){
        }

        public void Restart(){
            SceneManager.LoadScene(GameController.Instance.Level);
        }

        public void Menu(){
            SceneManager.LoadScene("Menu");
        }

        public void Next(){
            SceneManager.LoadScene(_nextLevel);
        }
    }
}
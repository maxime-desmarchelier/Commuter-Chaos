using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBoardManager : MonoBehaviour
{
    private string _nextLevel;

    // Start is called before the first frame update
    private void Start(){
        _nextLevel = GameController.Instance.GetNextLevel();
        if (_nextLevel == null) GameObject.Find("NEXT").SetActive(false);
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
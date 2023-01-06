using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBoardManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start(){
    }

    // Update is called once per frame
    private void Update(){
    }

    public void Restart(){
        Debug.Log("Restart");
    }

    public void Menu(){
        SceneManager.LoadScene("Menu");
    }

    public void Next(){
        Debug.Log("Next");
    }
}
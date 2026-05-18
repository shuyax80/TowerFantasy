using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadScene(int index)
    {
        Debug.Log(index);
        SceneManager.LoadSceneAsync(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("levelSwitcher");
    }

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}

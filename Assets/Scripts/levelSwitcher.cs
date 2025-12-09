using UnityEngine;
using UnityEngine.SceneManagement;

public class levelSwitcher : MonoBehaviour
{
    public void TutorialLevel()
    {
        Debug.Log("Tutorial Level");
        SceneManager.LoadScene("tutorialLevel");
    }

    public void level1()
    {
        Debug.Log("Level 1");
        SceneManager.LoadScene("level1");
    }

    public void level2()
    {
        Debug.Log("Level 2");
        SceneManager.LoadScene("level2");
    }

    public void level3()
    {
        Debug.Log("Level 3");
        SceneManager.LoadScene("level3");
    }

    public void level4()
    {
        Debug.Log("Level 4");
        SceneManager.LoadScene("level4");
    }

    public void Back()
    {
        Debug.Log("Back");
        SceneManager.LoadScene("menu");
    }
}

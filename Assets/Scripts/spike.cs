using UnityEngine;
using UnityEngine.SceneManagement;

public class spike : MonoBehaviour
{
    public string playerTag = "Player";

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player hit spike");

        if (collision.gameObject.CompareTag(playerTag))
        {
            Debug.Log("Respawning player");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

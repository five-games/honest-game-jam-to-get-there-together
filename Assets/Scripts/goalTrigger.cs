using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class playerCollector : MonoBehaviour
{
    public int needCount = 2;
    public string playerTag = "Player";
    public bool pauseGameOnWin = true;
    public int nextLevel = -1;
    public static string levelPrefix = "level";
    public string levelName;

    public TextMeshProUGUI missingFollowersText;
    private Coroutine currentMessageCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (missingFollowersText == null)
        {
            Debug.LogError($"[{name}] missingFollowersText is not assigned on {GetType().Name}. The missing-followers text will not display.");
            return;
        }

        if (missingFollowersText != null)
       {
           var c = missingFollowersText.color;
           c.a = 0f;
           missingFollowersText.color = c;
           missingFollowersText.gameObject.SetActive(false);

           missingFollowersText.fontSize = 65;
           missingFollowersText.rectTransform.sizeDelta = new Vector2(1000, 300);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        int currentCount = 0;
        foreach (var follower in followerChain.followers)
        {
            if (IsFollowingPlayer(follower, other.transform))
            {
                currentCount++;
            }
        }

        Debug.Log($"Goal: hráè vstoupil. NPC sledujících hráèe: {currentCount}/{needCount}");

        if (currentCount >= needCount)
        {
            Win();
        }
        else
        {
            int missing = needCount - currentCount;
            Debug.Log($"Chybí {missing} NPC do cíle.");

            if (missingFollowersText != null)
            {
                if (missing == 1)
                {
                    missingFollowersText.text = $"You still need {missing} NPC!";
                } else
                {
                    missingFollowersText.text = $"You still need {missing} NPCs!";
                }

                if (currentMessageCoroutine != null)
                {
                    StopCoroutine(currentMessageCoroutine);
                    currentMessageCoroutine = null;
                }

                currentMessageCoroutine = StartCoroutine(ShowMissingFollowersMessage());
            }
        }
    }



    private bool IsFollowingPlayer(followerChain f, Transform player)
    {
        if (f == null || player == null) return false;

        Transform t = f.followTarget;
        int safety = 0; // ochrana proti nekoneèné smyèce

        // Projdeme odkazy followTarget až najdeme hráèe nebo konec øetìzce
        while (t != null && safety++ < 50)
        {
            if (t == player) return true;

            // pokud followTarget odkazuje na dalšího followera, pokraèuj dál
            var next = t.GetComponent<followerChain>();
            if (next == null) break;

            t = next.followTarget;
        }

        return false;
    }

    private void Win()
    {
        Debug.Log("Cil dosazen - vyhral jsi!");
        if (pauseGameOnWin)
        {
            Time.timeScale = 0f;
            return;
        }

        if (nextLevel == -1)
        {
            Debug.Log("Neni definovan dalsi level.");
            Time.timeScale = 0f;
            return;
        }

        if (nextLevel == 99)
        {
            Debug.Log("Posledni level dokoncen");
            UnityEngine.SceneManagement.SceneManager.LoadScene("thxForPlaying");
            return;
        }

        else
        {
            Debug.Log($"Nacteni dalsiho levelu: {nextLevel}");
            levelName = levelPrefix + nextLevel;
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
        }
    }

    private IEnumerator FadeText(TextMeshProUGUI text, float duration, float startAlpha, float endAlpha)
    {
        if (text == null) yield break;

        float time = 0f;
        Color c = text.color;

        c.a = startAlpha;
        text.color = c;

        if (duration <= 0f)
        {
            c.a = endAlpha;
            text.color = c;
            yield break;
        }

        while (time < duration)
        {
            float t = time / duration;
            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            text.color = c;
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        c.a = endAlpha;
        text.color = c;
    }

    private IEnumerator ShowMissingFollowersMessage()
    {
        if (missingFollowersText == null) yield break;

        Debug.Log($"[playerCollector] showing missingFollowersText (current color): {missingFollowersText.color}");

        missingFollowersText.color = new Color(1f, 1f, 1f, 0f);
        missingFollowersText.gameObject.SetActive(true);

        yield return FadeText(missingFollowersText, 0.4f, 0f, 1f);
        yield return new WaitForSecondsRealtime(1.4f);
        yield return FadeText(missingFollowersText, 0.4f, 1f, 0f);
        missingFollowersText.gameObject.SetActive(false);
        currentMessageCoroutine = null;
    }

}

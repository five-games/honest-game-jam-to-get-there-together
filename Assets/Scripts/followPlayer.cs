using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 15f;
    public float followDistance = 0.5f;
    public Transform defaultPos;
    private bool shouldFollow = false;
    private bool sfxPlayed = false;


    private void Update()
    {
        if (shouldFollow && player != null)
        {
            float sign;
            if (Mathf.Approximately(player.localScale.x, 0f))
                sign = -1f; // fallback
            else
                sign = (player.localScale.x >= 0f) ? 1f : -1f;

            Vector3 desiredPosition = player.position + new Vector3(-sign * followDistance, 0f, 0f);

            float distance = Vector2.Distance(transform.position, desiredPosition);

            if (distance > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    desiredPosition,
                    followSpeed * Time.deltaTime
                );
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            shouldFollow = true;

            var p = other.GetComponent<player>();
            if (p != null && !sfxPlayed)
            {
                p.NpcCollected();
                sfxPlayed = true;
            }
        }
    }
}

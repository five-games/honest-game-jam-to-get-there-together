using UnityEngine;

public class projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 6f;
    public string playerTag = "Player";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject , lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            var p = other.GetComponent<player>();
            if (p != null)
            {
                p.HitByProjectile();
            }

            HitPlayer();
            return;
        }

        if (other.CompareTag("NPC"))
        {
            HitNPC(other);
            return;
        }
        
    }
    public void HitPlayer()
    {
        Destroy(gameObject);
        Debug.Log("HIT PLAYER");

        followerChain.BreakOffAll();
    }

    public void HitNPC(Collider2D npcCollider)
    {
        followerChain npc = npcCollider.GetComponent<followerChain>();
        if (npc == null)
        {
            Debug.LogWarning("HitNPC: objekt nema followerChain ");
            Destroy(gameObject);
            return;
        }

        if (npc.isFollowing)
        {
            Debug.Log("Hit NPC ktere sleduje hrace");
            followerChain.BreakOffAll();
            Destroy(gameObject);
            return;
        }
        Debug.Log("hit npc ktere nesleduje hrace");
        Destroy(gameObject);
    }
}

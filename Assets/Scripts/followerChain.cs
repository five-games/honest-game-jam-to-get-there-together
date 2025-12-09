using UnityEngine;
using System.Collections.Generic;

// brecim, ale funguje to (diky copilote)

public class followerChain : MonoBehaviour
{
    public static List<followerChain> followers = new List<followerChain>();

    public Transform followTarget;
    public float followSpeed = 3f;
    public float followDistance = 1.2f;
    public float returnSpeed = 4f;

    public bool isFollowing = false;
    private bool IsReturning = false;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }
    private void Awake()
    {
        followers.Add(this);
    }

    private void OnDestroy()
    {
        followers.Remove(this);
    }

    private void Update()
    {
        if (IsReturning)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, originalPosition) < 0.05f)
            {
                IsReturning = false;
            }
        }

        if (!isFollowing || followTarget == null)
            return;

        // Compute the desired follow position (keeps the follower at followDistance from the target)
        Vector3 desiredPosition = GetDesiredPosition(followTarget);

        // Move smoothly toward the desired position instead of moving directly to the target's pivot.
        float distance = Vector2.Distance(transform.position, desiredPosition);

        // A small epsilon prevents stuttering when already at the correct spot.
        if (distance > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                desiredPosition,
                followSpeed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFollowing)
        {
            JoinChain(other.transform);
        }
    }

    /* private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isFollowing)
        {
            JoinChain(collision.transform);
        }
    } */

    // pripoji se do retezce nasledovniku
    private void JoinChain(Transform player)
    {
        // Count how many followers are already active BEFORE marking this one as following.
        int activeFollowers = CountActiveFollowers();

        // If none are following, follow the player; otherwise follow the last active follower.
        if (activeFollowers == 0)
        {
            followTarget = player;
        }
        else
        {
            var last = GetLastActiveFollower();
            followTarget = (last != null) ? last.transform : player;
        }

        // Mark this follower as active after determining the correct follow target.
        isFollowing = true;

        // Do NOT teleport here. Let Update() smoothly move the follower to the desired offset position.
    }

    private Vector3 GetDesiredPosition(Transform target)
    {
        if (target == null)
            return transform.position;

        // Determine facing sign from target's localScale.x if available; fall back to world left.
        float sign = 1f;
        if (!Mathf.Approximately(target.localScale.x, 0f))
            sign = (target.localScale.x >= 0f) ? 1f : -1f;
        else
            sign = -1f;

        // Place follower behind the target along the local X axis (2D side-scroller assumption).
        Vector3 offset = new Vector3(-sign * followDistance, 0f, 0f);
        return target.position + offset;
    }

    private int CountActiveFollowers()
    {
        // kolik NPC aktualne sleduje hrace
        int count = 0;
        foreach (var f in followers)
        {
            if (f.isFollowing)
                count++;
        }
        return count;
    }

    // najde posledni NPC
    private followerChain GetLastActiveFollower()
    {
        followerChain last = null;
        foreach (var f in followers)
        {
            if (f.isFollowing)
                last = f;
        }
        return last;
    }

    private void BrakeOff()
    {
        isFollowing = false;
        followTarget = null;
        IsReturning = true;
    }

    public static void BreakOffAll()
    {
        foreach (var f in followers)
        {
            f.BrakeOff();
        }
    }
}

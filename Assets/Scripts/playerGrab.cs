using Unity.VisualScripting;
using UnityEngine;


public class PlayerGrabRay : MonoBehaviour
{
    public Transform grabDetect;
    public Transform boxHolder;
    public float rayDist;
    public string grabbableTag = "Objects";
    public KeyCode grabBind = KeyCode.Mouse0;

    void Update()
    {
        RaycastHit2D grabCheck = Physics2D.Raycast(grabDetect.position, Vector2.right * transform.localScale, rayDist);
    
        if (grabCheck.collider != null && grabCheck.collider.tag == grabbableTag)
        {
            if (Input.GetKey(grabBind))
            {
                grabCheck.collider.gameObject.transform.parent = boxHolder;
                grabCheck.collider.gameObject.transform.position = boxHolder.position;
                grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            }

            else
            {
                grabCheck.collider.gameObject.transform.parent = null;
                grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }

    }

    

}
using UnityEngine;

public class TriggerHurt : MonoBehaviour
{
    public bool killInstantly = false;
    public float damage = 25.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("hit player");
        }
    }
    
}

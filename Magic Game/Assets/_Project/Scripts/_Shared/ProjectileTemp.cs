using UnityEngine;

public class ProjectileTemp : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private float speed                = 1.0f;
    [SerializeField] private float damage               = 50.0f;
    [SerializeField] private float aliveTime            = 10.0f;
    [SerializeField] private float manaCost             = 10.0f;
    [SerializeField] private LayerMask physicsLayerMask = 1;
    [SerializeField] private GameObject explosion       = null;

    private bool hitSomething                           = false;
    private GameObject hitTarget                        = null;
    private GameObject owner                            = null;
    private Vector3 direction                           = Vector3.zero;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Start()
    {
        if (direction == Vector3.zero)
        {
            Debug.Log("Projectile instantiated with no direction! De-spawning...");
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(
            transform.position,
            direction,
            out hit,
            speed * Time.deltaTime,
            physicsLayerMask
            ))
        {
            if (hit.transform.gameObject != owner)
            {
                transform.position = hit.point;
                hitSomething = true;
                hitTarget = hit.transform.gameObject;
                Destroy(this.gameObject);
            }
        }
        else
        {
            transform.position += direction * speed * Time.deltaTime;
        }

        if (aliveTime > 0.0f)
        {
            aliveTime -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        if (explosion != null && hitSomething)
        {
            if (hitTarget.GetComponent<Health>() != null)
            {
                hitTarget.GetComponent<Health>().Hurt(damage, false);
            }
            Instantiate(explosion, this.transform.position, Quaternion.identity, null);
        }
    }

    #endregion

    #region CUSTOM_METHODS

    public void Initialize(Vector3 spawnPosition, Vector3 spawnDirection, GameObject caller)
    {
        owner = caller;
        transform.position = spawnPosition;
        direction = spawnDirection;
        transform.rotation = Quaternion.LookRotation(spawnDirection, Vector3.up);
    }

    public float GetManaCost()
    {
        return manaCost;
    }

    #endregion
}

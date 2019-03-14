using UnityEngine;

[RequireComponent(typeof(PlayerCore))]
[RequireComponent(typeof(Mana))]
public class PlayerSpellCaster : MonoBehaviour
{
    #region VARIABLES

    public GameObject projectile                        = null;

    [SerializeField] private GameObject blockedReticle  = null;
    [SerializeField] private LineRenderer blockedLine   = null;

    private bool bIsEnabled                             = true;
    private Mana cMana                                  = null;
    private LayerMask physicsLayerMask                  = 1;
    private new Transform camera                        = null;
    private Vector3 charPositionOffset                  = Vector3.up * 1.0f;
    private Vector3 castPoint                           = Vector3.zero;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Start()
    {
        physicsLayerMask    = GetComponent<PlayerCore>().physicsLayerMask;
        camera              = Camera.main.transform;
        cMana               = GetComponent<Mana>();
    }

    void Update()
    {
        if (bIsEnabled && camera != null)
        {
            RaycastHit hitFromCamera;
            RaycastHit hitFromPlayer;

            if (!Physics.Raycast(
                camera.position,
                camera.forward,
                out hitFromCamera,
                Mathf.Infinity,
                physicsLayerMask
                ))
            {
                hitFromCamera.point = transform.position + charPositionOffset + (camera.position + camera.forward * 5000.0f);
            }

            if (blockedReticle != null && blockedLine != null)
            {
                if (Physics.Raycast(
                transform.position + charPositionOffset,
                -Vector3.Normalize(transform.position + charPositionOffset - hitFromCamera.point),
                out hitFromPlayer,
                Mathf.Infinity,
                physicsLayerMask
                ))
                {
                    castPoint = hitFromPlayer.point;
                    if (AlmostEqual(hitFromPlayer.point, hitFromCamera.point, 0.05f))
                    {
                        blockedReticle.SetActive(false);
                        blockedLine.enabled = false;
                    }
                    else
                    {
                        blockedReticle.SetActive(true);
                        blockedReticle.transform.position = hitFromPlayer.point + hitFromPlayer.normal * 0.01f;
                        blockedReticle.transform.rotation = Quaternion.LookRotation(hitFromPlayer.normal, Vector3.up);

                        blockedLine.enabled = true;
                        blockedLine.SetPosition(0, transform.position + charPositionOffset);
                        blockedLine.SetPosition(1, hitFromPlayer.point);
                    }
                }
                else
                {
                    castPoint = hitFromCamera.point;
                    blockedReticle.SetActive(false);
                    blockedLine.enabled = false;
                }
            }
        }
    }

    public void CastBeamActive(bool b)
    {
        bIsEnabled = b;
        blockedReticle.SetActive(b);
        blockedLine.enabled = b;
    }

    #endregion

    #region CUSTOM_METHODS

    public void CastSpell()
    {
        if (projectile != null)
        {
            if (projectile.GetComponent<ProjectileTemp>().GetManaCost() < cMana.mana)
            {
                Vector3 direction = -Vector3.Normalize(transform.position + charPositionOffset - castPoint);
                ProjectileTemp spell = Instantiate(projectile).GetComponent<ProjectileTemp>();
                spell.Initialize(transform.position + charPositionOffset, direction, this.gameObject);
                cMana.UseMana(spell.GetManaCost());
            }
        }
    }

    bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;
        if (Mathf.Abs(Vector3.Angle(v1, v2)) > precision) { equal = false; }
        return equal;
    }

    #endregion
}

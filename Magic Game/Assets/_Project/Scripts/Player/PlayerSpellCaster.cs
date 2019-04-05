using UnityEngine;

public class PlayerSpellCaster : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private Transform cameraTransform = null;
    [SerializeField] private Transform spellCastTransform = null;
    [SerializeField] private GameObject blockedReticle = null;
    [SerializeField] private LineRenderer blockedLine = null;
    [SerializeField] private bool lineAlwaysEnabled = true;

    public Vector3 castPoint { get; private set; } = Vector3.zero;

    private bool bIsEnabled = true;
    private LayerMask physicsLayerMask = 1;
    private Vector3 charPositionOffset = Vector3.up * 1.0f;

    #endregion

    #region UNITY_DEFAULT_METHODS

    //void Start()
    //{
    //    physicsLayerMask = GetComponent<PlayerCore>().physicsLayerMask;
    //}

    void Update()
    {
        if (bIsEnabled && cameraTransform != null)
        {
            if (cameraTransform != null && spellCastTransform != null)
            {
                RaycastHit hitFromCamera;
                RaycastHit hitFromSpellCast;

                if (!Physics.Raycast(
                    cameraTransform.position,
                    cameraTransform.forward,
                    out hitFromCamera,
                    Mathf.Infinity,
                    physicsLayerMask
                    ))
                {
                    hitFromCamera.point = cameraTransform.position + cameraTransform.forward * 1000.0f;
                }

                if (blockedReticle != null && blockedLine != null)
                {
                    if (Physics.Raycast(
                    spellCastTransform.position,
                    -Vector3.Normalize(spellCastTransform.position - hitFromCamera.point),
                    out hitFromSpellCast,
                    Mathf.Infinity,
                    physicsLayerMask
                    ))
                    {
                        castPoint = hitFromSpellCast.point;
                        if (AlmostEqual(hitFromSpellCast.point, hitFromCamera.point, 0.1f))
                        {
                            blockedReticle.SetActive(false);
                            blockedLine.enabled = false;
                        }
                        else
                        {
                            blockedReticle.SetActive(true);
                            blockedReticle.transform.position = hitFromSpellCast.point + hitFromSpellCast.normal * 0.01f;
                            blockedReticle.transform.rotation = Quaternion.LookRotation(hitFromSpellCast.normal, Vector3.up);

                            blockedLine.enabled = true;
                            blockedLine.SetPosition(0, spellCastTransform.position);
                            blockedLine.SetPosition(1, hitFromSpellCast.point);
                        }
                    }
                    else
                    {
                        castPoint = hitFromCamera.point;
                        blockedReticle.SetActive(false);
                        blockedLine.enabled = false;
                    }
                }

                if (lineAlwaysEnabled)
                {
                    blockedLine.enabled = true;
                    blockedLine.SetPosition(0, spellCastTransform.position);
                    blockedLine.SetPosition(1, castPoint);
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

    //public void CastSpell()
    //{
    //    if (projectile != null)
    //    {
    //        if (projectile.GetComponent<ProjectileTemp>().GetManaCost() < cMana.mana)
    //        {
    //            Vector3 direction = -Vector3.Normalize(transform.position + charPositionOffset - castPoint);
    //            ProjectileTemp spell = Instantiate(projectile).GetComponent<ProjectileTemp>();
    //            spell.Initialize(transform.position + charPositionOffset, direction, this.gameObject);
    //            cMana.UseMana(spell.GetManaCost());
    //            GetComponent<PlayerAnimations>().CastSpell(0);
    //        }
    //    }
    //}

    bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;
        if (Mathf.Abs(Vector3.Angle(v1, v2)) > precision) { equal = false; }
        return equal;
    }

    #endregion
}

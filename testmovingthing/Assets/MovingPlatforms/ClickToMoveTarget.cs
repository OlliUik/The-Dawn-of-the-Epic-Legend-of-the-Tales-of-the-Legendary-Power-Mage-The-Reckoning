using UnityEngine;
using UnityEngine.AI;

// Use physics raycast hit from mouse click to set agent destination
[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMoveTarget : MonoBehaviour
{
    NavMeshAgent m_Agent;
    RaycastHit m_HitInfo = new RaycastHit();
    public Transform target;
    public Vector3 hit;

    void Start()
    {
        {
            //hit = target.position;
        }
        m_Agent = GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
            target.position = m_HitInfo.point;
            target.transform.SetParent(m_HitInfo.transform);
            hit = m_HitInfo.point;
            m_Agent.destination = target.position;
        }

        if (hit != Vector3.zero)
        {
            float distance = Vector3.Distance(hit, target.position);
            if (distance > 0.25f)
            {
                hit = target.position;
                m_Agent.destination = target.position;
            }

        }

    }
}

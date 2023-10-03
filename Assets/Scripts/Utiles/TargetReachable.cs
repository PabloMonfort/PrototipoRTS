using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class TargetReachable : MonoBehaviour
{
    public Transform target;
    NavMeshQuery m_NavQuery;
    NavMeshHit m_Hit;

    void OnEnable()
    {
        m_NavQuery = new NavMeshQuery(NavMeshWorld.GetDefaultWorld(), Allocator.Persistent);
    }

    void Update()
    {
        var startLocation = m_NavQuery.MapLocation(transform.position, Vector3.one, 0);
        var status = m_NavQuery.Raycast(out m_Hit, startLocation, target.position, NavMesh.AllAreas, new NativeArray<float>());
        if ((status & PathQueryStatus.Success) != 0)
        {
            Debug.DrawLine(transform.position, target.position, m_Hit.hit ? Color.red : Color.green);

            if (m_Hit.hit)
                Debug.DrawRay(m_Hit.position, Vector3.up, Color.red);
        }
    }

    void OnDisable()
    {
        m_NavQuery.Dispose();
    }
}
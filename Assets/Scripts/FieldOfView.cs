using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class FieldOfView : MonoBehaviour {

    [SerializeField]
    private WaypointMovement m_waypointMovement;

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDistanceThreshold;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public Material defaultMaterial;
    public Material detectedMaterial;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    public List<Transform> visibleTargets = new List<Transform>();
    public float maxSpeed = 10.0f;

    void Start()
    {
        m_waypointMovement = GetComponent<WaypointMovement>();

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine("findTargetWithDelay", 0.02f);
    }

    void LateUpdate()
    {
        drawFieldOfView();
        if(visibleTargets.Count > 0)
        {
            m_waypointMovement.targetSighted = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, Utils.RotateToTarget(transform.position, visibleTargets[0].position), 20 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, visibleTargets[0].position, Time.deltaTime * maxSpeed);
        } else
        {
            m_waypointMovement.targetSighted = false;
        }
    }

    IEnumerator findTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            findVisibleTargets();
        }
    }

    void findVisibleTargets()
    {
        visibleTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        foreach(Collider2D col in targetsInViewRadius)
        {
            Transform target = col.transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                
                if(!Physics2D.Raycast (transform.position, dirToTarget, distanceToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    void drawFieldOfView()
    {

        if(visibleTargets.Count > 0)
        {
            Renderer renderer = viewMeshFilter.GetComponent<Renderer>();
            renderer.material = detectedMaterial;
        } else
        {
            Renderer renderer = viewMeshFilter.GetComponent<Renderer>();
            renderer.material = defaultMaterial;
        }

        int rayCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float rayAngleSize = viewAngle / rayCount;

        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for(int i = 0; i <= rayCount; i++)
        {
            float angle = -transform.eulerAngles.z - viewAngle / 2 + rayAngleSize * i;
            ViewCastInfo newViewCast = viewCast(angle);

            if(i > 0)
            {
                bool edgeThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                if(oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeThresholdExceeded))
                {
                    EdgeInfo edge = findEdge(oldViewCast, newViewCast);
                    if(edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA); 
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.end);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for(int i = 0; i < vertexCount - 1; ++i)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo findEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for(int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = viewCast(angle);

            bool edgeThresholdExceeded = Mathf.Abs(minViewCast.distance - maxViewCast.distance) > edgeDistanceThreshold;
            if (newViewCast.hit == minViewCast.hit && edgeThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.end;
            } else
            {
                maxAngle = angle;
                maxPoint = newViewCast.end;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo viewCast(float globalAngle)
    {
        Vector3 direction = DirFromAngle(globalAngle, true);
        RaycastHit2D hit;

        if(hit = Physics2D.Raycast(transform.position, direction, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        } else
        {
            return new ViewCastInfo(false, transform.position + direction * viewRadius, viewRadius, globalAngle);
        }

    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees -= transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }
    
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 end;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _end, float _distance, float _angle)
        {
            hit = _hit;
            end = _end;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 a, Vector3 b)
        {
            pointA = a;
            pointB = b;
        }
    }

}

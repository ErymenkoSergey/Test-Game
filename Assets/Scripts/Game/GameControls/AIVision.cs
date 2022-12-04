using Morkwa.Interface;
using Morkwa.Mechanics.CommonBehaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Morkwa.Mechanics.AI
{
    public sealed class AIVision : CommonBehaviour, IVisions
    {
        private float _distanceView;
        private float _angleView;

        [SerializeField] private MeshFilter _viewMeshFilter;
        [SerializeField] private Mesh _viewMesh;

        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private LayerMask _obstacleLayerMask;

        private List<Transform> _targets = new List<Transform>();

        private float _meshResolution = 1f;
        private int _edgeResolveIterations = 5;
        private float _edgeDistanceThreshold = 0.5f;
        private bool IsSeeing;
        private float _timeDelay = 0.2f;

        private void FixedUpdate()
        {
            DrawFieldOfView();
        }

        public void SetVision(float distance, float angle)
        {
            _distanceView = distance;
            _angleView = angle;

            CreateMeshConfig();
            StartCoroutine(FindTargetsWithDelay());
        }

        private void CreateMeshConfig()
        {
            _viewMesh = new Mesh();
            _viewMesh.name = "ViewMesh";
            _viewMeshFilter.mesh = _viewMesh;
        }

        private IEnumerator FindTargetsWithDelay()
        {
            while (true)
            {
                FindVisibleTargets();
                yield return new WaitForSeconds(_timeDelay);
            }
        }

        private void FindVisibleTargets()
        {
            _targets.Clear();

            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, _distanceView, _targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dirToTarget) < _angleView)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, _obstacleLayerMask))
                    {
                        _targets.Add(target);
                        IsSeeing = true;
                    }
                }
            }
        }

        private void DrawFieldOfView()
        {
            int stepCount = Mathf.RoundToInt(_angleView * _meshResolution);

            float stepAngleSize = _angleView / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();
            ViewCastInfo oldViewCast = new ViewCastInfo();

            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.y - _angleView + stepAngleSize * i;
                ViewCastInfo newViewCast = ViewCast(angle);

                if (i > 0)
                {
                    bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast._distance - newViewCast._distance) > _edgeDistanceThreshold;

                    if (oldViewCast._hit != newViewCast._hit || (oldViewCast._hit && newViewCast._hit && edgeDistanceThresholdExceeded))
                    {
                        AIInfo edge = FindCast(oldViewCast, newViewCast);
                        if (edge.PointFisrt != Vector3.zero)
                            viewPoints.Add(edge.PointFisrt);

                        if (edge.PointSecond != Vector3.zero)
                            viewPoints.Add(edge.PointSecond);
                    }
                }

                viewPoints.Add(newViewCast._point);
                oldViewCast = newViewCast;
            }

            var vertexCount = viewPoints.Count + 1;

            Vector3[] vertices = new Vector3[vertexCount];

            int[] triangles = new int[(vertexCount - 2) * 3];

            vertices[0] = Vector3.zero;

            for (int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }

            _viewMesh.Clear();
            _viewMesh.vertices = vertices;
            _viewMesh.triangles = triangles;
            _viewMesh.RecalculateNormals();
        }

        private AIInfo FindCast(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
        {
            float minAngle = minViewCast._angle;
            float maxAngle = maxViewCast._angle;

            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < _edgeResolveIterations; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = ViewCast(angle);

                bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast._distance - newViewCast._distance) > _edgeDistanceThreshold;
                if (newViewCast._hit == minViewCast._hit && !edgeDistanceThresholdExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast._point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast._point;
                }
            }
            return new AIInfo(minPoint, maxPoint);
        }

        private ViewCastInfo ViewCast(float globalAngle)
        {
            Vector3 direction = DirectionFromAngle(globalAngle, true);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, _distanceView, _obstacleLayerMask))
                return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            else
                return new ViewCastInfo(false, transform.position + direction * _distanceView, _distanceView, globalAngle);
        }

        private Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
                angleInDegrees += transform.eulerAngles.y;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        //public float GetRadiusView()
        //{
        //    return _distanceView;
        //}

        //public float GetViewAngle()
        //{
        //    return _angleView;
        //}

        public void SetStatusIsSeeing(bool status)
        {
            IsSeeing = status;
        }

        public bool GetIsSeeingStatus()
        {
            return IsSeeing;
        }

        public struct ViewCastInfo
        {
            public bool _hit;
            public Vector3 _point;
            public float _distance;
            public float _angle;

            public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
            {
                _hit = hit;
                _point = point;
                _distance = distance;
                _angle = angle;
            }
        }

        public struct AIInfo
        {
            public Vector3 PointFisrt;
            public Vector3 PointSecond;

            public AIInfo(Vector3 pointFisrt, Vector3 pointSecond)
            {
                PointFisrt = pointFisrt;
                PointSecond = pointSecond;
            }
        }
    }
}
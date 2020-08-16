using System.Collections.Generic;
using UnityEngine;

public class ParabolaController : MonoBehaviour
{
    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        float f(float x) => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        float f(float x) => -4 * height * x * x + 4 * height * x;

        var mid = Vector2.Lerp(start, end, t);

        return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
    }

    /// <summary>
    /// Animation Speed
    /// </summary>
    public float Speed = 1;

    /// <summary>
    /// Start of Parabola (At least need three points(three gameobject) of this parent object.)
    /// </summary>
    public GameObject ParabolaRoot;

    /// <summary>
    /// Autostart Animation
    /// </summary>
    public bool Autostart = true;

    /// <summary>
    /// Animate
    /// </summary>
    public bool Animation = true;

    //next parabola event
    internal bool nextParbola = false;

    //animation time
    protected float animationTime = float.MaxValue;

    //gizmo
    protected ParabolaFly gizmo;

    //draw
    protected ParabolaFly parabolaFly;

    public void OnDrawGizmos()
    {
        if (gizmo == null)
        {
            if (ParabolaRoot == null)
                return;

            gizmo = new ParabolaFly(ParabolaRoot.transform);
        }

        gizmo.RefreshTransforms(1f);
        if ((gizmo.Points.Length - 1) % 2 != 0)
            return;

        int accur = 50;
        Vector3 prevPos = gizmo.Points[0].position;
        for (int c = 1; c <= accur; c++)
        {
            float currTime = c * gizmo.GetDuration() / accur;
            Vector3 currPos = gizmo.GetPositionAtTime(currTime);
            float mag = (currPos - prevPos).magnitude * 2;
            Gizmos.color = new Color(mag, 0, 0, 1);
            Gizmos.DrawLine(prevPos, currPos);
            Gizmos.DrawSphere(currPos, 0.01f);
            prevPos = currPos;
        }
    }

    private void Start()
    {
        parabolaFly = new ParabolaFly(ParabolaRoot.transform);

        if (Autostart)
        {
            RefreshTransforms(Speed);
            FollowParabola();
        }
    }

    private void Update()
    {
        nextParbola = false;

        if (Animation && parabolaFly != null && animationTime < parabolaFly.GetDuration())
        {
            int parabolaIndexBefore;
            int parabolaIndexAfter;
            parabolaFly.GetParabolaIndexAtTime(animationTime, out parabolaIndexBefore);
            animationTime += Time.deltaTime;
            parabolaFly.GetParabolaIndexAtTime(animationTime, out parabolaIndexAfter);

            transform.position = parabolaFly.GetPositionAtTime(animationTime);

            if (parabolaIndexBefore != parabolaIndexAfter)
                nextParbola = true;

            //if (transform.position.y > HighestPoint.y)
            //HighestPoint = transform.position;
        }
        else if (Animation && parabolaFly != null && animationTime > parabolaFly.GetDuration())
        {
            animationTime = float.MaxValue;
            Animation = false;
        }

    }

    public void FollowParabola()
    {
        RefreshTransforms(Speed);
        animationTime = 0f;
        transform.position = parabolaFly.Points[0].position;
        Animation = true;
        //HighestPoint = points[0].position;
    }

    public Vector3 GetHighestPoint(int parabolaIndex)
    {
        return parabolaFly.GetHighestPoint(parabolaIndex);
    }

    public Transform[] GetPoints()
    {
        return parabolaFly.Points;
    }

    public Vector3 GetPositionAtTime(float time)
    {
        return parabolaFly.GetPositionAtTime(time);
    }

    public float GetDuration()
    {
        return parabolaFly.GetDuration();
    }

    public void StopFollow()
    {
        animationTime = float.MaxValue;
    }
    /// <summary>
    /// Returns children transforms, sorted by name.
    /// </summary>
    public void RefreshTransforms(float speed)
    {
        parabolaFly.RefreshTransforms(speed);
    }


    public static float DistanceToLine(Ray ray, Vector3 point)
    {
        //see:http://answers.unity3d.com/questions/62644/distance-between-a-ray-and-a-point.html
        return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
    }

    public static Vector3 ClosestPointInLine(Ray ray, Vector3 point)
    {
        return ray.origin + ray.direction * Vector3.Dot(ray.direction, point - ray.origin);
    }

    public class ParabolaFly
    {
        public Transform[] Points;
        protected Parabola3D[] parabolas;
        protected float[] partDuration;
        protected float completeDuration;

        public ParabolaFly(Transform ParabolaRoot)
        {

            List<Component> components = new List<Component>(ParabolaRoot.GetComponentsInChildren(typeof(Transform)));
            List<Transform> transforms = components.ConvertAll(c => (Transform)c);

            transforms.Remove(ParabolaRoot.transform);
            transforms.Sort(delegate (Transform a, Transform b)
            {
                return a.name.CompareTo(b.name);
            });

            Points = transforms.ToArray();

            //check if odd
            if ((Points.Length - 1) % 2 != 0)
                throw new UnityException("ParabolaRoot needs odd number of points");

            //check if larger is needed
            if (parabolas == null || parabolas.Length < (Points.Length - 1) / 2)
            {
                parabolas = new Parabola3D[(Points.Length - 1) / 2];
                partDuration = new float[parabolas.Length];
            }

        }

        public Vector3 GetPositionAtTime(float time)
        {
            int parabolaIndex;
            float timeInParabola;
            GetParabolaIndexAtTime(time, out parabolaIndex, out timeInParabola);

            var percent = timeInParabola / partDuration[parabolaIndex];
            return parabolas[parabolaIndex].GetPositionAtLength(percent * parabolas[parabolaIndex].Length);
        }

        public void GetParabolaIndexAtTime(float time, out int parabolaIndex)
        {
            float timeInParabola;
            GetParabolaIndexAtTime(time, out parabolaIndex, out timeInParabola);
        }

        public void GetParabolaIndexAtTime(float time, out int parabolaIndex, out float timeInParabola)
        {
            //f(x) = axÂ² + bx + c
            timeInParabola = time;
            parabolaIndex = 0;

            //determine parabola
            while (parabolaIndex < parabolas.Length - 1 && partDuration[parabolaIndex] < timeInParabola)
            {
                timeInParabola -= partDuration[parabolaIndex];
                parabolaIndex++;
            }
        }

        public float GetDuration()
        {
            return completeDuration;
        }

        public Vector3 GetHighestPoint(int parabolaIndex)
        {
            return parabolas[parabolaIndex].GetHighestPoint();
        }

        /// <summary>
        /// Returns children transforms, sorted by name.
        /// </summary>
        public void RefreshTransforms(float speed)
        {
            if (speed <= 0f)
                speed = 1f;

            if (Points != null)
            {

                completeDuration = 0;

                //create parabolas
                for (int i = 0; i < parabolas.Length; i++)
                {
                    if (parabolas[i] == null)
                        parabolas[i] = new Parabola3D();

                    parabolas[i].Set(Points[i * 2].position, Points[i * 2 + 1].position, Points[i * 2 + 2].position);
                    partDuration[i] = parabolas[i].Length / speed;
                    completeDuration += partDuration[i];
                }


            }

        }
    }

    public class Parabola3D
    {
        public float Length { get; private set; }

        public Vector3 A;
        public Vector3 B;
        public Vector3 C;

        protected Parabola2D parabola2D;
        protected Vector3 h;
        protected bool tooClose;

        public Parabola3D()
        {
        }

        public Parabola3D(Vector3 A, Vector3 B, Vector3 C)
        {
            Set(A, B, C);
        }

        public void Set(Vector3 A, Vector3 B, Vector3 C)
        {
            this.A = A;
            this.B = B;
            this.C = C;
            RefreshCurve();
        }

        public Vector3 GetHighestPoint()
        {
            var d = (C.y - A.y) / parabola2D.Length;
            var e = A.y - C.y;

            var parabolaCompl = new Parabola2D(parabola2D.A, parabola2D.B + d, parabola2D.C + e, parabola2D.Length);

            Vector3 E = new Vector3();
            E.y = parabolaCompl.E.y;
            E.x = A.x + (C.x - A.x) * (parabolaCompl.E.x / parabolaCompl.Length);
            E.z = A.z + (C.z - A.z) * (parabolaCompl.E.x / parabolaCompl.Length);

            return E;
        }

        public Vector3 GetPositionAtLength(float length)
        {
            //f(x) = axÂ² + bx + c
            var percent = length / Length;

            var x = percent * (C - A).magnitude;
            if (tooClose)
                x = percent * 2f;

            Vector3 pos;

            pos = A * (1f - percent) + C * percent + h.normalized * parabola2D.F(x);
            if (tooClose)
                pos.Set(A.x, pos.y, A.z);

            return pos;
        }

        private void RefreshCurve()
        {

            if (Vector2.Distance(new Vector2(A.x, A.z), new Vector2(B.x, B.z)) < 0.1f &&
                Vector2.Distance(new Vector2(B.x, B.z), new Vector2(C.x, C.z)) < 0.1f)
                tooClose = true;
            else
                tooClose = false;

            Length = Vector3.Distance(A, B) + Vector3.Distance(B, C);

            if (!tooClose)
            {
                RefreshCurveNormal();
            }
            else
            {
                RefreshCurveClose();
            }
        }


        private void RefreshCurveNormal()
        {
            //                        .  E   .
            //                   .       |       point[1]
            //             .             |h         |       .
            //         .                 |       ___v1------point[2]
            //      .            ______--vl------    
            // point[0]---------
            //

            //lower v1
            Ray rl = new Ray(A, C - A);
            var v1 = ClosestPointInLine(rl, B);

            //get A=(x1,y1) B=(x2,y2) C=(x3,y3)
            Vector2 A2d, B2d, C2d;

            A2d.x = 0f;
            A2d.y = 0f;
            B2d.x = Vector3.Distance(A, v1);
            B2d.y = Vector3.Distance(B, v1);
            C2d.x = Vector3.Distance(A, C);
            C2d.y = 0f;

            parabola2D = new Parabola2D(A2d, B2d, C2d);

            //lower v
            //var p = parabola.E.x / parabola.Length;
            //Vector3 vl = points[0].position * (1f - p) + points[2].position * p;

            //h
            h = (B - v1) / Vector3.Distance(v1, B) * parabola2D.E.y;
        }

        private void RefreshCurveClose()
        {
            //distance to x0 - x2 line = |(x1-x0)x(x1-x2)|/|x2-x0|
            var fac01 = (A.y <= B.y) ? 1f : -1f;
            var fac02 = (A.y <= C.y) ? 1f : -1f;

            Vector2 A2d, B2d, C2d;

            //get A=(x1,y1) B=(x2,y2) C=(x3,y3)
            A2d.x = 0f;
            A2d.y = 0f;

            //b = sqrt(cÂ²-aÂ²)
            B2d.x = 1f;
            B2d.y = Vector3.Distance((A + C) / 2f, B) * fac01;

            C2d.x = 2f;
            C2d.y = Vector3.Distance(A, C) * fac02;

            parabola2D = new Parabola2D(A2d, B2d, C2d);
            h = Vector3.up;
        }
    }

    public class Parabola2D
    {
        public float A { get; private set; }
        public float B { get; private set; }
        public float C { get; private set; }

        public Vector2 E { get; private set; }
        public float Length { get; private set; }

        public Parabola2D(float a, float b, float c, float length)
        {
            this.A = a;
            this.B = b;
            this.C = c;

            SetMetadata();
            this.Length = length;
        }

        public Parabola2D(Vector2 A, Vector2 B, Vector2 C)
        {
            //f(x) = axÂ² + bx + c
            //a = (x1(y2 - y3) + x2(y3 - y1) + x3(y1 - y2)) / ((x1 - x2)(x1 - x3)(x3 - x2))
            //b = (x1Â²(y2 - y3) + x2Â²(y3 - y1) + x3Â²(y1 - y2))/ ((x1 - x2)(x1 - x3)(x2 - x3))
            //c = (x1Â²(x2y3 - x3y2) + x1(x3Â²y2 - x2Â²y3) + x2x3y1(x2 - x3))/ ((x1 - x2)(x1 - x3)(x2 - x3))
            var divisor = ((A.x - B.x) * (A.x - C.x) * (C.x - B.x));
            if (divisor == 0f)
            {
                A.x += 0.00001f;
                B.x += 0.00002f;
                C.x += 0.00003f;
                divisor = ((A.x - B.x) * (A.x - C.x) * (C.x - B.x));
            }
            this.A = (A.x * (B.y - C.y) + B.x * (C.y - A.y) + C.x * (A.y - B.y)) / divisor;
            this.B = (A.x * A.x * (B.y - C.y) + B.x * B.x * (C.y - A.y) + C.x * C.x * (A.y - B.y)) / divisor;
            this.C = (A.x * A.x * (B.x * C.y - C.x * B.y) + A.x * (C.x * C.x * B.y - B.x * B.x * C.y) + B.x * C.x * A.y * (B.x - C.x)) / divisor;

            this.B *= -1f;//hack

            SetMetadata();
            Length = Vector2.Distance(A, C);
        }

        public float F(float x)
        {
            return A * x * x + B * x + C;
        }

        private void SetMetadata()
        {
            //derive
            //a*xÂ²+b*x+c = 0
            //2ax+b=0
            //x = -b/2a
            var x = -B / (2 * A);
            E = new Vector2(x, F(x));
        }
    }
}

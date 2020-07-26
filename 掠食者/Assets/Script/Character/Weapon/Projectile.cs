using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float moveSpeed;
    private float duration;
    private float initialAngle;
    private float radShootAngleIncrement;
    private Transform target;
    private float freeFlyDuration;

    private float timer;
    private Vector2 finalDirection;

    public void ProjectileSetup(float moveSpeed, float duration,float initialAngle, float radShootAngleIncrement, Transform target, float freeFlyDuration)
    {
        this.moveSpeed = moveSpeed;
        this.duration = duration;
        this.initialAngle = initialAngle;  //Rad
        this.radShootAngleIncrement = radShootAngleIncrement;
        this.target = target;
        this.freeFlyDuration = freeFlyDuration;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        ProjectilePattern();
    }

    private void ProjectilePattern()
    {
        transform.eulerAngles = new Vector3(0 , 0, initialAngle);

        initialAngle += radShootAngleIncrement;  //Rad
        finalDirection = new Vector2(Mathf.Cos(initialAngle), Mathf.Sin(initialAngle));

        if (target != null)
        {
            if(timer < freeFlyDuration)
                transform.position += (Vector3)finalDirection * moveSpeed * Time.deltaTime;
            else if(timer < freeFlyDuration + 0.1f) { }
            else
                transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        else
            transform.position += (Vector3)finalDirection * moveSpeed * Time.deltaTime;

        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Character>() != null)
        {
            //return true to skill
            Destroy(gameObject);
        }
    }
}

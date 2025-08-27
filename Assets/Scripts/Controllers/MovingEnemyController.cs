using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingEnemyController : EnemyController
{
    /// <summary>
    /// How quickly this enemy moves, if at all
    /// </summary>
    [SerializeField]
    private float moveSpeed;

    /// <summary>
    /// How quickly the enemy turns, in radians per second
    /// </summary>
    [SerializeField]
    private float turnSpeed;

    /// <summary>
    /// World points this enemy paths between
    /// </summary>
    [SerializeField]
    private Vector3[] pathingPoints;

    /// <summary>
    /// Distance to target at which enemy is "close enough" to target
    /// </summary>
    [SerializeField]
    private float targetDistanceThreshold; // Could probably be a constant

    /// <summary>
    /// Degrees to target at which enemy is "close enough" to facing target
    /// </summary>
    [SerializeField]
    private float targetLookThreshold; // Could probably be a constant

    /// <summary>
    /// The most recent pathing point the enemy reached
    /// </summary>
    private int nextPathingPointIndex;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // TODO: Spawn at pathingPoints[0] (?)
        nextPathingPointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // If at the next point, update nextPathingPoint
        Vector3 target = pathingPoints[nextPathingPointIndex];
        if (isAtTargetPoint(target))
        {
            nextPathingPointIndex++;
            if (nextPathingPointIndex > pathingPoints.Length - 1)
            {
                nextPathingPointIndex = 0;
            }
            target = pathingPoints[nextPathingPointIndex];
        }

        if (isLookingAt(target))
        {
            // If facing the next point, move towards it
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        }
        else
        {
            // If not facing the next point, turn towards it
            float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    private bool isAtTargetPoint(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) <= targetDistanceThreshold;
    }

    private bool isLookingAt(Vector3 target)
    {
        return Vector3.Angle(getForward2D(), target - transform.position) <= targetLookThreshold;
    }

    private Vector3 getForward2D()
    {
        // Thank you Unity 2D
        return transform.right;
    }
}

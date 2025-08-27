using UnityEngine;

public class MovingEnemyController : MonoBehaviour
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
    private float targetDistanceThreshold;

    /// <summary>
    /// Degrees to target at which enemy is "close enough" to facing target
    /// </summary>
    [SerializeField]
    private float targetLookThreshold;

    /// <summary>
    /// How much suspicion being spotted by this enemy builds, per frame
    /// </summary>
    public int SuspicionRate;

    /// <summary>
    /// The most recent pathing point the enemy reached
    /// </summary>
    private int nextPathingPointIndex;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // TODO: Spawn at pathingPoints[0] (?)
        nextPathingPointIndex = pathingPoints.Length > 1 ? 1 : 0;
    }

    // Update is called once per frame
    void Update()
    {
        // If at the next point, update nextPathingPoint
        Vector3 target = pathingPoints[nextPathingPointIndex];
        if (isAtTargetPoint(target)) {
            nextPathingPointIndex++;
            if (nextPathingPointIndex > pathingPoints.Length)
            {
                nextPathingPointIndex = 0;
            }
            target = pathingPoints[nextPathingPointIndex];
        }

        if (isLookingAt(target))
        {
            // If facing the next point, move towards it
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed);
        }
        else
        {
            // If not facing the next point, turn towards it
            Vector3 targetDirection = target - transform.position;
            float rotateStep = turnSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotateStep, 0);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    private bool isAtTargetPoint(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) <= targetDistanceThreshold;
    }

    private bool isLookingAt(Vector3 target)
    {
        return Vector3.Angle(transform.forward, target - transform.position) <= targetLookThreshold;
    }
}

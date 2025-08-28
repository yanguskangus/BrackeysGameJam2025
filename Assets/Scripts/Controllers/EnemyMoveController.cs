using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

// TODO: Delete DogMoveController and HumanMoveController - really no reason to have these
// classes
public class EnemyMoveController : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;

    /// <summary>
    /// How quickly this enemy moves, if at all
    /// </summary>
    [SerializeField]
    protected float moveSpeed;

    /// <summary>
    /// How quickly the enemy turns, in radians per second
    /// </summary>
    [SerializeField]
    protected float turnSpeed;

    /// <summary>
    /// World points this enemy paths between
    /// </summary>
    [SerializeField]
    protected Transform[] pathingPoints;

    /// <summary>
    /// Distance to target at which enemy is "close enough" to target
    /// </summary>
    [SerializeField]
    protected float targetDistanceThreshold; // Could probably be a constant

    /// <summary>
    /// Degrees to target at which enemy is "close enough" to facing target
    /// </summary>
    [SerializeField]
    protected float targetAngleThreshold; // Could probably be a constant

    /// <summary>
    /// The most recent pathing point the enemy reached
    /// </summary>
    protected int nextPathingPointIndex;

    // States and type definitions
    [SerializeField] public MoveState MoveState;
    private bool _rotating;

    // Chase parameters
    [SerializeField] protected float chaseSpeed;
    public GameObject ChaseTarget;
    public string ChaseTargetTag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pathingPoints.Length > 0)
        {
            transform.position = pathingPoints[0].transform.position;
            nextPathingPointIndex = nextPathingPointIndex + 1 % pathingPoints.Length;
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var pathingPoint in pathingPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pathingPoint.position, 0.05f);
        }
    }

    public void StartChase(GameObject obj, string tag)
    {
        ChaseTarget = obj;
        ChaseTargetTag = tag;

        MoveState = MoveState.Chase;
    }

    public void ProcessMoveState()
    {
        if (_rotating)
        {
            return;
        }

        switch (MoveState)
        {
            case MoveState.Patrol:
                HandlePatrolMovement();
                break;
            case MoveState.Chase:
                HandleCatchMovement();
                break;
        }
    }

    protected void HandleCatchMovement()
    {
        Vector2 relativeDirection = (ChaseTarget.transform.position - transform.position).normalized;
        rb.linearVelocity = chaseSpeed * relativeDirection;

        var targetRotation = Quaternion.Euler(0, 180, 0) * transform.rotation;
        if (TargetIsOppositeDirection(ChaseTarget.transform.position))
        {
            transform.rotation = targetRotation;
        }
    }

    private void HandlePatrolMovement()
    {
        if (pathingPoints.Length == 0)
        {
            return;
        }

        // If at the next point, update nextPathingPoint
        var nextPathingPoint = pathingPoints[nextPathingPointIndex];
        Vector3 targetPosition = pathingPoints[nextPathingPointIndex].position;
        if (isAtTargetPoint(targetPosition))
        {
            nextPathingPointIndex++;
            if (nextPathingPointIndex > pathingPoints.Length - 1)
            {
                nextPathingPointIndex = 0;
            }
            targetPosition = pathingPoints[nextPathingPointIndex].position;

            // Decide if the dalmation needs to flip directio
            if (TargetIsOppositeDirection(targetPosition))
            {
                StartCoroutine(FlipAnimation());
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private IEnumerator FlipAnimation()
    {
        _rotating = true;

        var targetRotation = Quaternion.Euler(0, 180, 0) * transform.rotation;

        while (Quaternion.Angle(transform.rotation, targetRotation) > targetAngleThreshold)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation;

        _rotating = false;
    }

    private bool TargetIsOppositeDirection(Vector3 targetPosition)
    {
        var currentDirection = (transform.rotation * Vector2.left).normalized;
        var targetDirection = (targetPosition - transform.position).normalized;

        return Vector3.Dot(targetDirection, currentDirection) < 0;
    }


    private bool isAtTargetPoint(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) <= targetDistanceThreshold;
    }

    private bool isLookingAt(Vector3 target)
    {
        return Vector3.Angle(getForward2D(), target - transform.position) <= targetAngleThreshold;
    }

    private Vector3 getForward2D()
    {
        // Thank you Unity 2D
        return transform.right;
    }
}

public enum MoveState
{
    Stationary,
    Patrol,
    Chase
}

using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// How quickly this enemy moves, if at all
    /// </summary>
    public float moveSpeed;

    /// <summary>
    /// How quickly the enemy turns in (unit???)
    /// </summary>
    public float turnSpeed;

    /// <summary>
    /// World points this enemy paths between
    /// </summary>
    public Vector2[] pathingPoints;

    /// <summary>
    /// The most recent pathing point the enemy reached
    /// </summary>
    protected int nextPathingPointIndex;

    /// <summary>
    /// How far ahead the enemy can see
    /// </summary>
    public float sightlineLength;

    /// <summary>
    /// How wide the enemy's sightline is at the furthest point (straight line)
    /// </summary>
    public float sightlineWidth;


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
        Vector2 target2D = pathingPoints[nextPathingPointIndex];
        if (transform.position == new Vector3(target2D.x, target2D.y, transform.position.z)) { // Is this bad 2D to 3D conversion?
            nextPathingPointIndex++;
            if (nextPathingPointIndex > pathingPoints.Length)
            {
                nextPathingPointIndex = 0;
            }
            target2D = pathingPoints[nextPathingPointIndex];
        }

        // If not facing the next point, turn towards it

        // If facing the next point, move towards it
    }

    protected void playerEnteredSightCone()
    {
        // TODO: Notify suspicion meter that the player is building up suspicion points
    }

    protected void playerExitedSightCone()
    {
        // TODO: Notify suspicion meter than the player is no longer building up suspicion points
    }
}

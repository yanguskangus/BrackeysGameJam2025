using System.Collections.Generic;
using UnityEngine;

public class NoCameraOnPlayerCondition : MonoBehaviour, ITriggerCondition
{
    public StationaryDoggyCamController doggyCam;

    public bool Pass()
    {
        // This requires a condition frame because these type of events will constantly check
        // for whether the conditions are true (like camera opening / closing door depending)
        // on if it sees the player or not
        List<Collider2D> colliders = new List<Collider2D>();
        if (doggyCam.sightCollider.Overlap(colliders) > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.CompareTag(Tags.Dog))
                {
                    return false;
                }
            }
        }

        return true;
    }
}

public interface ITriggerCondition
{
    bool Pass();
}

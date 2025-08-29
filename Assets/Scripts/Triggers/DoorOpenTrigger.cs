using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] public ITriggerCondition[] conditions;
    [SerializeField] public GameObject door;
    public bool openIfPass;

    private void Awake()
    {
        conditions = GetComponents<ITriggerCondition>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Pass())
        {
            door.gameObject.SetActive(openIfPass ? false : true);
        }
        else
        {
            door.gameObject.SetActive(openIfPass ? true : false);
        }
    }

    private bool Pass()
    {
        foreach (var condition in conditions)
        {
            if (!condition.Pass())
            {
                return false;
            }
        }

        return true;
    }
}

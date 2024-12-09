using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusSetup : MonoBehaviour
{
    [SerializeField] private StatusButton statusPrefab;
    [SerializeField] private Transform spawnArea;

    [SerializeField] private GrabObject parentObject;

    void Start()
    {
        Setup(parentObject, true);
    }

    void Setup(GrabObject grab, bool isParent)
    {
        Instantiate(statusPrefab, spawnArea).Setup(grab, isParent);

        foreach (var item in grab.Detachables)
            Setup(item, false);
    }
}

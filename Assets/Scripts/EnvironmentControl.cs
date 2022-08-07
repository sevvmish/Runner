using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentControl : MonoBehaviour
{
    [SerializeField] private GeneralSettings settings;
    private Transform environment;
    private bool isRunning = true;

    private ObjectPooling road;    
    private Vector3 roadToCoverPosition;
    private Queue<GameObject> roadInAction = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        environment = GetComponent<Transform>();
        road = new ObjectPooling(20, Resources.Load<GameObject>("road"), environment);

        for (int i = 0; i < 8; i++)
        {
            AddRoad();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerControl.CurrentPlayerState != PlayerStates.isDead)
        {
            environment.position -= transform.forward * Time.deltaTime * settings.PlayerSpeed;


        }

        print(environment.position.z +" = " + roadToCoverPosition.z + ": " + (environment.position.z +roadToCoverPosition.z));

        if (( environment.position.z + roadToCoverPosition.z)<36)
        {
            RemoveRoad();
            AddRoad();
        }        
    }

    private void AddRoad()
    {
        GameObject r = road.GetObject();
        r.transform.localPosition = roadToCoverPosition;
        r.SetActive(true);
        roadToCoverPosition += (Vector3.forward * 6);
        roadInAction.Enqueue(r);
    }

    private void RemoveRoad()
    {        
        GameObject toDel = roadInAction.Dequeue();
        road.ReturnObject(toDel);        
    }


}

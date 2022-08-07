using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField] private GeneralSettings settings;
    [SerializeField] private Camera cameraMain;    
    [SerializeField] private Transform cameraBody;
    [SerializeField] private Transform player;

    private readonly Vector3 cameraShift = new Vector3(0, 5.5f, -4f);
    private readonly Vector3 cameraAngle = new Vector3(35, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        cameraBody.position = cameraShift;
        cameraBody.rotation = Quaternion.Euler(cameraAngle);


    }

    // Update is called once per frame
    void Update()
    {
        cameraBody.position = cameraShift + new Vector3(player.position.x, 0, 0);
    }
}

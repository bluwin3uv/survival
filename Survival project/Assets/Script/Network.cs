using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(FPS))]

public class Network : NetworkBehaviour
{
    public Camera cam;
    public AudioListener al;

    [SyncVar] Vector3 syncPos;
    [SyncVar] Quaternion syncRot;
    [Range(0, 1)]
    public float lerpRate = 1;

    private FPS player;

    private void Start()
    {
        player = GetComponent<FPS>();
        
        if(!isLocalPlayer)
        {
            al.enabled = false;
            cam.enabled = false;
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            player.MoveCam();
            CharacterController controller = player.controller;
            Cmd_SendPositionToServer(controller.transform.position);
            Cmd_SendRotationToServer(controller.transform.rotation);
        }
        else
        {
            LerpPos();
            LerpRot();
        }
	}

    [Command]
    void Cmd_SendPositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }

    [Command]
    void Cmd_SendRotationToServer(Quaternion rot)
    {
        syncRot = rot;
    }

    void LerpPos()
    {
        CharacterController controller = player.controller;
        controller.transform.position = Vector3.Lerp(controller.transform.position, syncPos, lerpRate);
    }

    void LerpRot()
    {
        CharacterController controller = player.controller;
        controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, syncRot, lerpRate);
    }
}

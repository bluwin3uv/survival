using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FPS))]
public class LocalUser : MonoBehaviour
{
    private FPS player;
	void Start ()
    {
        player = GetComponent<FPS>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        player.MoveCam();
	}
}

using UnityEngine;
using System.Collections;

public class KeyboardCameraMoving : MonoBehaviour {
    public float speed = 0.2f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.DownArrow))
            gameObject.transform.position += new Vector3(0.0f,-speed,0.0f);
        if (Input.GetKey(KeyCode.UpArrow))
            gameObject.transform.position += new Vector3(0.0f,speed,0.0f);
        if (Input.GetKey(KeyCode.RightArrow))
            gameObject.transform.position += new Vector3( speed,0.0f,0.0f);
        if (Input.GetKey(KeyCode.LeftArrow))
            gameObject.transform.position += new Vector3(-speed,0.0f,0.0f);
	}
}

using UnityEngine;
using System.Collections;

public class MouseCameraMove : MonoBehaviour {
    private Vector3 position;
    public float speed = 0.5f;
    public float bound_size = 5.0f;
	// Update is called once per frame
	void Update () {
        if (IsMouseOnScreenBound() && !IsCameraOnMapBound())
        {
            if (position.x > (Screen.width - bound_size))
                gameObject.transform.position += new Vector3(speed,0.0f,0.0f);
            else if (position.x < bound_size)
                gameObject.transform.position -= new Vector3(speed,0.0f,0.0f);
            if (position.y > (Screen.height - bound_size))
                gameObject.transform.position += new Vector3(0.0f,speed,0.0f);
            else if (position.y < 5)
                gameObject.transform.position -= new Vector3(0.0f, speed, 0.0f);
        }
	
	}
    bool IsMouseOnScreenBound()
    {
        position = Input.mousePosition;
        if ((position.x < bound_size | position.x > (Screen.width - bound_size)) | (position.y < bound_size | position.y > (Screen.height - bound_size)))
            return true;
        else
            return false;
    }
    //TODO
    bool IsCameraOnMapBound()
    {
        
        return false;
    }
}

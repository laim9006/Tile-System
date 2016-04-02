using UnityEngine;
using System.Collections;


public class DetectClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	
	}
    void OnMouseOver()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Vector4(0.5f,0.5f,0.5f,1.0f);
    }
    void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Vector4(1.0f, 1.0f, 1.0f, 1);
    }
}

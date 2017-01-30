using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	Rigidbody2D Blt;

	// Use this for initialization
	void Start () {
		Blt = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

	}
}

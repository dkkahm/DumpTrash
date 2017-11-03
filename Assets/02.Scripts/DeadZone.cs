using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour {
	public GameManager m_manager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider) {
		// Debug.Log("OnTriggerEnter");
		if(collider.gameObject.tag == "Trash") {
			m_manager.TrashDumpped(collider.gameObject);
		}
	}
}

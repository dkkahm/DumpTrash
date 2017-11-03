using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectThrow : MonoBehaviour {
	GameObject m_hooked_obj = null;
	Vector3 m_hooked_obj_pos;
	Vector3 m_hooked_obj_velocity = Vector3.zero;

	Vector3 m_n_of_plane;
	Vector3 m_p_of_plane;
	float m_d_of_plane;

	HashSet<string> m_target_tag_set = new HashSet<string>();

	public void AddTag(string tag) {
		m_target_tag_set.Add(tag);
	}

	public void RemoveTag(string tag) {
		m_target_tag_set.Remove(tag);
	}

	void Start() {
		
	}

	// Update is called once per frame
	void Update () {
		// Handle native touch events
		foreach (Touch touch in Input.touches) {
			HandleTouch(touch.fingerId, touch.position, touch.phase);
		}

		// Simulate touch events from mouse events
		if (Input.touchCount == 0) {
			if (Input.GetMouseButtonDown(0) ) {
				HandleTouch(10, Input.mousePosition, TouchPhase.Began);
			}
			if (Input.GetMouseButton(0) ) {
				HandleTouch(10, Input.mousePosition, TouchPhase.Moved);
			}
			if (Input.GetMouseButtonUp(0) ) {
				HandleTouch(10, Input.mousePosition, TouchPhase.Ended);
			}
		}	

		if(m_hooked_obj != null) {
			m_hooked_obj.transform.position = m_hooked_obj_pos;
		}
	}

	private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase) {
		switch (touchPhase) {
		case TouchPhase.Began:
			{
				Ray ray = Camera.main.ScreenPointToRay(touchPosition);
				RaycastHit hit;

				if(Physics.Raycast(ray, out hit)) {
					if(m_target_tag_set.Contains(hit.collider.gameObject.tag)) {
						//Debug.Log("Hit");
						m_hooked_obj = hit.collider.gameObject;
						m_p_of_plane = hit.collider.gameObject.transform.position;
						m_n_of_plane = Camera.main.transform.TransformDirection(Vector3.forward);

						Vector3 v = m_p_of_plane - Camera.main.gameObject.transform.position;
						m_d_of_plane = Mathf.Abs(Vector3.Dot(v, m_n_of_plane));
						//Debug.Log("p:" + m_p_of_plane + ",n:" + m_n_of_plane + ",d:" + m_d_of_plane);
					}
				}
			}
			break;
		case TouchPhase.Moved:
			if(m_hooked_obj != null) {
				UpdateHookObjectPosition(touchPosition);
			}
			break;
		case TouchPhase.Ended:
			if(m_hooked_obj != null) {
				UpdateHookObjectPosition(touchPosition);
				m_hooked_obj.GetComponent<Rigidbody>().velocity = m_hooked_obj_velocity;
				m_hooked_obj = null;
			}
			break;
		}
	}

	private void UpdateHookObjectPosition(Vector3 touchPosition) {
		Vector3 prev_obj_pos = m_hooked_obj_pos;
		Vector3 touch_pos = new Vector3(touchPosition.x, touchPosition.y, m_d_of_plane);
		m_hooked_obj_pos = Camera.main.ScreenToWorldPoint(touch_pos);
		m_hooked_obj_velocity = (m_hooked_obj_pos - prev_obj_pos) / Time.deltaTime;
	}
}

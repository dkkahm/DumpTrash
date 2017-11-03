using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public enum GameStatus
	{
		Waiting,
		Running,
		Done,
	}

	const int INITIAL_TRASH_COUNT = 100;

	const int INITIAL_MIN_TRASH_COUNT = 0;
	const int INITIAL_MAX_TRASH_COUNT = 10;

	const float INITIAL_TRASH_X_POS_MIN = -1f;
	const float INITIAL_TRASH_X_POS_MAX = 1f;
	const float INITIAL_TRASH_Y_POS_MIN = 5f;
	const float INITIAL_TRASH_Y_POS_MAX = 9f;
	const float INITIAL_TRASH_Z_POS_MIN = -1f;
	const float INITIAL_TRASH_Z_POS_MAX = 1f;

	const float HEAVY_TRASH_RATIO = 0.01f;

	GameStatus m_status = GameStatus.Waiting;
	int m_created_trash_count = 0;
	int m_dumped_trash_count = 0;
	
	public GameObject m_trash_prefab;
	public GameObject m_heavy_trash_prefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(m_status == GameStatus.Waiting) {
			int count = Random.Range(INITIAL_MIN_TRASH_COUNT, INITIAL_MAX_TRASH_COUNT);
			count = count >= INITIAL_MAX_TRASH_COUNT - 1 ? 1 : 0;
			if(m_created_trash_count + count > INITIAL_TRASH_COUNT) {
				count = INITIAL_TRASH_COUNT - m_created_trash_count;
			}

			for(int i = 0; i < count; ++ i) {
				Vector3 pos = new Vector3(
					Random.Range(INITIAL_TRASH_X_POS_MIN, INITIAL_TRASH_X_POS_MAX),
					Random.Range(INITIAL_TRASH_Y_POS_MIN, INITIAL_TRASH_Y_POS_MAX),
					Random.Range(INITIAL_TRASH_Z_POS_MIN, INITIAL_TRASH_Z_POS_MAX));

				if(Random.value < HEAVY_TRASH_RATIO)
					Instantiate(m_heavy_trash_prefab, pos, Random.rotation);
				else
					Instantiate(m_trash_prefab, pos, Random.rotation);
			}

			m_created_trash_count += count;
			if(m_created_trash_count >= INITIAL_TRASH_COUNT) {
				m_status = GameStatus.Running;
			}
		}
	}

	public GameStatus GetStatus() {
		return m_status;
	}

	public void TrashDumpped(GameObject trash) {
		++ m_dumped_trash_count;
		Destroy(trash, 1f);

		if(m_dumped_trash_count == m_created_trash_count) {
			Debug.Log("Done");
			m_status = GameStatus.Done;
		}
	}
}

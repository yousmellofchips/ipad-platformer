using UnityEngine;
using System.Collections;

public class GameManagerInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameManager thrownAwayRef = GameManager.Instance;
		thrownAwayRef = null;
	}
}

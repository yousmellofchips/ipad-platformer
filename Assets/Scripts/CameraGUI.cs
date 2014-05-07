using UnityEngine;
using System.Collections;

// Draw an always-visible outline for the usable game area.
public class CameraGUI : MonoBehaviour {
	void OnDrawGizmos() {
		// Area excludes top and bottom areas for controls, adverts, lives etc.
		// Just includes the actual play area for level designing.
		Gizmos.DrawWireCube(new Vector3(0,0,0), new Vector3(16f, 10f));

		// Also draw cube to highlight top and bottom controls / scores area.
		Gizmos.DrawWireCube(new Vector3(0,0,0), new Vector3(16f, 12f));
	}
}

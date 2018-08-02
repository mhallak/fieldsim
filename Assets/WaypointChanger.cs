using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path{
	public GameObject PathParent;
	public bool activate = false;
}

public class WaypointChanger : MonoBehaviour {

	public bool runScript = false;
	public Path[] paths;
	public WaypointDriver driver;

	Path activePath;

	void DisableAllPaths(){
		
		foreach (Path path in paths)
		{
			path.activate = false;
		}
	}

	void GetActivePath(){
		foreach(Path path in paths){

			if(path.activate) {
				activePath = path;

			}

		}
	}

	void InjectActivePath(){
		activePath.activate = true;

		driver.waypointsParent = activePath.PathParent;
		driver.Refresh();
	} 

	private void OnValidate() {
		if(!runScript) return;
		runScript = false;

		GetActivePath();
		DisableAllPaths();
		InjectActivePath();
		

	}

}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public Text scoreText;
	public Text coinsText;
	public Text keysText;
	public UIInventory uiInventory;
	public WeaponInventory weaponInventory;
	public Text dungeonLevelText;
	public Text currentHealthText;
	public Text maxHealthText;
	public Image healthImage;
	public Timer timer;
	public GameObject Minimap;
	public Camera minimapCam;
	private int minimapMode = 0;
	private Vector3 smoothDampVar1;
	private float smoothDampVar2;
	public float minimapSmoothTime = .5f;

	void Start(){
		minimapCam = GameManager.Instance.minimap;
	}

	public void toggleMinimap(){
		if (minimapMode == 0)
			minimapMode = 1;
		else
			minimapMode = 0;
	}

	void Update(){
		if (minimapCam == null) {
			minimapCam = GameManager.Instance.minimap;
		} else {

			if (minimapMode == 0) {
				Minimap.transform.localScale = Vector3.SmoothDamp (Minimap.transform.localScale, new Vector3 (4f, 4f, 4f), ref smoothDampVar1, minimapSmoothTime);
				minimapCam.orthographicSize = Mathf.SmoothDamp (minimapCam.orthographicSize, 100f, ref smoothDampVar2, minimapSmoothTime);
			} else if (minimapMode == 1) {
				Minimap.transform.localScale = Vector3.SmoothDamp (Minimap.transform.localScale, new Vector3 (1.5f, 1.5f, 1.5f), ref smoothDampVar1, minimapSmoothTime);
				minimapCam.orthographicSize = Mathf.SmoothDamp (minimapCam.orthographicSize, 40f, ref smoothDampVar2, minimapSmoothTime);
			}
		}
	}
}

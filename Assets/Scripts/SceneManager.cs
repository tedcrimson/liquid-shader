using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using us = UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

	public Button nextButton;
	public static SceneManager instance;
	private int currentScene;

	
	public string formatedString = "{value} FPS";
	public Text txtFps;

	public float updateRateSeconds = 4.0F;

	int frameCount = 0;
	float dt = 0.0F;
	float fps = 0.0F;
	// Use this for initialization
	void Awake () {

		if(instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}else
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		StartCoroutine(NextScene());
		nextButton.onClick.AddListener(()=>StartCoroutine(NextScene()));
	}
	
	IEnumerator NextScene()
	{
		currentScene = (currentScene + 1) % us.SceneManager.sceneCountInBuildSettings;
		if(currentScene == 0) currentScene++;
		Debug.Log(currentScene);
		yield return us.SceneManager.LoadSceneAsync(currentScene);
	}


 
     void Update()
     {
         frameCount++;
         dt += Time.unscaledDeltaTime;
         if (dt > 1.0 / updateRateSeconds)
         {
             fps = frameCount / dt;
             frameCount = 0;
             dt -= 1.0F / updateRateSeconds;
         }
         txtFps.text = formatedString.Replace("{value}", System.Math.Round(fps, 1).ToString("0.0"));
     }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {
    private Slider slider;
    private AsyncOperation loading;

    private bool shouldContinue;
    private bool shouldTryContinue;

    public Text Continue;
    public Text Info;
    public Image Slider;

    private void Awake() {
        this.slider = this.GetComponent<Slider>();
    }
    private void Start() {
        this.slider.value = 0;
        this.slider.maxValue = 0.9f;

        StartCoroutine(LoadScene());
    }

    private void Update() {
        if(this.shouldTryContinue)
            if(Input.GetKeyDown(KeyCode.Space)) this.shouldContinue = true;
    }

    IEnumerator LoadScene() {
        yield return null;

        this.loading = SceneManager.LoadSceneAsync("Main");
        this.loading.allowSceneActivation = false;
        this.Info.text = "Loading Scene...";

        while (!this.loading.isDone) {
            this.slider.value = this.loading.progress;
            if(this.loading.progress >= 0.9f){
                this.shouldTryContinue = true;
                this.Continue.enabled = true;
                this.Slider.color = Color.green;

                this.Info.text = "Scene loaded";

                if(this.shouldContinue) this.loading.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public void PlayGame() {
      SceneManager.LoadSceneAsync(1);
    }

    public void ReadInstructions() {
        SceneManager.LoadSceneAsync(2);
    }

    public void QuitGame() {
        Application.Quit();
    }
}

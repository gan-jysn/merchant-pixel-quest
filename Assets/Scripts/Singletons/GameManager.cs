using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS0414
using Sirenix.OdinInspector;

public class GameManager : Singleton<GameManager> {
    [SerializeField] bool isGamePaused = true;
    [SerializeField] bool hasGameStarted = false;

    public bool IsGamePaused { get { return isGamePaused; } }

    #region Events
    public event Action OnGamePaused;
    public event Action OnGameResumed;
    public event Action OnGameStarted;
    #endregion

    private void Start() {
        ResetParameters();
        SoundManager.Instance.PlayTitleBGM();

        OnGameStarted += OnGameStart;
    }

    private void OnDestroy() {
        OnGameStarted -= OnGameStart;
    }

    private void ResetParameters() {
        isGamePaused = false;
        hasGameStarted = false;
    }

    public void StartGame() {
        OnGameStarted?.Invoke();
        SoundManager.Instance.PlayBtnSFX();
    }

    private void OnGameStart() {
        hasGameStarted = true;
        SoundManager.Instance.PlayGameBGM();
    }

    [Button]
    public void PauseGame() {
        isGamePaused = true;
        OnGamePaused?.Invoke();
    }

    [Button]
    public void ResumeGame() {
        isGamePaused = false;
        OnGameResumed?.Invoke();
    }
}
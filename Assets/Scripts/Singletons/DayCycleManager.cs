using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DayCycleManager : Singleton<DayCycleManager> {
    [SerializeField] Day currentDay;
    [SerializeField] bool isTimerActive = false;
    [SerializeField] float dayCycleTime = 300; //in seconds

    public Day CurrentDay { get { return currentDay; } }

    private float internalTimer;
    private Coroutine dayCycleTimer;

    #region Events
    public event Action OnDayChanged;
    #endregion

    private void Start() {
        internalTimer = dayCycleTime;

        GameManager.Instance.OnGameStarted += StartNewCycle;
        GameManager.Instance.OnGameResumed += ResumeCountdown;
        GameManager.Instance.OnGamePaused += PauseCountdown;
        OnDayChanged += StartNewCycle;
    }

    private void OnDestroy() {
        GameManager.Instance.OnGameStarted -= StartNewCycle;
        GameManager.Instance.OnGameResumed -= ResumeCountdown;
        GameManager.Instance.OnGamePaused -= PauseCountdown;
        OnDayChanged -= StartNewCycle;
    }

    private void Update() {
        Countdown();
    }

    private void Countdown() {
        if (!isTimerActive)
            return;

        internalTimer -= Time.deltaTime;

        if (internalTimer <= 0) {
            isTimerActive = false;
            CycleDay();
        }
    }

    [Button]
    public void CycleDay() {
        int nextDay = ((int) currentDay) + 1;
        if (nextDay > 6) {
            nextDay = 0;
        }
        currentDay = (Day) nextDay;
        OnDayChanged?.Invoke();
    }

    private void StartNewCycle() {
        internalTimer = dayCycleTime;
        isTimerActive = true;
    }

    private void PauseCountdown() {
        isTimerActive = false;
    }

    private void ResumeCountdown() {
        isTimerActive = true;
    }
}

public enum Day {
    Mon,
    Tues,
    Wed,
    Thurs,
    Fri,
    Sat,
    Sun
}
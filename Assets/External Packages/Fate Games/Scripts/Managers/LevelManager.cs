using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FateGames
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        public UnityEvent OnStart { get; private set; } = new();
        public UnityEvent OnSuccess { get; private set; } = new();
        public UnityEvent OnFail { get; private set; } = new();

        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            Instance = this;

        }
        public void StartLevel()
        {
            OnStart.Invoke();
        }
        public void FinishLevel(bool success)
        {
            GameManager.Instance.UpdateGameState(GameState.FINISHED);
            if (success) OnSuccess.Invoke();
            else OnFail.Invoke();
            SceneManager.FinishLevel(success);
        }
    }
}
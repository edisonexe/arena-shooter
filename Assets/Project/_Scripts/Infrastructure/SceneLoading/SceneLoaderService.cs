using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArenaShooter.Infrastructure.SceneLoading
{
    public class SceneLoaderService
    {
        public void LoadScene(string sceneName, Action onSceneLoaded = null)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            
            if (operation == null)
            {
                Debug.LogError($"[SceneLoaderService] Error load {sceneName}");
                return;
            }

            operation.completed += _ => onSceneLoaded?.Invoke();
        }
    }
}
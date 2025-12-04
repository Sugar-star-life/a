using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    [Header("可供随机选择的场景 Index 列表")]
    public int[] randomSceneIndexes;   // 你可以在 Inspector 里设置，比如：0,1,3,5

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // -----------------------------
    // 进入下一个场景（Index + 1）
    // -----------------------------
    public void NextSceneLoader()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogWarning("已经是最后一个场景！");
        }
    }

    // -----------------------------
    // 进入上一个场景（Index - 1）
    // -----------------------------
    public void PreviousSceneLoader()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int previousIndex = currentIndex - 1;

        if (previousIndex >= 0)
        {
            SceneManager.LoadScene(previousIndex);
        }
        else
        {
            Debug.LogWarning("已经是第一个场景！");
        }
    }

    // -----------------------------
    // 进入第一个场景
    // -----------------------------
    public void FirstSceneLoader()
    {
        SceneManager.LoadScene(0);
    }

    // -----------------------------
    // 进入随机场景（从 randomSceneIndexes 中随机）
    // -----------------------------
    public void ramdomSceneLoader()
    {
        if (randomSceneIndexes == null || randomSceneIndexes.Length == 0)
        {
            Debug.LogWarning("随机场景列表为空，请在 Inspector 中指定 randomSceneIndexes！");
            return;
        }

        int randomIndex = randomSceneIndexes[Random.Range(0, randomSceneIndexes.Length)];

        if (randomIndex >= 0 && randomIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(randomIndex);
        }
        else
        {
            Debug.LogError("randomSceneIndexes 中包含无效的场景 Index！");
        }
    }
}

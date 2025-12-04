using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class RankingUI : MonoBehaviour
{
    [Header("用于显示玩家排名的父物体（Vertical Layout Group）")]
      public Transform rankingContainer;
    [Header("玩家单行显示的 UI Prefab")]
      public GameObject rankingItemPrefab;
    IEnumerator Start()
    {
        yield return null;
        RefreshRankingUI();
    }
    private void Update()
    {
        RefreshRankingUI();
    }
    public void RefreshRankingUI()
    {
        var pm = PlayerManager2.instance;
        if (pm == null) return;

        // 清空旧内容
        foreach (Transform child in rankingContainer)
            Destroy(child.gameObject);

        // 为每个玩家创建一行 UI
        for (int rank = 0; rank < pm.Players.Count; rank++)
        {
            int playerIndex = pm.PlayerRankIndex[rank];

            GameObject item = Instantiate(rankingItemPrefab, rankingContainer);

            item.transform.Find("RankText").GetComponent<Text>().text = (rank + 1).ToString();
            item.transform.Find("NameText").GetComponent<Text>().text = pm.PlayerNames[playerIndex];
            item.transform.Find("PointText").GetComponent<Text>().text = pm.PlayerPoints[playerIndex].ToString();
        }
    }

}

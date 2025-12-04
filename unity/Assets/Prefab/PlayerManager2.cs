using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager2 : MonoBehaviour
{
    public static PlayerManager2 instance;

    [Header("玩家列表 (GameObject 对应每个玩家)")]
    public List<GameObject> Players = new List<GameObject>();

    [Header("玩家名称")]
    public List<string> PlayerNames = new List<string>();

    [Header("玩家分数")]
    public List<int> PlayerPoints = new List<int>();

    [Header("玩家排名 (0 = 第一名, 1 = 第二名...)")]
    public List<int> PlayerRankIndex = new List<int>(); // 排名 → 玩家索引
    private int PlayersAmount;
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

        InitializeLists();
        PlayersAmount = Players.Count;
    }

    private void Start()
    {
        InitializePointAndRanking();
    }

    // 初始化列表长度
    private void InitializeLists()
    {
        int count = Players.Count;

        ResizeList(PlayerPoints, count);
        ResizeList(PlayerRankIndex, count);
    }

    // 自动扩展 / 修复长度
    private void ResizeList(List<int> list, int count)
    {
        list.Clear();
        for (int i = 0; i < count; i++)
            list.Add(0);
    }

    // 玩家加分
    public void AddPlayerPoint(int playerIndex, int points)
    {
        PlayerPoints[playerIndex] += points;
        UpdateRanking();
    }

    // 更新排名
    private void UpdateRanking()
    {
        // 排名按分数排序
        List<int> indexList = new List<int>();
        for (int i = 0; i < Players.Count; i++)
            indexList.Add(i);

        indexList.Sort((a, b) => PlayerPoints[b].CompareTo(PlayerPoints[a]));

        PlayerRankIndex = indexList;
    }

    // 初始化分数 & 排名
    public void InitializePointAndRanking()
    {
        for (int i = 0; i < Players.Count; i++)
            PlayerPoints[i] = 0;

        UpdateRanking();
    }

    // 获取指定玩家当前排名（0 = 第一名）
    public int GetPlayerRank(int playerIndex)
    {
        return PlayerRankIndex.IndexOf(playerIndex);
    }

    public void EndJudge()//终点判定，当所有玩家均到达终点以后，进入下一个场景
    {
        PlayersAmount -= 1;
        if (PlayersAmount == 0)
        {
            SceneLoader.instance.NextSceneLoader();
            PlayersAmount = Players.Count;
        }
    }
    
}

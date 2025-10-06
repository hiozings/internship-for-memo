using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    [System.Serializable]
    public class BuffConfig
    {
        public GameObject buffPrefab;
        public string buffName;
        
    }

    [Header("Buff配置")]
    public BuffConfig[] buffConfigs;

    [Header("生成设置")]
    public float spawnInterval;           // 生成间隔（秒）
    public float initialDelay;             // 初始延迟
    public int maxBuffsInScene;             // 场景中最大Buff数量
    public bool spawnEnabled = true;            // 是否启用生成

    [Header("生成区域")]
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    public float safeDistanceFromPlayer;   // 离玩家的安全距离

    private List<GameObject> activeBuffs = new List<GameObject>();
    private Transform player;
    private Coroutine spawnCoroutine;

    private void Start()
    {
        // 查找玩家
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // 验证配置
        if (buffConfigs == null || buffConfigs.Length == 0)
        {
            Debug.LogError("未配置Buff预制体！");
            return;
        }

        // 开始生成协程
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void OnDestroy()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }

    private IEnumerator SpawnRoutine()
    {
        // 初始延迟
        yield return new WaitForSeconds(initialDelay);

        while (spawnEnabled)
        {
            // 检查是否达到最大数量限制
            if (activeBuffs.Count < maxBuffsInScene)
            {
                TrySpawnRandomBuff();
            }

            // 等待下一个生成周期
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void TrySpawnRandomBuff()
    {
        
        BuffConfig selectedBuff = GetRandomBuff();
       
        // 获取生成位置
        Vector2 spawnPosition = GetSpawnPosition();
        if (spawnPosition == Vector2.zero) // 使用zero表示未找到有效位置
        {
            Debug.Log("未找到有效的生成位置");
            return;
        }

        // 生成Buff
        SpawnBuff(selectedBuff, spawnPosition);
    }

    private BuffConfig GetRandomBuff()
    {
        int index = Random.Range(0, buffConfigs.Length);
        return buffConfigs[index];
    }

    private Vector2 GetSpawnPosition()
    {
        const int maxAttempts = 10; // 最大尝试次数
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 potentialPosition = new Vector2(x, y);

            // 检查与玩家的距离
            if (player != null && Vector2.Distance(potentialPosition, player.position) < safeDistanceFromPlayer)
            {
                continue; // 太近，重新尝试
            }

            return potentialPosition; // 找到有效位置
        }

        return Vector2.zero; // 未找到有效位置
    }

    private void SpawnBuff(BuffConfig buffConfig, Vector2 position)
    {
        GameObject buffInstance = Instantiate(buffConfig.buffPrefab, position, Quaternion.identity);
        activeBuffs.Add(buffInstance);
        Loot loot = buffInstance.GetComponent<Loot>();
        if (loot != null){
            loot.OnRemoveBuff += RemoveBuff;
            
        }
    }

    public void RemoveBuff(GameObject buff)
    {
        //Debug.Log("Remove");
        activeBuffs.Remove(buff);
        Loot loot = buff?.GetComponent<Loot>();
        if (loot != null)
        {
            loot.OnRemoveBuff -= RemoveBuff;
        }
    }

    public void ClearBuffs()
    {
        foreach (GameObject buff in activeBuffs)
        {
            if (buff != null)
                Destroy(buff);
        }
        activeBuffs.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        
        Vector3 center = (spawnAreaMin + spawnAreaMax) / 2;
        Vector3 size = (Vector3)(spawnAreaMax - spawnAreaMin);
        Gizmos.DrawWireCube(center, size);

        // 绘制安全距离
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, safeDistanceFromPlayer);
        }
    }
}

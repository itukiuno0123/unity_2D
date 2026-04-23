using System.Collections;
using UnityEngine;

/// <summary>
/// 敵を一定間隔でスポーンするスクリプト
/// - 画面上端の横ランダム位置に出現させる
/// - SpawnInterval で間隔を調整
/// - spawnParent を指定すると生成オブジェクトをその親にする（階層整理用）
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("スポーン設定")]
    [Tooltip("スポーンする敵のプレハブ（EnemyController が付いたプレハブ）")]
    [SerializeField] private GameObject enemyPrefab;

    [Tooltip("敵を出現させる間隔（秒）")]
    [SerializeField] private float spawnInterval = 1.0f;

    [Tooltip("画面端からどれだけ内側に出現させるか（ワールド単位）。小さくすると端ギリギリに出る")]
    [SerializeField] private float horizontalPadding = 0.2f;

    [Tooltip("画面上端からどれだけ上にスポーンさせるか（ワールド単位）。通常は少し上に出す")]
    [SerializeField] private float spawnYOffset = 0.5f;

    [Tooltip("生成した敵を入れる親（Hierarchy の整理用。未設定でも OK）")]
    [SerializeField] private Transform spawnParent;

    private Camera mainCam;
    private Coroutine spawnCoroutine;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void OnEnable()
    {
        // スポーンループ開始
        spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    void OnDisable()
    {
        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
    }

    // スポーンを繰り返すコルーチン
    private IEnumerator SpawnLoop()
    {
        // 初回は待たず spawnInterval 後に連続で発生させたい場合は WaitForSeconds を後ろに移動してください
        while (true)
        {
            SpawnOne();
            yield return new WaitForSeconds(Mathf.Max(0.01f, spawnInterval));
        }
    }

    // 1体スポーンする処理
    private void SpawnOne()
    {
        if (enemyPrefab == null) return;
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        // カメラの top-left / top-right をワールドに変換
        float zDistance = Mathf.Abs(mainCam.transform.position.z - transform.position.z);
        Vector3 leftWorld = mainCam.ViewportToWorldPoint(new Vector3(0f, 1f, zDistance));
        Vector3 rightWorld = mainCam.ViewportToWorldPoint(new Vector3(1f, 1f, zDistance));

        float minX = leftWorld.x + horizontalPadding;
        float maxX = rightWorld.x - horizontalPadding;

        float x = Random.Range(minX, maxX);
        float y = leftWorld.y + spawnYOffset;

        Vector3 spawnPos = new Vector3(x, y, 0f);

        // Instantiate（親を指定して階層整理）
        GameObject go = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, spawnParent);
        // 生成後の追加初期化がある場合はここに記述（例：速度変更など）
    }
}

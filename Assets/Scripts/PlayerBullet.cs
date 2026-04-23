using UnityEngine;

/// <summary>
/// プレイヤーの弾（プレハブ用）
/// </summary>
public class PlayerBullet : MonoBehaviour
{
    [Tooltip("弾速（ワールド単位 / 秒）")]
    [SerializeField] private float speed = 12f;

    [Tooltip("画面外判定の余白（Viewport 単位）。")]
    [SerializeField] private float viewportMargin = 0.1f;

    private Rigidbody2D rb;
    private Camera mainCam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        mainCam = Camera.main;
    }

    void Start()
    {
        // 修正点: 正しいプロパティは `velocity`
        rb.linearVelocity = transform.up * speed;
    }

    void Update()
    {
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        // カメラのビューポート座標に変換して画面外判定
        Vector3 vp = mainCam.WorldToViewportPoint(transform.position);

        // ビューポートが余白を超えたら破棄
        if (vp.y > 1f + viewportMargin || vp.y < -viewportMargin || vp.x < -viewportMargin || vp.x > 1f + viewportMargin)
        {
            Destroy(gameObject);
        }
    }
}

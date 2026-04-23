using UnityEngine;

/// <summary>
/// 敵の挙動：
// - 下方向へまっすぐ移動する（Rigidbody2D を使う）
// - 画面外に出たら自動で削除する
// - プレイヤーの弾（タグ "PlayerBullet"）と当たったら自分と弾を Destroy する
/// </summary>
public class EnemyController : MonoBehaviour
{
    [Header("移動設定")]
    [Tooltip("敵の速度（ワールド単位 / 秒）。下方向に移動します。")]
    [SerializeField] private float speed = 2.5f;

    [Tooltip("画面外判定の余白（Viewport 単位）。0.1なら画面外に少し出たら削除します。")]
    [SerializeField] private float viewportMargin = 0.1f;

    // 内部参照
    private Rigidbody2D rb;
    private Camera mainCam;

    void Awake()
    {
        // Rigidbody2D を取得（なければ追加）
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();

        // 推奨設定（プレハブでも Inspector で設定してください）
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        mainCam = Camera.main;
    }

    void Start()
    {
        // 発生直後に下方向へ移動させる（Rigidbody2D の速度を直接設定）
        rb.linearVelocity = Vector2.down * speed;
    }

    void Update()
    {
        // カメラがない場合は処理しない
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        // 画面外判定：ワールド座標をビューポートに変換して範囲外か確認
        Vector3 vp = mainCam.WorldToViewportPoint(transform.position);

        if (vp.y < 0f - viewportMargin || vp.x < 0f - viewportMargin || vp.x > 1f + viewportMargin)
        {
            // 画面下または左右にはみ出したら削除
            Destroy(gameObject);
        }
    }

    // トリガーで弾と当たったら両方消す（弾に "PlayerBullet" タグを指定する想定）
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        // 弾の判定はタグ比較が簡単で高速
        if (other.CompareTag("PlayerBullet"))
        {
            // 弾と敵の両方を削除
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}

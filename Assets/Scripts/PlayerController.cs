using UnityEngine;

/// <summary>
/// プレイヤーの移動（上下左右）と連射（Space長押し）を扱うコントローラ
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    [Tooltip("移動速度（ワールド単位 / 秒）")]
    [SerializeField] private float moveSpeed = 6f;

    [Tooltip("画面端からどれだけ内側に収めるか（ワールド単位）")]
    [SerializeField] private float screenPadding = 0.2f;

    [Header("射撃設定")]
    [Tooltip("弾のプレハブ（PlayerBullet スクリプト付き）")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("弾を生成する位置（Player の子オブジェクト）")]
    [SerializeField] private Transform firePoint;

    [Tooltip("連射間隔（秒）。小さいほど速い連射。例: 0.12")]
    [SerializeField] private float fireInterval = 0.12f;

    private Rigidbody2D rb;
    private Camera mainCam;
    private Vector2 targetVelocity = Vector2.zero;

    private float minX, maxX, minY, maxY;
    private float halfWidth = 0f;
    private float halfHeight = 0f;

    private float nextFireTime = 0f;

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
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            halfWidth = sr.bounds.extents.x;
            halfHeight = sr.bounds.extents.y;
        }
        else
        {
            var col = GetComponent<Collider2D>();
            if (col != null)
            {
                halfWidth = col.bounds.extents.x;
                halfHeight = col.bounds.extents.y;
            }
        }

        UpdateBounds();
    }

    void Update()
    {
        float hx = Input.GetAxisRaw("Horizontal");
        float hy = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(hx, hy);

        if (input.sqrMagnitude > 1f) input = input.normalized;

        targetVelocity = input * moveSpeed;

        if (bulletPrefab != null && firePoint != null && Input.GetKey(KeyCode.Space))
        {
            if (Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + Mathf.Max(0.0001f, fireInterval);
            }
        }
    }

    void FixedUpdate()
    {
        // 修正点: 正しいプロパティは `velocity`
        rb.linearVelocity = targetVelocity;

        Vector2 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        rb.position = pos;
    }

    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            mainCam = Camera.main;
            UpdateBounds();
        }
    }

    private void UpdateBounds()
    {
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        float zDistance = Mathf.Abs(mainCam.transform.position.z - transform.position.z);
        Vector3 bl = mainCam.ViewportToWorldPoint(new Vector3(0f, 0f, zDistance));
        Vector3 tr = mainCam.ViewportToWorldPoint(new Vector3(1f, 1f, zDistance));

        minX = bl.x + screenPadding + halfWidth;
        maxX = tr.x - screenPadding - halfWidth;

        minY = bl.y + screenPadding + halfHeight;
        maxY = tr.y - screenPadding - halfHeight;
    }

    private void Fire()
    {
        Debug.Log("Fire!");
        if (bulletPrefab == null || firePoint == null) return;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
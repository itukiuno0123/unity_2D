using UnityEngine; //using C++の#includeと同じ
                   //予測変換などで関数を呼び出した際にusingが増えることもある

//・classの行に関して
//編集しない。親クラスやpublicを変更しないと実装できない内容はないため

public class Player : MonoBehaviour
{
    //変数宣言
    //private 型名　変数名（＝値）；
    //使用されてない変数は文字の色が灰色
    private int num = 0;
    //変数の方はC++と一緒
    //変数などの入力際に、灰色で候補が出る
    //TABキーで入力。
    private float floatValue = 0;
    private bool bpplean = false;
    private string word = string.Empty;

    //クラスの変数名は、方の入力後の候補の中から選択がおすすめ。クラスの変数は、クラス名と同じ名前をつけることが多い。
    private Rigidbody2D rigidbody2D = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        num = num * 3;

        //変数の初期化
        //（座標などのゲームが始まらないと設定できない値など)
        transform.position = Vector3.zero;//transformは、MonoBehaviourクラスの変数で、ゲームオブジェクトの位置や回転などを管理するクラス

        //Vector3は、3D空間の座標を表すクラス。x、y、zの3つの値を持つ。
        Vector3 vector = transform.position;
        //定数を使用する場合は、クラス名.定数名で呼び出す。Vector3.zeroは、(0,0,0)の座標を表す定数。
        transform.position = new Vector3(5.0f, 3.0f, 3);//newは、クラスのインスタンスを作成するためのキーワード。Vector3(1, 2, 3)は、(1,2,3)の座標を表すVector3クラスのインスタンスを作成する。
        //X,Y,Zの一一部要素だけ代入は不可能
        //transform.position.x = 5.0f;は、エラーになる。transform.positionは、Vector3クラスのインスタンスであり、x、y、zの値を持つため、xだけを代入することはできない。

        //コンポーネント変数の初期化

        //Rigidbody2Dは、物理演算を管理するクラス。2Dゲームで使用される。
        rigidbody2D = GetComponent<Rigidbody2D>();//GetComponentは、ゲームオブジェクトにアタッチされているコンポーネントを取得する関数

        if (rigidbody2D = GetComponent<Rigidbody2D>())//if文で、GetComponentの戻り値がnullでない場合に処理を行う。GetComponentは、ゲームオブジェクトにアタッチされているコンポーネントを取得する関数。nullの場合は、コンポーネントが存在しないことを意味する。
        {
            Debug.Log("Rigidbody2Dコンポーネントが存在します。");//Debug.Logは、コンソールにメッセージを表示する関数。
            Debug.LogError("Rigidbody2Dコンポーネントが存在しません。");//Debug.LogErrorは、コンソールにエラーメッセージを表示する関数。
            Debug.LogWarning("Rigidbody2Dコンポーネントが存在するか確認してください。");//Debug.LogWarningは、コンソールに警告メッセージを表示する関数。
            rigidbody2D.gravityScale = 0;//Rigidbody2DクラスのgravityScaleは、重力の影響を受けるかどうかを設定する変数。0の場合は、重力の影響を受けない。
        }

        //更新処理でも呼び出しできる重たい処理
        Player player = GetComponent<Player>();
        FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //関数内で使用する
        //クラス・コンポーネントの関数を呼び出す
        if ((rigidbody2D == null))
        {
            //nullチェック
            Debug.LogError("Rigidbody2Dコンポーネントが存在しません。");
            return;//returnは、関数の処理を終了するためのキーワード。void関数の場合は、return;と書く。
        }
    }

    /// <summary>
    /// 移動関数
    /// </summary>

    private void Move()
    {
        //移動処理
        //rigidbody2D.velocity = new Vector2(1.0f, 0.0f);//velocityは、Rigidbody2Dクラスの変数で、物体の速度を表す。new Vector2(1, 0)は、(1,0)の速度を表すVector2クラスのインスタンスを作成する。
    }

    private void OnDestroy()
    {
        
    }
}

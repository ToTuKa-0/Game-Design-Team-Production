using UnityEngine;

public class PlayerZoomLight : MonoBehaviour
{
    public Camera mainCamera;      // メインカメラ
    public Transform player;       // プレイヤー
    public float defaultSize = 5f; // 通常カメラサイズ（暗く見える範囲）
    public float lightSize = 3f;   // プレイヤー周りを明るくするサイズ
    public float smoothSpeed = 5f; // カメラサイズの補間速度
    public Vector3 initialPosition; // カメラの初期位置（画面全体が見える位置）

    private float targetSize;

    void Start()
    {
        targetSize = defaultSize;
        if (initialPosition == Vector3.zero)
            initialPosition = mainCamera.transform.position; // 初期位置を保存
    }

    void Update()
    {
        // カメラ位置をプレイヤーに追従
        Vector3 desiredPos = targetSize == defaultSize ? player.position : initialPosition;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,
                                                     new Vector3(desiredPos.x, desiredPos.y, mainCamera.transform.position.z),
                                                     Time.deltaTime * smoothSpeed);

        // カメラサイズを補間
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * smoothSpeed);
    }

    // プレイヤー周りを明るく
    public void BrightenPlayerArea()
    {
        targetSize = lightSize;
    }

    // 元の暗いサイズに戻す
    public void DimPlayerArea()
    {
        targetSize = defaultSize;
    }

    // カメラを初期位置に戻す
    public void ResetCameraPosition()
    {
        targetSize = defaultSize;
        mainCamera.transform.position = initialPosition;
    }
}

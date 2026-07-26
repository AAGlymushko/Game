using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerMovement playerMovement;
    CameraRotation cameraRotation;

    Transform playerCamera;

    private float scaleCamera = Constants.PLAYER_SIZE / 5;

    [Header("Скорость")]
    public float speedMovement;
    public float speedRotationX;
    public float speedRotationY;

    void Awake()
    {
        playerMovement = new PlayerMovement();
        cameraRotation = new CameraRotation();

        playerCamera = transform.Find("PlayerCamera");

        transform.localScale = new Vector3(Constants.PLAYER_SIZE, Constants.PLAYER_SIZE, Constants.PLAYER_SIZE);
        playerCamera.localScale = new Vector3(scaleCamera, scaleCamera, scaleCamera);
    }

    private void Start()
    {
        Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2f, Screen.height / 2f));
    }

    void Update()
    {
        cameraRotation.update(playerCamera.transform, speedRotationX, speedRotationY);

        playerMovement.update(transform, speedMovement);
    }
}

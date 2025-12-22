using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraZoomController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float smooth = 10f;
    [SerializeField] private float minFov = 30f;
    [SerializeField] private float maxFov = 70f;

    private float targetFov;

    private InputSystem_Actions inputSystem;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
        inputSystem.UI.ScrollWheel.performed += OnScroll;

        targetFov = cinemachineCamera.Lens.FieldOfView;
    }

    private void OnEnable() => inputSystem.Enable();
    private void OnDisable() => inputSystem.Disable();

    private void OnScroll(InputAction.CallbackContext ctx)
    {
        float scroll = ctx.ReadValue<Vector2>().y;
        targetFov -= scroll * zoomSpeed;
        targetFov = Mathf.Clamp(targetFov, minFov, maxFov);
    }

    private void Update()
    {
        float current = cinemachineCamera.Lens.FieldOfView;
        cinemachineCamera.Lens.FieldOfView =
            Mathf.Lerp(current, targetFov, Time.deltaTime * smooth);
    }


}

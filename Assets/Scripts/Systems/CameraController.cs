using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    [Header("References")]
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [Header("Properties")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float zoomSmooth = 8f;
    [SerializeField] private float minZoomDistance = 5f;
    [SerializeField] private float maxZoomDistance = 20f;

    private CinemachineFollow followComponent;
    private float targetZoom;
    private Vector3 zoomAngle;

    private void Start()
    {
        if(cinemachineCamera == null)
        {
            cinemachineCamera = GetComponent<CinemachineCamera>();
        }

        followComponent = cinemachineCamera.GetComponent<CinemachineFollow>();

        if (followComponent != null)
        {
            Vector3 initialOffset = followComponent.FollowOffset;
            targetZoom = initialOffset.magnitude;
            zoomAngle = initialOffset.normalized;
        }
        else
            Debug.LogError("CinemachineFollow not found");
    }



    private void Update()
    {
        //MOVEMENT

        Vector3 inputMoveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }
        
        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;

        //ROTATION

        Vector3 rotationVector = new Vector3(0, 0, 0);

        if(Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = +1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;

        //ZOOM

        if (followComponent == null) return;

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            targetZoom -= scrollInput * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoomDistance, maxZoomDistance);
        }

        Vector3 targetOffset = zoomAngle * targetZoom;
        followComponent.FollowOffset = Vector3.Lerp(followComponent.FollowOffset, targetOffset, Time.deltaTime * zoomSmooth);
        /*Vector3 currentOffset = followComponent.FollowOffset;
        float newZ = Mathf.Lerp(currentOffset.z, -targetZoom, Time.deltaTime * zoomSmooth);
        followComponent.FollowOffset = new Vector3(currentOffset.x, currentOffset.y, newZ);*/

    }

}





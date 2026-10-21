using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    [Header("References")]
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [Header("Properties")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float zoomSmooth = 8f;
    [SerializeField] private float minZoomDistance = 5f;
    [SerializeField] private float maxZoomDistance = 20f;
    [SerializeField] private float tiltSpeed = 2f;
    [SerializeField] private float maxTiltAngle = 65f;
    [SerializeField] private float tiltSmooth = 5f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float snapRotationAngle = 90f;



    private CinemachineFollow followComponent;
    private float targetZoom;
    private Vector3 zoomAngle;
    private float targetTilt;
    private float currentTilt;
    private float targetRotationY;


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
            targetRotationY = transform.eulerAngles.y;
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

        bool isRotating = Quaternion.Angle(transform.rotation, Quaternion.Euler(0f, targetRotationY, 0f)) > 0.1f;

        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                targetRotationY += snapRotationAngle;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                targetRotationY -= snapRotationAngle;
            }
        }

        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, targetRotationY, transform.eulerAngles.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //ZOOM

         if (followComponent == null) return;

         float scrollInput = Input.GetAxis("Mouse ScrollWheel");

         if (Mathf.Abs(scrollInput) > 0.01f)
         {
            if (targetZoom > minZoomDistance && scrollInput > 0f)
            {
                targetZoom -= scrollInput * zoomSpeed;
                targetZoom = Mathf.Max(targetZoom, minZoomDistance);
            }
            else if (scrollInput > 0f)
            {
                targetTilt += scrollInput * tiltSpeed * 25f;
                targetTilt = Mathf.Clamp(targetTilt, 0f, maxTiltAngle);
            }

            if (scrollInput < 0f)
            {
                if (targetTilt > 0f)
                {
                    targetTilt += scrollInput * tiltSpeed * 25f;
                    targetTilt = Mathf.Max(targetTilt, 0f);
                }
                else
                {
                    targetZoom -= scrollInput * zoomSpeed;
                    targetZoom = Mathf.Clamp(targetZoom,minZoomDistance,maxZoomDistance);
                }
            }

         }

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSmooth);
        Quaternion tiltRotation = Quaternion.AngleAxis(-currentTilt, Vector3.right);
        Vector3 tiltedAngle = tiltRotation * zoomAngle;
        Vector3 targetOffset = tiltedAngle * targetZoom;


         //Vector3 targetOffset = zoomAngle * targetZoom;
         followComponent.FollowOffset = Vector3.Lerp(followComponent.FollowOffset, targetOffset, Time.deltaTime * zoomSmooth); 
        

        

    }

   

}





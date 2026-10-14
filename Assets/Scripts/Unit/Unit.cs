using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed = 5f;
    private float stoppingDistance = 0.1f;
    [SerializeField] private Animator unitAnimator;

    [SerializeField] private float rotateSpeed = 20f;

    private GridPosition gridPosition;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) >= stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            LevelGrid.Instance.ChangeUnitGridPosition(gridPosition, newGridPosition, this);
            gridPosition = newGridPosition;
        }

    }

   public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }


}

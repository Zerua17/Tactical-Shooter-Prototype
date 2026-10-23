using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
       
        float spinAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);
        totalSpinAmount += spinAmount;

        if(totalSpinAmount >= 360f)
        {
            isActive = false;
        }
        
    }
    public void Spin()
    {
        totalSpinAmount = 0;
        isActive = true;
    }
}

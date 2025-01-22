using UnityEngine;

[CreateAssetMenu(menuName = "Implants/DoubleJumpImplant")]
public class DoubleJumpImplant : Implant
{
    private void OnEnable()
    {
        cost = 50; // Set the cost for the double jump implant
        duration = 25f; // Set the duration for the double jump implant
    }

    public override void ApplyEffect(GameObject target)
    {
        Debug.Log("DoubleJumpImplant applied");
        PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.EnableDoubleJump();
        }
    }

    public override void RemoveEffect(GameObject target)
    {
        PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.DisableDoubleJump();
        }
    }
}
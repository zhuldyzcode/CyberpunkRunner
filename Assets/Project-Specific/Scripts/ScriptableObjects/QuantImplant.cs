using UnityEngine;

[CreateAssetMenu(menuName = "Implants/QuantImplant")]
public class QuantImplant : Implant
{
    public float speedMultiplier;
    private float originalSpeed;
    private PlayerCollision playerCollision;

    public override void ApplyEffect(GameObject target)
    {

            originalSpeed = RoadGenerator.instance.speed;
            RoadGenerator.instance.speed *= speedMultiplier;
        playerCollision = target.GetComponent<PlayerCollision>();
        if (playerCollision != null)
        {
            playerCollision.ChangeIsCOllisionEnables();

        }
    }

    public override void RemoveEffect(GameObject target)
    {
            RoadGenerator.instance.speed = originalSpeed;

        if (playerCollision != null)
        {
            playerCollision.ChangeIsCOllisionEnables();
        }
    }
}
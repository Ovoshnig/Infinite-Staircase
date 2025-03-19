using UnityEngine;

public sealed class ThirdPersonLook : PersonLook
{
    protected override Transform FollowPoint => PlayerTransform.Find(PlayerConstants.PlayerHeadCenterName);
}

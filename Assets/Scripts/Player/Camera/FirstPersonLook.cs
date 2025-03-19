using UnityEngine;

public sealed class FirstPersonLook : PersonLook
{
    protected override Transform FollowPoint => PlayerTransform.Find(ZenjectIdConstants.PlayerEyeCenterName);
}

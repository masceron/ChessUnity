using System;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Class)]
public class FriendAttribute: Attribute
{
    public FriendAttribute(params Type[] allowedTypes)
    {
    }
}
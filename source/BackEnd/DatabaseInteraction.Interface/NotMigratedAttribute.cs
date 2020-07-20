using System;

namespace DatabaseInteraction.Interface
{
    /// <summary>
    /// Use this attribute when updating resources and the Entity has changed on the changed properties
    /// </summary>
    public class NotMigratedAttribute : Attribute
    {
    }
}

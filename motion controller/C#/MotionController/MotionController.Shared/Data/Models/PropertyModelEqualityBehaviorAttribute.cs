namespace MotionController.Data.Models;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class PropertyModelEqualityBehaviorAttribute : Attribute
{
    public bool Ignore { get; set; } = true;
}

namespace BlazorServer.Infrastructure.binder;

/// <summary>
/// Options class used by the <see cref="ConfigurationBinderExtension"/>.
/// </summary>
public class BinderOptionsExtension
{
    /// <summary>
    /// When false (the default), the binder will only attempt to set public properties.
    /// If true, the binder will attempt to set all non read-only properties.
    /// </summary>
    public bool BindNonPublicProperties { get; set; }

    /// <summary>
    /// When false (the default), the binder will only attempt to set property values by property name lookup.
    /// If true, the binder will attempt to use the custom property attribute names available to bind property values.
    /// </summary>
    public bool BindPropertyUsingAttributeNames { get; set; }
}

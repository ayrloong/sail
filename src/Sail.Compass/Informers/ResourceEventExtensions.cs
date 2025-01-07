namespace Sail.Compass.Informers;

public static class ResourceEventExtensions
{
    public static ResourceEvent<T> ToResourceEvent<T>(this T obj, EventType eventType, T oldValue = default)
    {
        if (eventType == EventType.Deleted && oldValue == null)
            oldValue = obj;
        return new ResourceEvent<T>(eventType, obj, oldValue);
    }
}

using Services;

public class Collection
{
    public EObject type;
    public bool ifEnable;
    public float lastPos;
    public float interval;

    public Collection(EObject type, float interval)
    {
        this.type = type;
        this.interval = interval;
        ifEnable = false;
        lastPos = 0;
    }
}
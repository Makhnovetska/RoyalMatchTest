
public class UIFaceLocationFactory: FactoryBase<UIFaceLocation>
{
    public static UIFaceLocationFactory Instance { get; private set; }

    private void Awake() => Instance = this;
}
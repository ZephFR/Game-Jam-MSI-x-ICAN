using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class WoolProgressBar : MonoBehaviour
{
    [Header("Progress source (same GameObject)")]
    public GameEventManager source;            

    [Header("Assets")]
    public Sprite stringSprite;
    public Sprite stringBackgroundSprite;
    public Sprite ballSprite;

    [Header("Look")]
    public Vector2 ballSize = new Vector2(48f, 48f);
    public float stringHeight = 12f;
    public int sliceLeft = 8;
    public int sliceRight = 8;

    [Header("Behaviour")]
    public float spinDegreesPerSecond = 40f;
    public float smoothing = 8f;

    VisualElement track;
    VisualElement fill;
    VisualElement ball;
    float shown;
    float angle;

    void Awake()
    {
        if (source == null)
            source = GetComponent<GameEventManager>();
    }

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        track = root.Q<VisualElement>("string-track");
        fill = root.Q<VisualElement>("string-fill");
        ball = root.Q<VisualElement>("wool-ball");
        ApplyAssets();
    }

    public void ApplyAssets()
    {
        if (track == null || fill == null || ball == null)
            return;

        track.style.height = stringHeight;
        fill.style.height = stringHeight;

        if (stringBackgroundSprite != null)
            track.style.backgroundImage = new StyleBackground(stringBackgroundSprite);

        if (stringSprite != null)
        {
            fill.style.backgroundImage = new StyleBackground(stringSprite);
            fill.style.unitySliceLeft = sliceLeft;
            fill.style.unitySliceRight = sliceRight;
        }

        if (ballSprite != null)
            ball.style.backgroundImage = new StyleBackground(ballSprite);

        ball.style.width = ballSize.x;
        ball.style.height = ballSize.y;
    }

    void Update()
    {
        if (fill == null || ball == null || source == null)
            return;

        float target = Mathf.Clamp01(source.Progress);   // <-- your variable here
        shown = Mathf.Lerp(shown, target, 1f - Mathf.Exp(-smoothing * Time.unscaledDeltaTime));

        Length pct = Length.Percent(shown * 100f);
        fill.style.width = pct;
        ball.style.left = pct;

        angle = (angle + spinDegreesPerSecond * Time.unscaledDeltaTime) % 360f;
        ball.style.rotate = new StyleRotate(new Rotate(angle));
    }
}
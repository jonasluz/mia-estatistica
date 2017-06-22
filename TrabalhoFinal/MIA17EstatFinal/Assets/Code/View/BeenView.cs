using UnityEngine;

public class BeenView : MonoBehaviour {

    [Header("Sprites")]
    public Sprite born;
    public Sprite healthy;
    public Sprite imune;
    public Sprite pseudoImunes;
    public Sprite infected;
    public Sprite deadByAge;
    public Sprite deadByAccident;
    public Sprite terrain;

    [Header("Conexões")]
    public SpriteRenderer sprite;
    public SpriteRenderer transition;
    public TextMesh debugIdText;

    public int DebugID
    {
        set
        {
            debugIdText.text = value.ToString();
        } 
        get
        {
            return int.Parse(debugIdText.text);
        }
    }

    protected Animator anim;

    #region Monobehaviour functions
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    #endregion Monobehaviour functions

    public void SetSprite()
    {
        SetSpriteImpl(deadByAccident);
    }
    public void SetSprite(bool byAccident = false)
    {
        //SetSpriteImpl(byAccident ? deadByAccident : deadByAge);
    }
    public void SetSprite(Being.HealthStatus state)
    {
        switch (state)
        {
            case Being.HealthStatus.HEALTHY:
                SetSpriteImpl(healthy);
                break;
            case Being.HealthStatus.IMUNE:
                SetSpriteImpl(imune);
                break;
            case Being.HealthStatus.INFECTED:
                SetSpriteImpl(infected);
                break;
            case Being.HealthStatus.PSEUDOIMUNE:
                SetSpriteImpl(pseudoImunes);
                break;
            case Being.HealthStatus.DEAD:
                SetSpriteImpl(deadByAccident);
                break;
        }
    }

    private void SetSpriteImpl(Sprite value)
    {
        transition.sprite = value;
        transition.gameObject.SetActive(true);
        anim.SetTrigger("FadeOut");
        while (! anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") ) { /* wait */ }
        sprite.sprite = value;
        transition.gameObject.SetActive(false);
    }
}

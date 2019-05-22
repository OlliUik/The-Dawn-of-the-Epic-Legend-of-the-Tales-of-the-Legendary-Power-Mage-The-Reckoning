using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private GameObject crosshair = null;
    [SerializeField] private GameObject goPause = null;
    [SerializeField] private GameObject goGameOver = null;
    [SerializeField] private GameObject goHPAndManaBars = null;
    [SerializeField] private Image healthBar = null;
    [SerializeField] private Image manaBar = null;
    [SerializeField] private Image hurtFlash = null;
    [SerializeField] private Image fadeIn = null;
    [SerializeField] private SpellIconSwitcher switcher = null;

    [SerializeField] private GameObject spellEditingUI = null;
    public SpellEditorController spellEditingController { get; private set; } 
    public bool bIsEditingSpells { get; private set; } = false;
    
    public bool bIsPaused { get; private set; } = false;

    private float hurtFlashReduceAmount = 0.5f;
    private float hurtFlashMaxAlpha = 0.2f;
    private PlayerCore cPlayerCore = null;

    #endregion

    #region UNITY_DEFAULT_METHODS

    private void Awake()
    {
        if(spellEditingUI != null)
        {
            spellEditingController = spellEditingUI.GetComponent<SpellEditorController>();
        }
    }

    void Update()
    {
        if (hurtFlash != null)
        {
            if (hurtFlash.color.a > 0.0f)
            {
                hurtFlash.color = new Color(1.0f, 0.0f, 0.0f, hurtFlash.color.a - hurtFlashReduceAmount * Time.deltaTime);
            }
            else
            {
                hurtFlash.color = Color.clear;
            }
        }

        if (fadeIn != null)
        {
            if (fadeIn.color.a > 0.0f)
            {
                fadeIn.color = new Color(0.0f, 0.0f, 0.0f, fadeIn.color.a - Time.deltaTime / 1.0f);
            }
            else
            {
                fadeIn.color = Color.clear;
            }
        }
    }

    void OnEnable()
    {
        if (fadeIn != null)
        {
            fadeIn.color = Color.black;
        }
    }

    #endregion

    #region CUSTOM_METHODS

    public void SetHealth(float amount, float max)
    {
        healthBar.rectTransform.localScale = new Vector3(amount / max, 1.0f, 1.0f);
    }

    public void SetMana(float amount, float max)
    {
        //manaBar.rectTransform.localScale = new Vector3(amount / max, 1.0f, 1.0f);
        manaBar.fillAmount = amount / max;
    }

    public bool FlipPauseState(PlayerCore pc)
    {
        cPlayerCore = pc;
        bIsPaused = !bIsPaused;
        goPause.SetActive(bIsPaused);
        crosshair.SetActive(!bIsPaused);
        goHPAndManaBars.SetActive(!bIsPaused);
        Time.timeScale = bIsPaused ? 0.0f : 1.0f;
        return bIsPaused;
    }

    public bool FlipSpellEditingState(PlayerCore pc)
    {
        cPlayerCore = pc;
        bIsEditingSpells = !bIsEditingSpells;
        spellEditingUI.SetActive(bIsEditingSpells);
        crosshair.SetActive(!bIsEditingSpells);
        goHPAndManaBars.SetActive(!bIsEditingSpells);
        Time.timeScale = bIsEditingSpells ? 0.0f : 1.0f;

        spellEditingController.useCrystalButton.gameObject.SetActive(true);
        spellEditingController.useCrystalButton.interactable = spellEditingController.crystalsLeft > 0 ? true : false;

        return bIsEditingSpells;
    }

    //Use previous caller if no PlayerInput is specified
    //(example: pressing an UI button resumes the game)
    public void FlipPauseState()
    {
        if (cPlayerCore != null)
        {
            cPlayerCore.EnableControls(!FlipPauseState(cPlayerCore));
        }
    }

    public void ChangeSpell(int spellNumber)
    {
        if (switcher != null)
        {
            switcher.ChangeIcon(spellNumber);
        }
    }

    public void OnPlayerHurt()
    {
        hurtFlash.color = new Color(1.0f, 0.0f, 0.0f, hurtFlashMaxAlpha);
    }

    public void OnPlayerDeath()
    {
        OnPlayerHurt();
        goHPAndManaBars.SetActive(false);
        goGameOver.SetActive(true);
        crosshair.SetActive(false);
    }

    #endregion
}

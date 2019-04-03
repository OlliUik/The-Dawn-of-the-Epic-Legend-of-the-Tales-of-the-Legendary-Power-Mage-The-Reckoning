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

    [SerializeField] private GameObject spellEditingUI = null;
    private SpellEditorController controller = null;
    public bool bIsEditingSpells { get; private set; } = false;
    
    public bool bIsPaused { get; private set; } = false;

    private float hurtFlashReduceAmount = 0.5f;
    private float hurtFlashMaxAlpha = 0.2f;
    private PlayerCore cPlayerCore = null;

    #endregion

    #region UNITY_DEFAULT_METHODS

    private void Start()
    {
        controller = spellEditingUI.GetComponent<SpellEditorController>();
    }

    void Update()
    {
        if (hurtFlash.color.a > 0.0f)
        {
            hurtFlash.color = new Color(1.0f, 0.0f, 0.0f, hurtFlash.color.a - hurtFlashReduceAmount * Time.deltaTime);
        }
        else
        {
            hurtFlash.color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
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
        manaBar.rectTransform.localScale = new Vector3(amount / max, 1.0f, 1.0f);
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

        controller.useCrystalButton.gameObject.SetActive(true);
        controller.useCrystalButton.interactable = controller.crystalsLeft > 0 ? true : false;

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

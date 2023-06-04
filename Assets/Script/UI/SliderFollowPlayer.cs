using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SliderFollowPlayer : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's Transform component
    public Slider HPslider; // Reference to the Slider component
    public TMP_Text Nametext;

    private RectTransform HPsliderRectTransform;
    private RectTransform NametextRectTransform;
    public PlayerRenderCharacter playerRender;
    public OnClickToPlayer onClickToPlayer;
    [SerializeField] private PlayWhenReady playWhenReady;
    [SerializeField] private DeathController deathController;

    private void Start()
    {
        HPsliderRectTransform = HPslider.GetComponent<RectTransform>();
        NametextRectTransform = Nametext.GetComponent<RectTransform>();
        playWhenReady = GameObject.FindGameObjectWithTag("TurnBaseManager").GetComponent<PlayWhenReady>();
        deathController = GameObject.FindGameObjectWithTag("DeathManager").GetComponent<DeathController>();
        SetBarEnable(false);
    }

    private void LateUpdate()
    {
        if (playerTransform != null && playWhenReady.isStarted == true && deathController.isDeath == false && onClickToPlayer.isZoomToCharacter == false)
        {
            SetBarEnable(true);
            SetBarPosition();
            SetBarValue();
        }
        else if (deathController.isDeath == true || onClickToPlayer.isZoomToCharacter == true)
        {
            SetBarEnable(false);
        }
    }

    private void SetBarEnable(bool isEnabled)
    {
        Nametext.enabled = isEnabled;
        HPslider.gameObject.SetActive(isEnabled);
    }

    private void SetBarPosition()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(playerTransform.position);
        HPsliderRectTransform.position = new Vector2(screenPosition.x, screenPosition.y + 60);
        NametextRectTransform.position = new Vector2(screenPosition.x, screenPosition.y + 100);
    }   
    private void SetBarValue()
    {
        HPslider.maxValue = playerRender.characterData.characters[playerRender.PlayerId].characteStat.HP;
        HPslider.value = playerRender.PlayerHP;
        Nametext.text = playerRender.PlayerName;
    }
}

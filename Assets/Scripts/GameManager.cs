using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public ImageTimer HarvestTime;
    public ImageTimer FoodEatTime;
    public ImageTimer RaidTime;
    public ButtonsUI ButtonManager;

    [SerializeField] private AudioSource _backgroundSongAudio;
    [SerializeField] private AudioSource _buttonClickAudio;
    [SerializeField] private AudioSource _harvestingAudio;
    [SerializeField] private AudioSource _foodEatingAudio;
    [SerializeField] private AudioSource _raidBattleAudio;
    [SerializeField] private AudioSource _warriorHireAudio;
    [SerializeField] private AudioSource _peasantHireAudio;

    [SerializeField] private GameObject _winObject;
    [SerializeField] private GameObject _loseObject;
    [SerializeField] private GameObject _allAudioOffImg;
    [SerializeField] private GameObject _gamePaused;


    [SerializeField] private int _wheatsCount;
    [SerializeField] private int _warriorsCount;
    [SerializeField] private int _peasantsCount;

    private bool isHirePeasants;
    private float _countingReloadHirePeasants;
    [SerializeField] private float _peasantsReloadTime;
    [SerializeField] private byte _peasantsCost;

    private bool isHireWarriors;
    private float _countingReloadHireWarriors;
    [SerializeField] private float _warriorsReloadTime;
    [SerializeField] private byte _warriorsCost;

    [SerializeField] private Text _wheatsCountText;
    [SerializeField] private Text _warriorsCountText;
    [SerializeField] private Text _peasantsCountText;
    [SerializeField] private Text _raidersCountAttackText;
    [SerializeField] private Text _wheatsForWarriorsText;
    [SerializeField] private Text _wheatsOfHarvestingText;
    [SerializeField] private Text _winGameResultText;
    [SerializeField] private Text _loseGameResultText;

    [SerializeField] private Button _peasantsHireButton;
    [SerializeField] private Button _warriorsHireButton;

    [SerializeField] private Image _peasantsReloadImage;
    [SerializeField] private Image _warriorsReloadImage;

    private int _raidersCount = 0;
    private byte _raidTicks = 0;

    private int _wheatsStartValue;
    private int _peasantsStartValue;
    private int _warriorsStartValue;
    private int _raidersStartValue;
    private byte _raidsStartTicks;

    private int _resultWheats = 0;
    private int _resultPeasants = 0;
    private int _resultWarriors = 0;
    private int _resultWarriorsDie = 0;
    private byte _resultRaidWins = 0;

    private void Start()
    {
        _peasantsStartValue = _peasantsCount;
        _raidersStartValue = _raidersCount;
        _warriorsStartValue = _warriorsCount;
        _raidsStartTicks = _raidTicks;
        _wheatsStartValue = _wheatsCount;
        RetryGame();
        
    }

    private void Update()
    {
        if (isHirePeasants && _countingReloadHirePeasants > 0)
        {
            _countingReloadHirePeasants -= Time.deltaTime;
            _peasantsReloadImage.fillAmount = _countingReloadHirePeasants / _peasantsReloadTime;
        }
        else if (_countingReloadHirePeasants < 0)
        {
            isHirePeasants = false;
            _peasantsHireButton.interactable = true;
            _countingReloadHirePeasants = _peasantsReloadTime;
            _peasantsReloadImage.fillAmount = 1;
        }

        if (isHireWarriors && _countingReloadHireWarriors > 0)
        {
            _countingReloadHireWarriors -= Time.deltaTime;
            _warriorsReloadImage.fillAmount = _countingReloadHireWarriors / _warriorsReloadTime;
        }
        else if (_countingReloadHireWarriors < 0)
        {
            isHireWarriors = false;
            _warriorsHireButton.interactable = true;
            _countingReloadHireWarriors = _warriorsReloadTime;
            _warriorsReloadImage.fillAmount = 1;
        }

        if(HarvestTime.Tick)
        {
            _harvestingAudio.Play();
            _wheatsCount += _peasantsCount * _peasantsCost;
            _resultWheats += _peasantsCount * _peasantsCost;
            _wheatsCountText.text = _wheatsCount.ToString();
            _wheatsOfHarvestingText.text = "+" + (_peasantsCount * _peasantsCost).ToString() + " сбор урожая";
            
        }

        if(FoodEatTime.Tick)
        {
            _foodEatingAudio.Play();
            _wheatsCount -= _warriorsCost * _warriorsCount;
            _wheatsCountText.text = _wheatsCount.ToString();
        }

        CheckWheatsForButton();


        if (RaidTime.Tick)
        {
            if (_raidersCount > 0)
            {
                _raidBattleAudio.Play();
            }

            if (_warriorsCount < _raidersCount)
            {
                _resultWarriorsDie = _warriorsCount - (_warriorsCount - _raidersCount);
            }
            else _resultWarriorsDie += _raidersCount;
            _warriorsCount -= _raidersCount;
            _raidersCount += 1;

            if (_raidTicks >= 2)
            {
                _raidersCount += 1;
            }
            _raidTicks++;
            _warriorsCountText.text = _warriorsCount.ToString();
            _raidersCountAttackText.text = _raidersCount.ToString();
            _wheatsForWarriorsText.text = "-" + (_warriorsCount * _warriorsCost).ToString() + " цикл еды";
            _resultRaidWins += 1;
        }

        if (_wheatsCount < 0 || _warriorsCount < 0) LoseGame();
        else if (_wheatsCount > 300 && _peasantsCount >= 30) WinGame();
    }

    public void HirePeasant()
    {
        _peasantHireAudio.Play();
        _countingReloadHirePeasants = _peasantsReloadTime;
        _peasantsCount += 1;
        _resultPeasants += 1;
        _peasantsCountText.text = _peasantsCount.ToString();
        _wheatsCount -= _peasantsCost;
        _wheatsCountText.text = _wheatsCount.ToString();
        _wheatsOfHarvestingText.text = "+" + (_peasantsCount * _peasantsCost).ToString() + " сбор урожая";
        isHirePeasants = true;
        _peasantsHireButton.interactable = false;

    }

    public void HireWarrior()
    {
        _warriorHireAudio.Play();
        _countingReloadHireWarriors = _warriorsReloadTime;
        _warriorsCount += 1;
        _resultWarriors += 1;
        _warriorsCountText.text = _warriorsCount.ToString();
        _wheatsCount -= _warriorsCost;
        _wheatsCountText.text = _wheatsCount.ToString();
        _wheatsForWarriorsText.text = "-" + (_warriorsCount * _warriorsCost).ToString() + " цикл еды";
        isHireWarriors = true;
        _warriorsHireButton.interactable = false;

    }

    private void CheckWheatsForButton()
    {
        if(_wheatsCount < _peasantsCost) _peasantsHireButton.interactable = false;
        else if(_wheatsCount > _peasantsCost && isHirePeasants == false) _peasantsHireButton.interactable = true;

        if (_wheatsCount < _warriorsCost) _warriorsHireButton.interactable = false;
        else if (_wheatsCount > _warriorsCost && isHireWarriors == false) _warriorsHireButton.interactable = true;
    }

    private void LoseGame()
    {
        Time.timeScale = 0;
        _loseGameResultText.text = $"Произведено зерна: {_resultWheats}" +
                                   $"\r\nНанято крестьян: {_resultPeasants}" +
                                   $"\r\nНанято воинов: {_resultWarriors}" +
                                   $"\r\n\r\nПогибло воинов: {_resultWarriorsDie}" +
                                   $"\r\nНабегов пережито: {_resultRaidWins}";
        RaidTime.Tick = false;
        _loseObject.SetActive(true);
    }

    private void WinGame()
    {
        Time.timeScale = 0;
        _winGameResultText.text = $"Произведено зерна: {_resultWheats}" +
                                   $"\r\nНанято крестьян: {_resultPeasants}" +
                                   $"\r\nНанято воинов: {_resultWarriors}" +
                                   $"\r\n\r\nПогибло воинов: {_resultWarriorsDie}" +
                                   $"\r\nНабегов пережито: {_resultRaidWins}";
        RaidTime.Tick = false;
        _winObject.SetActive(true);
    }

    public void RetryGame()
    {
        _peasantsCount = _peasantsStartValue;
        _raidersCount = _raidersStartValue;
        _warriorsCount = _warriorsStartValue;
        _raidTicks = _raidsStartTicks;
        _wheatsCount = _wheatsStartValue;

        HarvestTime.RetryTime();
        FoodEatTime.RetryTime();
        RaidTime.RetryTime();

        isHirePeasants = false;
        isHireWarriors = false;
        _warriorsReloadImage.fillAmount = 1;
        _peasantsReloadImage.fillAmount = 1;

        _resultWheats = 0;
        _resultPeasants = 0;
        _resultWarriors = 0;
        _resultWarriorsDie = 0;
        _resultRaidWins = 0;

    _wheatsCountText.text = _wheatsStartValue.ToString();
        _warriorsCountText.text = _warriorsStartValue.ToString();
        _peasantsCountText.text = _peasantsStartValue.ToString();
        _raidersCountAttackText.text = _raidersStartValue.ToString();
        _wheatsForWarriorsText.text = "-" + (_warriorsCount * _warriorsCost).ToString() + " цикл еды";
        _wheatsOfHarvestingText.text = "+" + (_peasantsCount * _peasantsCost).ToString() + " сбор урожая";

        _resultWheats += _wheatsCount;
        _resultPeasants += _peasantsCount;
        _resultWarriors += _warriorsCount;

        Time.timeScale = 1;

    }

    public void OnOffAudio()
    {
        if (_allAudioOffImg.active == false)
        {
            _backgroundSongAudio.mute = true;
            _foodEatingAudio.mute = true;
            _harvestingAudio.mute = true;
            _raidBattleAudio.mute = true;
            _warriorHireAudio.mute = true;
            _peasantHireAudio.mute = true;
            _allAudioOffImg.SetActive(true);
        }
        else if (_allAudioOffImg.active == true)
        {
            _backgroundSongAudio.mute = false;
            _foodEatingAudio.mute = false;
            _harvestingAudio.mute = false;
            _raidBattleAudio.mute = false;
            _warriorHireAudio.mute = false;
            _peasantHireAudio.mute = false;
            _allAudioOffImg.SetActive(false);
        }
    }

    public void PauseGame()
    {
        if(_gamePaused.active == false)
        {
            Time.timeScale = 0;
            _peasantsHireButton.interactable = false;
            _warriorsHireButton.interactable = false;
            _gamePaused.SetActive(true);
        }
        else if(_gamePaused.active == true)
        {
            Time.timeScale = 1;
            _peasantsHireButton.interactable = true;
            _warriorsHireButton.interactable = true;
            _gamePaused.SetActive(false);
        }
        
    }

}

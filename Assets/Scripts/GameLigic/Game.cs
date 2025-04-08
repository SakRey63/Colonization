using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class Game : MonoBehaviour
{
    [SerializeField] private List<Base> _bases;
    [SerializeField] private Bot _firstBot;
    [SerializeField] private SpawnerResources _spawnerResources;
    [SerializeField] private SpawnerBaseView _spawnerBaseView;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private BuilderBasePreview _builderBasePreview;
    [SerializeField] private float _delaySpawnResource;
    [SerializeField] private float _delayOutputTextHint;
    [SerializeField] private TextMeshProUGUI _textHint;
    [SerializeField] private TextMeshProUGUI _textFewBots;
    [SerializeField] private TextMeshProUGUI _textErrorBuilds;
    [SerializeField] private int _minCountBots;

    private int _numberBase = 1;
    private RaycastHit _hitInfo;
    private Base _baseResource;
    private Bot _botBuilder;

    private void OnEnable()
    {
        _inputReader.ClickedMouse += BuildNewBase;
        _inputReader.TurnToRight += SetRotationBase;
        _inputReader.TurnToLeft += SetRotationBase;

        foreach (Base newBase in _bases)
        {
            newBase.AssignedBot += AddBotBuilder;
        }
    }

    private void OnDisable()
    {
        _inputReader.ClickedMouse -= BuildNewBase;
        _inputReader.TurnToRight -= SetRotationBase;
        _inputReader.TurnToLeft -= SetRotationBase;
        
        foreach (Base newBase in _bases)
        {
            newBase.AssignedBot -= AddBotBuilder;
        }
    }

    private void Start()
    {
        StartCoroutine(RepeatResource());
        
        _bases[0].ArrangeNewBots(_firstBot);

        DisplayBaseInformation(_bases[0]);
    }
    
    private IEnumerator RepeatResource()
    {
        WaitForSeconds delay = new WaitForSeconds(_delaySpawnResource);

        while (enabled)
        {
            SetAreaSpawnResources();

            yield return delay;
        }
    }

    private IEnumerator OutputTextHint(TextMeshProUGUI text)
    {
        WaitForSeconds delay = new WaitForSeconds(_delayOutputTextHint);

        text.gameObject.SetActive(true);
        
        yield return delay;
        
        text.gameObject.SetActive(false);
    }
    
    private void BuildNewBase()
    {
        if (_builderBasePreview.IsChoosingPositions == false)
        {
            FindBase();
        }
        else
        {
            CreateFlagNewPositionBase();
        }
    }

    private void CreateFlagNewPositionBase()
    {
        if (_builderBasePreview.BasePreview.IsPossibleBuild)
        {
            _builderBasePreview.ReturnToPoolBasePreview();
            
            _baseResource.CreateFlag(_builderBasePreview.PositionBuild);
        }
        else
        {
            StartCoroutine(OutputTextHint(_textErrorBuilds));
        }
    }

    private void AddBotBuilder(Bot bot)
    {
        _botBuilder = bot;
        _botBuilder.FinishedBuildsNewBase += AddNewBase;
    }

    private void AddNewBase(Base newBase)
    {
        _botBuilder.FinishedBuildsNewBase -= AddNewBase;
        
        newBase.AssignedBot += AddBotBuilder;
        
        _bases.Add(newBase);
        
        DisplayBaseInformation(newBase);
    }

    private void FindBase()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hitInfo);
        
        if (_hitInfo.transform.TryGetComponent(out Base baseResource))
        {
            _baseResource = baseResource;
            
            if (_baseResource.CountBots > _minCountBots)
            {
                _baseResource.ChangeStatus();
                                
                _builderBasePreview.CreateBasePreview(_baseResource.transform);
            }
            else
            {
                StartCoroutine(OutputTextHint(_textFewBots));
            }
        }
        else
        {
            StartCoroutine(OutputTextHint(_textHint)); 
        }
    }

    private void SetRotationBase(float rotationAngle)
    {
        _builderBasePreview.SetRotationBase(rotationAngle);
    }

    private void SetAreaSpawnResources()
    {
        foreach (Base playerBase in _bases)
        {
            _spawnerResources.SetAreaResource(playerBase.AreaScanningBase);
        }
    }

    private void DisplayBaseInformation(Base toBase)
    {
        BaseView baseView = _spawnerBaseView.GetTextBaseInfo();

        toBase.CreateDisplayView(baseView, _numberBase);
                
        _numberBase++;
    }
}
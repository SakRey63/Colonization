using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private List<Base> _bases;
    [SerializeField] private Bot _firstBot;
    [SerializeField] private Camera _camera;
    [SerializeField] private SpawnerBaseConstructionOrchestrator _spawnerBaseConstructionOrchestrator;
    [SerializeField] private SpawnerResources _spawnerResources;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private BuilderBasePreview _builderBasePreview;
    [SerializeField] private float _delayOutputTextHint;
    [SerializeField] private TextMeshProUGUI _textHint;
    [SerializeField] private TextMeshProUGUI _textFewBots;
    [SerializeField] private TextMeshProUGUI _textErrorBuilds;
    [SerializeField] private int _minCountBots;

    private int _numberBase = 1;
    private RaycastHit _hitInfo;
    private Base _baseResource;
    private HashSet<int> _numbersUsedBases;
    private Coroutine _coroutineHint;

    private void OnEnable()
    {
        _inputReader.ClickedMouse += HandleBaseClick;
        _inputReader.TurnToRight += SetRotationBase;
        _inputReader.TurnToLeft += SetRotationBase;
    }

    private void OnDisable()
    {
        _inputReader.ClickedMouse -= HandleBaseClick;
        _inputReader.TurnToRight -= SetRotationBase;
        _inputReader.TurnToLeft -= SetRotationBase;
    }

    private void Awake()
    {
        _numbersUsedBases = new HashSet<int>();
    }

    private void Start()
    {
        _bases[0].ArrangeBots(_firstBot);

        DisplayBaseInformation(_bases[0]);
    }
    
    private IEnumerator OutputTextHint(TextMeshProUGUI text)
    {
        WaitForSeconds delay = new WaitForSeconds(_delayOutputTextHint);

        text.gameObject.SetActive(true);
        
        yield return delay;
        
        text.gameObject.SetActive(false);

        _coroutineHint = null;
    }
    
    private void HandleBaseClick()
    {
        if (_builderBasePreview.IsChoosingPositions == false)
        {
            FindBaseForBuilding();
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
            _baseResource = null;
        }
        else
        {
            SetOutputTextHint(_textErrorBuilds);
        }
    }

    private void AddNewBase(Base newBase, BaseConstructionOrchestrator baseConstructionOrchestrator)
    {
        baseConstructionOrchestrator.FinishedBuildsNewBase -= AddNewBase;

        _numbersUsedBases.Remove(newBase.NumberBase);
        
        _bases.Add(newBase);
        
        DisplayBaseInformation(newBase);

        _spawnerBaseConstructionOrchestrator.ReturnInPool(baseConstructionOrchestrator);
    }

    private void FindBaseForBuilding()
    {
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out _hitInfo))
        {
            if (_hitInfo.transform.TryGetComponent(out Base clickedBase))
            {
                if (clickedBase.CountBots > _minCountBots)
                {
                    _baseResource = clickedBase;
                            
                    clickedBase.EnableBuildMode();
                                            
                    _builderBasePreview.CreateBasePreview(clickedBase.transform);

                    if (_numbersUsedBases.Contains(clickedBase.NumberBase) == false)
                    {
                        _numbersUsedBases.Add(clickedBase.NumberBase);
                        
                        BaseConstructionOrchestrator baseConstructionOrchestrator = _spawnerBaseConstructionOrchestrator.GetBaseConstructionOrchestrator();
                                                
                        baseConstructionOrchestrator.PrepareNewBaseBuilding(clickedBase);
                                            
                        baseConstructionOrchestrator.FinishedBuildsNewBase += AddNewBase;
                    }
                }
                else
                {
                    SetOutputTextHint(_textFewBots);
                }
            }
            else
            {
                SetOutputTextHint(_textHint);
            }
        }
        else
        {
            SetOutputTextHint(_textHint);
        }
    }

    private void DisableAllTextHint()
    {
        _textHint.gameObject.SetActive(false);
        _textFewBots.gameObject.SetActive(false);
        _textErrorBuilds.gameObject.SetActive(false);
    }

    private void SetOutputTextHint(TextMeshProUGUI text)
    {
        if (_coroutineHint != null)
        {
            StopCoroutine(_coroutineHint);

            DisableAllTextHint();
                        
            _coroutineHint = StartCoroutine(OutputTextHint(text));
        }
        else
        {
            _coroutineHint = StartCoroutine(OutputTextHint(text));
        }
    }

    private void SetRotationBase(float rotationAngle)
    {
        _builderBasePreview.SetRotationBase(rotationAngle);
    }

    private void DisplayBaseInformation(Base toBase)
    {
        toBase.CreateDisplayView(_numberBase, _camera);
                
        _numberBase++;
    }
}
using System.Collections.Generic;
using System.Linq;
using KModkit;
using UnityEngine;
using UnityEngine.UI;
using wawa.Extensions;
using wawa.Modules;

public sealed class GraduationModule : ModdedModule
{
    [SerializeField]
    private GameObject _mainArea;

    [SerializeField]
    private Text _majorDisplay;

    [SerializeField]
    private Text _minorDisplay;

    [SerializeField]
    private KMSelectable _minorButtonLeft;

    [SerializeField]
    private KMSelectable _minorButtonRight;

    [SerializeField]
    private KMSelectable[] _courseButtons;

    [SerializeField]
    private GameObject _extra1Area;

    [SerializeField]
    private KMSelectable[] _extra1Selectables;

    [SerializeField]
    private GameObject _extra2Area;

    [SerializeField]
    private KMSelectable[] _extra2Selectables;

    [SerializeField]
    private GameObject _extra3Area;

    [SerializeField]
    private RectTransform[] _extra3Anchors;

    [SerializeField]
    private Image _extra3Shape;

    [SerializeField]
    private Sprite[] _extra3Shapes;

    [SerializeField]
    private KMSelectable _extra3Left;

    [SerializeField]
    private KMSelectable _extra3Right;

    [SerializeField]
    private KMSelectable _extra3Submit;

    [SerializeField]
    private Text _extra3KeyText;

    [SerializeField]
    private Color[] _buttonColors;

    private static readonly string[] s_colorNames = new[]
    {
        "red",
        "green",
        "blue",
        "yellow",
    };

    private static readonly string[] s_minorList = new[]
    {
        "Forensics",
        "Aerospace Engineering",
        "Public Health",
        "Computer Science",
        "Sociology",
        "Creative Writing",
        "French",
        "Communications",
        "Business",
        "International Relations",
    };

    private static readonly Major[] s_majorList = new[]
    {
        new Major("Criminal Justice", "CRIM 100", "CRIM 343", "BIOL 117"),
        new Major("Engineering Mechanics", "MATH 162", "ME 225", "ME 205"),
        new Major("Dietetics and Nutrition", "HA&P 1", "CHEM 1020L", "HUN 1201"),
        new Major("Epidemiology", "ECP 4530", "PHC 4030", "GIS 4043/L"),
        new Major("Graphic Design", "GRA 2208C", "ARH 2051", "2190C"),
        new Major("Biblical Studies", "APOL 201", "BIBL 350", "THEO 350"),
        new Major("Fashion Design", "FASH 212", "FASH 335", "FASH 466"),
        new Major("Psychology", "PSYC 292", "PSYC 3710", "MATH 152"),
        new Major("Zoology", "BSC 2010", "PCB 4043C", "PCB 4674"),
    };

    private static readonly Dictionary<string, int[]> s_extra1Rules = new Dictionary<string, int[]>
    {
        ["0:1:2"] = new[] { 1, 2, 0 },
        ["0:2:1"] = new[] { 1, 0, 2 },
        ["2:0:1"] = new[] { 0, 1, 2 },
        ["2:1:0"] = new[] { 0, 2, 1 },
        ["1:2:0"] = new[] { 2, 0, 1 },
        ["1:0:2"] = new[] { 2, 1, 0 },
    };

    private static readonly Dictionary<string, int[]> s_extra2Rules = new Dictionary<string, int[]>
    {
        ["0:2:3:1"] = new[] { 1, 2, 0, 3 },
        ["0:2:1:3"] = new[] { 1, 2, 3, 0 },
        ["0:3:2:1"] = new[] { 1, 3, 0, 2 },
        ["0:3:1:2"] = new[] { 1, 3, 2, 0 },
        ["0:1:2:3"] = new[] { 1, 0, 3, 2 },
        ["0:1:3:2"] = new[] { 1, 0, 2, 3 },
        ["2:0:3:1"] = new[] { 0, 2, 1, 3 },
        ["2:0:1:3"] = new[] { 0, 2, 3, 1 },
        ["2:3:0:1"] = new[] { 0, 3, 1, 2 },
        ["2:3:1:0"] = new[] { 0, 3, 2, 1 },
        ["2:1:0:3"] = new[] { 0, 1, 3, 2 },
        ["2:1:3:0"] = new[] { 0, 1, 2, 3 },
        ["1:0:3:2"] = new[] { 3, 2, 1, 0 },
        ["1:0:2:3"] = new[] { 3, 2, 0, 1 },
        ["1:3:0:2"] = new[] { 3, 0, 1, 2 },
        ["1:3:2:0"] = new[] { 3, 0, 2, 1 },
        ["1:2:0:3"] = new[] { 3, 1, 0, 2 },
        ["1:2:3:0"] = new[] { 3, 1, 2, 0 },
        ["3:0:1:2"] = new[] { 2, 0, 1, 3 },
        ["3:0:2:1"] = new[] { 2, 0, 3, 1 },
        ["3:1:0:2"] = new[] { 2, 3, 1, 0 },
        ["3:1:2:0"] = new[] { 2, 3, 0, 1 },
        ["3:2:0:1"] = new[] { 2, 1, 3, 0 },
        ["3:2:1:0"] = new[] { 2, 1, 0, 3 },
    };

    private static readonly CoordRule[] s_extra3Rules = new[]
    {
        new CoordRule(2, 0, 0),
        new CoordRule(2, 1, 1),
        new CoordRule(2, 2, 2),
        new CoordRule(2, 3, 3),
        new CoordRule(1, 0, 4),
        new CoordRule(1, 1, 5),
        new CoordRule(1, 2, 6),
        new CoordRule(1, 3, 7),
        new CoordRule(0, 0, 8),
        new CoordRule(0, 1, 9),
        new CoordRule(0, 2, 10),
        new CoordRule(0, 3, 11),
        new CoordRule(3, 0, 12),
        new CoordRule(3, 1, 13),
        new CoordRule(3, 2, 14),
        new CoordRule(3, 3, 15),
    };

    private int _currentStage;

    private int _selectedMinor;

    private Major _currentMajor;

    private List<string> _majorCourses;

    private int _correctMinor;

    private int _correctCourse;

    private List<string> _courseLabels;

    private bool _isExtra;

    private int[] _extraColorSource;

    private int[] _extraColorSequence;

    private int _extraColorProgress;

    private int _extra3Solution;

    private int _extra3Selected;

    private string[] _ignoreList;

    private void Start()
    {
        _ignoreList = Get<KMBossModule>().GetIgnoredModuleIDs(Get<KMBombModule>(), new[]
        {
            Get<KMBombModule>().ModuleType,
        });

        _minorButtonLeft.Add(onInteract: OnMinorLeft);
        _minorButtonRight.Add(onInteract: OnMinorRight);

        for (int i = 0; i < _courseButtons.Length; i++)
        {
            int j = i;
            _courseButtons[i].Add(onInteract: () => SelectCourse(j));
        }

        for (int i = 0; i < _extra1Selectables.Length; i++)
        {
            int j = i;
            _extra1Selectables[i].Add(onInteract: () => ChooseExtra1(j));
        }

        for (int i = 0; i < _extra2Selectables.Length; i++)
        {
            int j = i;
            _extra2Selectables[i].Add(onInteract: () => ChooseExtra2(j));
        }

        _extra3Left.Add(onInteract: OnExtra3Left);
        _extra3Right.Add(onInteract: OnExtra3Right);
        _extra3Submit.Add(onInteract: OnExtra3Submit);

        UpdateMinorDisplay();

        var bombInfo = Get<KMBombInfo>();
        _correctMinor = (bombInfo.GetBatteryCount() + bombInfo.GetIndicators().Count()) * bombInfo.GetSerialNumberNumbers().Last() % 10;

        _currentMajor = s_majorList.PickRandom();
        _majorDisplay.text = _currentMajor.Name;
        _majorCourses = new List<string>(_currentMajor.Courses);

        Log($"Your major is {_currentMajor.Name}");
        Log($"Your minor is {s_minorList[_correctMinor]}");

        RandomizeStage();
        UpdateChildren();
    }

    private int TotalSolvable => Get<KMBombInfo>().GetSolvableModuleIDs().Count(id => !_ignoreList.Contains(id));

    private int NumSolved => Get<KMBombInfo>().GetSolvedModuleIDs().Count(id => !_ignoreList.Contains(id));

    private int TargetPercent => (_currentStage + 1) * 25;

    private int TargetSolved => Mathf.FloorToInt(TotalSolvable * TargetPercent / 100f);

    private void RandomizeStage()
    {
        var decoyMajors = new List<Major>(s_majorList);
        decoyMajors.Remove(_currentMajor);

        _correctCourse = Random.Range(0, 3);
        _courseLabels = new List<string>();

        for (int i = 0; i < _courseButtons.Length; i++)
        {
            string text;
            if (i == _correctCourse)
            {
                text = _majorCourses.PickRandom();
                _majorCourses.Remove(text);
            }
            else
            {
                var decoy = decoyMajors.PickRandom();
                decoyMajors.Remove(decoy);
                text = decoy.Courses.PickRandom();
            }
            _courseButtons[i].GetComponentInChildren<Text>().text = text;
            _courseLabels.Add(text);
        }

        Log($"Stage {_currentStage + 1}: Your course options are {_courseLabels.Select((label, index) => index == _correctCourse ? label + "*" : label).Join(", ")}");
    }

    private void UpdateMinorDisplay() => _minorDisplay.text = s_minorList[_selectedMinor];

    private void OnMinorLeft()
    {
        Shake(_minorButtonLeft, 0.25f, Sound.ButtonPress);

        if (!Status.IsSolved)
        {
            _selectedMinor--;
            if (_selectedMinor < 0)
                _selectedMinor = s_minorList.Length - 1;
            UpdateMinorDisplay();
        }
    }

    private void OnMinorRight()
    {
        Shake(_minorButtonRight, 0.25f, Sound.ButtonPress);

        if (!Status.IsSolved)
        {
            _selectedMinor++;
            if (_selectedMinor >= s_minorList.Length)
                _selectedMinor = 0;
            UpdateMinorDisplay();
        }
    }

    private void StageComplete()
    {
        _currentStage++;
        if (_currentStage >= 3)
        {
            Solve("Module solved!");
        }
        else
        {
            RandomizeStage();
        }
    }

    private void ExtraComplete()
    {
        _extra1Area.SetActive(false);
        _extra2Area.SetActive(false);
        _extra3Area.SetActive(false);
        _mainArea.SetActive(true);
        _isExtra = false;
        UpdateChildren();
        StageComplete();
    }

    private void SelectCourse(int index)
    {
        Shake(_courseButtons[index], 1f, Sound.ButtonPress);

        if (!Status.IsSolved)
        {
            Log($"Selected course {_courseLabels[index]} and minor {s_minorList[_selectedMinor]}");

            if (index == _correctCourse && _selectedMinor == _correctMinor)
            {
                Log($"Solved modules: {NumSolved} of {TotalSolvable} - expecting {TargetPercent}% = {TargetSolved}");
                if (NumSolved > TargetSolved)
                {
                    InitializeExtraCredit();
                }
                else if (NumSolved < TargetSolved)
                {
                    Strike("Strike! (too early)");
                }
                else
                {
                    StageComplete();
                }
            }
            else
            {
                Strike("Strike!");
            }
        }
    }

    private IEnumerable<KMSelectable> GetChildren()
    {
        if (_isExtra)
        {
            switch (_currentStage)
            {
                case 0:
                    // already in a 3-wide row :3
                    return _extra1Selectables;
                case 1:
                    return new[]
                    {
                        // row 1
                        _extra2Selectables[0],
                        _extra2Selectables[1],
                        _extra2Selectables[1],
                        // row 2
                        _extra2Selectables[2],
                        _extra2Selectables[3],
                        _extra2Selectables[3],
                    };
                case 2:
                    return new[]
                    {
                        _extra3Left,
                        _extra3Submit,
                        _extra3Right,
                    };
            }
        }

        return new[]
        {
            // row 1
            _minorButtonLeft,
            _minorButtonRight,
            _minorButtonRight,
            // row 2
            _courseButtons[0],
            _courseButtons[0],
            _courseButtons[0],
            // row 3
            _courseButtons[1],
            _courseButtons[1],
            _courseButtons[1],
            // row 4
            _courseButtons[2],
            _courseButtons[2],
            _courseButtons[2],
        };
    }

    private void UpdateChildren()
    {
        Get<KMSelectable>().Children = GetChildren().ToArray();
        Get<KMSelectable>().UpdateChildrenProperly();
    }

    private void InitializeExtraCredit()
    {
        _isExtra = true;
        _mainArea.SetActive(false);
        Log($"Late for class! Entering extra credit stage {_currentStage + 1}");
        switch (_currentStage)
        {
            case 0:
                {
                    _extra1Area.SetActive(true);
                    var displayColors = Enumerable.Range(0, 3).ToList().Shuffle();
                    for (int i = 0; i < displayColors.Count; i++)
                        _extra1Selectables[i].GetComponent<Renderer>().material.color = _buttonColors[displayColors[i]];
                    _extraColorSource = displayColors.ToArray();
                    _extraColorSequence = s_extra1Rules[displayColors.Join(":")];
                    _extraColorProgress = 0;
                    Log($"Selected colors: {displayColors.Select(x => s_colorNames[x]).Join(", ")}");
                    Log($"Correct sequence: {_extraColorSequence.Select(x => s_colorNames[x]).Join(", ")}");
                    break;
                }
            case 1:
                {
                    _extra2Area.SetActive(true);
                    var displayColors = Enumerable.Range(0, 4).ToList().Shuffle();
                    for (int i = 0; i < displayColors.Count; i++)
                        _extra2Selectables[i].GetComponent<Renderer>().material.color = _buttonColors[displayColors[i]];
                    _extraColorSource = displayColors.ToArray();
                    _extraColorSequence = s_extra2Rules[displayColors.Join(":")];
                    _extraColorProgress = 0;
                    Log($"Selected colors: {displayColors.Select(x => s_colorNames[x]).Join(", ")}");
                    Log($"Correct sequence: {_extraColorSequence.Select(x => s_colorNames[x]).Join(", ")}");
                    break;
                }
            case 2:
                {
                    _extra3Area.SetActive(true);
                    int quadrant = Random.Range(0, 4);
                    int shape = Random.Range(0, _extra3Shapes.Length);
                    _extra3Shape.sprite = _extra3Shapes[shape];
                    _extra3Shape.rectTransform.anchoredPosition = _extra3Anchors[quadrant].anchoredPosition + new Vector2(Random.Range(-80f, 80f), Random.Range(-80f, 80f));
                    _extra3Selected = 0;
                    _extra3Solution = s_extra3Rules.First(rule => rule.Quadrant == quadrant && rule.Shape == shape).Solution;
                    UpdateExtra3Display();
                    Log($"Selected {_extra3Shape.sprite.name} in quadrant {new[] { "I", "II", "III", "IV" }[quadrant]}");
                    Log($"Solution: key {_extra3Solution + 1}");
                    break;
                }
        }
        UpdateChildren();
    }

    private void ChooseExtra1(int index)
    {
        Shake(_extra1Selectables[index], 1f, Sound.ButtonPress);
        Log($"Selected {s_colorNames[_extraColorSource[index]]}, expected {s_colorNames[_extraColorSequence[_extraColorProgress]]}");
        if (_extraColorSource[index] == _extraColorSequence[_extraColorProgress])
        {
            _extraColorProgress++;
            if (_extraColorProgress >= 3)
            {
                ExtraComplete();
            }
        }
        else
        {
            Strike("Strike!");
        }
    }

    private void ChooseExtra2(int index)
    {
        Shake(_extra2Selectables[index], 1f, Sound.ButtonPress);
        Log($"Selected {s_colorNames[_extraColorSource[index]]}, expected {s_colorNames[_extraColorSequence[_extraColorProgress]]}");
        if (_extraColorSource[index] == _extraColorSequence[_extraColorProgress])
        {
            _extraColorProgress++;
            if (_extraColorProgress >= 4)
            {
                ExtraComplete();
            }
        }
        else
        {
            Strike("Strike!");
        }
    }

    private void UpdateExtra3Display() => _extra3KeyText.text = $"Key {_extra3Selected + 1}";

    private void OnExtra3Left()
    {
        Shake(_extra3Left, 0.25f, Sound.ButtonPress);

        _extra3Selected--;
        if (_extra3Selected < 0)
            _extra3Selected = 15;

        UpdateExtra3Display();
    }

    private void OnExtra3Right()
    {
        Shake(_extra3Right, 0.25f, Sound.ButtonPress);

        _extra3Selected++;
        if (_extra3Selected >= 16)
            _extra3Selected = 0;

        UpdateExtra3Display();
    }

    private void OnExtra3Submit()
    {
        Shake(_extra3Submit, 1f, Sound.ButtonPress);

        Log($"Submitted key {_extra3Selected + 1}");
        if (_extra3Selected == _extra3Solution)
        {
            ExtraComplete();
        }
        else
        {
            Strike("Strike!");
        }
    }
}

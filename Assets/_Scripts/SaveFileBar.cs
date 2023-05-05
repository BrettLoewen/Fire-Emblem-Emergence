using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveFileBar: MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI activityText;
    [SerializeField] private TextMeshProUGUI timestampText;
    [SerializeField] private GameObject emptySaveFileText;

    public Button button { get; private set; }

    private SaveData data;
    private int index;
    private SaveFileScreen saveFileScreen;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        
    }//end Start

    // Update is called once per frame
    void Update()
    {
        
    }//end Update

    #endregion //end Unity Control Methods

    #region

    public void Setup(SaveData _data, int _index, SaveFileScreen _saveFileScreen)
    {
        data = _data;
        index = _index;
        saveFileScreen = _saveFileScreen;

        if(data == null)
        {
            emptySaveFileText.SetActive(true);
            activityText.gameObject.SetActive(false);
            timestampText.gameObject.SetActive(false);
        }
        else
        {
            emptySaveFileText.SetActive(false);
            activityText.gameObject.SetActive(true);
            timestampText.gameObject.SetActive(true);

            activityText.text = data.activity;
            timestampText.text = data.savedAtTimestamp;
        }

        button = GetComponent<Button>();
    }

    public void SetNavigationLinks(SaveFileBar up, SaveFileBar down)
    {
        Navigation navigation = button.navigation;

        navigation.selectOnUp = up.button;
        navigation.selectOnDown = down.button;

        button.navigation = navigation;
    }

    public async void OnClick()
    {
        await saveFileScreen.OnSaveFileBarClick(index);
    }

    #endregion
}
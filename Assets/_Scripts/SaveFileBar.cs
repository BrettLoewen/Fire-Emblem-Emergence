using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveFileBar: MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI activityText;
    [SerializeField] private TextMeshProUGUI timestampText;
    [SerializeField] private GameObject emptySaveFileText;

    private SaveData data;

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

    public void SetSaveData(SaveData _data)
    {
        data = _data;
    }

    #endregion
}
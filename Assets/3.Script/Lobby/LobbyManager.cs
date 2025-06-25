using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    OptionPanelController optionPanelController;

    void Start()
    {
        TryGetComponent(out optionPanelController);

        InitPanel();
    }

    void InitPanel()
    {
        optionPanelController.SetActivePanel(false);
    }

    public void OnClickOptionButton()
    {
        optionPanelController.SetActivePanel(true);
    }
}

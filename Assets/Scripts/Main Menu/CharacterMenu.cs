using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Palmmedia.ReportGenerator.Core;
using UnityEngine.SceneManagement;

public class CharacterMenu : MonoBehaviour
{
    [Serializable]
    public class CharacterSelect
    {
        public TMP_InputField nameInput;
        public TMP_Dropdown typeDropdown;
        public TMP_Dropdown colorDropdown;
        public Toggle toggle;
    }
    [SerializeField] CharacterSelect[] playerSelection;

    public void StartButton()
    {
        foreach (var player in playerSelection)
        {
            if (player.toggle.isOn)
            {
                Setting newSet = new Setting(player.nameInput.text, player.typeDropdown.value, player.colorDropdown.value);
                GameSettings.AddSetting(newSet);
            }
        }
        SceneManager.LoadScene("new-monopoly");
    }
}

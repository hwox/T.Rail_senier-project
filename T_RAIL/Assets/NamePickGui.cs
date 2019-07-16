using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(ChatGui))]
public class NamePickGui : MonoBehaviourPunCallbacks
{
    private const string UserNamePlayerPref = "NamePickUserName";

    public ChatGui chatNewComponent;

    //public InputField idInput;

    public void Start()
    {
        DontDestroyOnLoad(this);

        this.chatNewComponent = FindObjectOfType<ChatGui>();

        string prefsName = PhotonNetwork.LocalPlayer.NickName;
        //string prefsName = Prefs.GetString(NamePickGui.UserNamePlayerPref);
        //if (!string.IsNullOrEmpty(prefsName))
        //{
        //    this.idInput.text = prefsName;
        //}

        StartChat();
    }


    // new UI will fire "EndEdit" event also when loosing focus. So check "enter" key and only then StartChat.
    public void EndEditOnEnter()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            this.StartChat();
        }
    }

    public void StartChat()
    {
        ChatGui chatNewComponent = FindObjectOfType<ChatGui>();
        chatNewComponent.UserName = PhotonNetwork.LocalPlayer.NickName; // this.idInput.text.Trim();
        chatNewComponent.Connect();
        enabled = false;

        PlayerPrefs.SetString(NamePickGui.UserNamePlayerPref, chatNewComponent.UserName);
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TMPro;

using UnityEngine;

public class MessageLogger : MonoBehaviour
{
    [SerializeField] private TMP_Text _displayMessage;
    [SerializeField] private int _logLimit = 5;

    private List<string> _messageLog;

    private void Start() {
        _messageLog = new List<string>();
    }

    public void OnDebugMessage(string message) {
         if (_displayMessage == null)
             return;
        
         if (_messageLog.Count >= _logLimit) 
             _messageLog.Remove(_messageLog.First());

         _messageLog.Add(message);

         var compiledLog = new StringBuilder();
         foreach (var msgLine in _messageLog)
             compiledLog.AppendLine(msgLine);

         _displayMessage.text = compiledLog.ToString();
    }
}

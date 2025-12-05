using Fungus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace top.Isteyft.MCS.IsTools.DialogEvent.UI
{
    public class ExtendedSayDialog
    {
        private SayDialog sayDialog;

        public string NameText
        {
            get
            {
                return sayDialog.NameText;
            }
            set
            {
                sayDialog.NameText = value;
            }
        }

        public ExtendedSayDialog(SayDialog dialog)
        {
            sayDialog = dialog;
        }

        public void SetChengHao(string text)
        {
            FieldInfo field = typeof(SayDialog).GetField("chengHao", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null)
            {
                Text text2 = (Text)field.GetValue(sayDialog);
                if (text2 != null)
                {
                    text2.text = text;
                }
            }
        }

        public void SetCharacter(Character character, int characterID = 0)
        {
            sayDialog.SetCharacter(character, characterID);
        }

        public void SetCharacterImage(Sprite image, int characterID = 0)
        {
            sayDialog.SetCharacterImage(image, characterID);
        }
    }
}

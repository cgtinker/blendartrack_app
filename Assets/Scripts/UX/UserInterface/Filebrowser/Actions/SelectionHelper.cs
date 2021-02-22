using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArRetarget;

namespace ArRetarget
{
    public class SelectionHelper : MonoBehaviour
    {
        public void SelectTodaysFiles(List<JsonDirectory> jsonDirectories)
        {
            if (jsonDirectories.Count == 0)
                return;

            string daytime = FileManagement.GetDateTime();
            var curTime = FileManagement.StringToInt(daytime);    //conversion to get rid of signs
            string today = FileManagement.RemoveLengthFromEnd(curTime.ToString(), 6);

            foreach (JsonDirectory data in jsonDirectories)
            {
                //get only year / month / day
                string day = FileManagement.RemoveLengthFromEnd(data.value.ToString(), 6);

                if (today == day)
                {
                    var btn = data.obj.GetComponent<JsonFileButton>();
                    btn.ChangeSelectionToggleStatus(true);
                }

                else
                {
                    var btn = data.obj.GetComponent<JsonFileButton>();
                    btn.ChangeSelectionToggleStatus(false);
                }
            }
            HighlightSelectBtnText(0);
        }

        public void SelectAllFiles(List<JsonDirectory> jsonDirectories)
        {
            if (jsonDirectories.Count == 0)
                return;

            foreach (JsonDirectory data in jsonDirectories)
            {
                var btn = data.obj.GetComponent<JsonFileButton>();
                btn.ChangeSelectionToggleStatus(true);
            }

            HighlightSelectBtnText(1);
        }

        public void DeselectAllFiles(List<JsonDirectory> jsonDirectories)
        {
            if (jsonDirectories.Count == 0)
                return;

            foreach (JsonDirectory data in jsonDirectories)
            {
                var btn = data.obj.GetComponent<JsonFileButton>();
                btn.ChangeSelectionToggleStatus(false);
            }

            HighlightSelectBtnText(2);
        }

        //TODO: implement states for highlighting SelectionBtnText
        public void HighlightSelectBtnText(int index)
        {
            //PurgeOrphans.PurgeOrphanZips("");
            /*
            for (int i = 0; i < selectBtnTextList.Count; i++)
            {
                if (i == index)
                {
                    if (selectBtnTextList[i].fontStyle != FontStyles.Underline)
                    {
                        selectBtnTextList[i].fontStyle = FontStyles.Underline;
                    }
                }

                else
                {
                    if (selectBtnTextList[i].fontStyle != FontStyles.Normal)
                        selectBtnTextList[i].fontStyle = FontStyles.Normal;
                }
            }
            */
        }
    }
}

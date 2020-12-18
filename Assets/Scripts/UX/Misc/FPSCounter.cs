using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public int FPS;
    public TextMeshProUGUI disp_text;
    const string disp_name = "FPS: ";
    const int step = 30;


    int current;
    List<int> cur_list = new List<int>();
    int index = 0;
    int tmp = 0;

    void Update()
    {
        index++;

        if (index < step)
        {
            current = (int)(1f / Time.unscaledDeltaTime);
            cur_list.Add(current);
        }
        else
        {
            tmp = 0;
            index = 0;

            for (int i = 0; i < cur_list.Count; i++)
            {
                tmp += cur_list[i];
            }

            FPS = tmp / cur_list.Count;
            disp_text.text = $"{disp_name}{FPS}";
        }
    }
}

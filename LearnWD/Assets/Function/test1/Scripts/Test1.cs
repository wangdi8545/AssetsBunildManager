using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class Test1 : MonoBehaviour {

	void Start () {
        
    }
	
	void Update ()
	{
        //1-9   = 1

	    //1-99    1*10+10 = 20;

       // 1-999   20*10+100 = 300

        //1-9999  300*10+1000 = 4000

        //1-99999   4000*10+10000 = 50000
	}
    [ContextMenu("AllOneTest2")]
    public void AllOneTest2()
    {
        int count = 10000;
        int m = GetDigit(count)-1;
        int n = (int)(Mathf.Pow(10,m ) - 1);
        n = GetOneToV(n);
        Debug.Log(n);

    }

    int GetOneToV(int v)
    {
        int n = v;
        int m = 0;
        if (n < 10)
        {
            m = 1;
        }
        else
        {
           m = GetOneToV(n/10)*10+(int)Mathf.Pow(10, GetDigit(n)-1);
        }
        Debug.Log(v+"   "+m);
        return m;
    }

    int GetDigit(int v)
    {
        if (v < 10)
        {
            return 1;
        }
        else
        {
            return GetDigit(v/10) + 1;
        }
    }

    [ContextMenu("AllOneTest1")]
    public void AllOneTest1()
    {
        int index = 0;
        for (int i = 1; i <= 1000; i++)
        {
            if (i < 10)
            {
                if (i == 1)
                {
                    Debug.Log(i);
                    index++;
                }
            }else if (i < 100)
            {
                int n = (int)(i/10);
                int m = i%10;

                if (n == 1)
                {
                    Debug.Log(i);

                    index++;
                }
                if (m == 1)
                {
                    Debug.Log(i);

                    index++;
                }
            }
            else if (i < 1000)
            {
                int b = (i/100);
                int n = ((int) (i)/10)%10;
                int m = i%10;

                if (b == 1)
                {
                    Debug.Log(i);

                    index++;
                }
                if (n == 1)
                {
                    Debug.Log(i);

                    index++;
                }
                if (m == 1)
                {
                    Debug.Log(i);

                    index++;
                }

            }
            else if (i < 10000)
            {
                int v = (i/1000);
                int b = (i / 100)%10;
                int n = ((int)(i) / 10) % 10;
                int m = i % 10;

                if (v == 1)
                {
                    Debug.Log(i);

                    index++;
                }
                if (b == 1)
                {
                    Debug.Log(i);

                    index++;
                }
                if (n == 1)
                {
                    Debug.Log(i);

                    index++;
                }
                if (m == 1)
                {
                    Debug.Log(i);

                    index++;
                }
            }
            else
            {
                Debug.Log(i);

                index++;
            }
        }
        Debug.Log(index);
    }


}

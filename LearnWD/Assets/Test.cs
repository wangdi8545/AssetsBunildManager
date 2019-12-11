using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour {

	void Start ()
	{

	    Dictionary<string, TestData> td = new Dictionary<string, TestData>();

        td.Add("1",new TestData("AAA"));
        Debug.Log(td["1"].name);

        List<TestData> list = td.Values.ToList();
	    list[0].name = "12345";


        Debug.Log(td["1"].name);

    }

    void Update () {
		
	}
}


public class TestData
{
    public string name;

    public TestData(string name)
    {
        this.name = name;
    }
}
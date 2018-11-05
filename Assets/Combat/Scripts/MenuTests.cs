using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuTests : IPrebuildSetup{
	GameObject grid;
	GenerateBattlemap map;

	GameObject Units;
    public void Setup()
    {

    }
	void SetScene()
	{
		SceneManager.LoadScene("Map");
	}
	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator MainMenuOpened()
	{
        SceneManager.LoadScene("Menu");
        yield return new WaitForFixedUpdate();
        //open up map
        Assert.AreEqual(SceneManager.GetActiveScene().name, "Map");
	}

	[UnityTest]
	public IEnumerator MainMenuClosed()
	{
        SceneManager.LoadScene("Menu");
        yield return new WaitForFixedUpdate();
        //simulate quit
		Assert.AreEqual(SceneManager.GetActiveScene(), null);
	}

	[UnityTest]
	public IEnumerator GameMenuOpenClose()
	{
        SceneManager.LoadScene("TestBattle");
        yield return new WaitForFixedUpdate();
        //open game menu
        Assert.True(false);
        //close game menu
        Assert.True(false);
    }

	[UnityTest]
	public IEnumerator AudioMenuOpenClose()
	{
		SceneManager.LoadScene("TestBattle");
        yield return new WaitForFixedUpdate();
        //open audio menu
        Assert.True(false);
        //close audio menu
        Assert.True(false);
    }
	[UnityTest]
	public IEnumerator GraphicMenuOpenClose()
	{
		SceneManager.LoadScene("TestBattle");
        yield return new WaitForFixedUpdate();
        //open graphics menu
        Assert.True(false);
        //close graphics menu
        Assert.True(false);
    }
	[UnityTest]
	public IEnumerator LoadGameMenuOpenClose()
	{
		SceneManager.LoadScene("TestBattle");
        yield return new WaitForFixedUpdate();
        //open game menu
        Assert.True(false);
        //close game menu
        Assert.True(false);
    }
	[UnityTest]
	public IEnumerator SaveGameMenuOpenClose()
	{
		SceneManager.LoadScene("TestBattle");
        yield return new WaitForFixedUpdate();
        //simulate key press
		Assert.True(false);
	}
	[UnityTest]
	public IEnumerator NewGameMenuOpenClose()
	{
        SceneManager.LoadScene("Map");
        //load Testbattle from Map
        yield return new WaitForFixedUpdate();
        Assert.AreEqual(SceneManager.GetActiveScene().name, "TestBattle");
	}
	public IEnumerator ItemMenuOpenClose()
	{
        SceneManager.LoadScene("Map");
        //load items
        yield return new WaitForFixedUpdate();
        Assert.AreEqual(SceneManager.GetActiveScene().name, "Items");
	}

	public IEnumerator ToolTipBreakdown()
	{
		SceneManager.LoadScene("ItemMenu"); //eventually Map
		yield return new WaitForFixedUpdate();
		Assert.IsTrue(false);
	}

	public IEnumerator ToolTipDisplay()
	{
		SceneManager.LoadScene("ItemMenu"); //eventually Map
		yield return new WaitForFixedUpdate();
		Assert.IsTrue(false);
	}

	public IEnumerator ToolTipClippingCheck()
	{
		SceneManager.LoadScene("ItemMenu"); //eventually Map
		yield return new WaitForFixedUpdate();
		Assert.IsTrue(false);
	}

	public IEnumerator ToolTipRemoval()
	{
		SceneManager.LoadScene("ItemMenu"); //eventually Map
		yield return new WaitForFixedUpdate();
		Assert.IsTrue(false);
	}


}
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemTest : IPrebuildSetup{



	public void Setup()
	{

	}
	void SetUpWithScene()
	{

	}
	void SetScene()
	{
		GameObject ScrollMenu = GameObject.Find ("ScrollMenu");
		ItemControl IT = ScrollMenu.GetComponent<ItemControl>();
		IT.item = GameObject.Find ("ArmourUP");
		IT.Create(IT.item);
		//Players = Resources.Load("Prefabs/PlayerParent", typeof(GameObject)) as GameObject;
		//GameObject Units = new GameObject("Units");
		//Units = Resources.Load("Prefabs/UnitParent", typeof(GameObject)) as GameObject;
		//grid = new GameObject();
		//grid.GetComponent<CellGrid>().PlayersParent = Players.transform;
	}
	
	[UnityTest]
	public IEnumerator ListedItemGrid()
	{
		SceneManager.LoadScene("ItemMenu");
		yield return new WaitForFixedUpdate();
		GameObject ScrollMenu = GameObject.Find ("ScrollMenu");
		ItemControl IT = ScrollMenu.GetComponent<ItemControl>();
		IT.item = GameObject.Find ("ArmourUP");
		IT.Hide(IT.item);
		yield return new WaitForFixedUpdate();
		Assert.IsTrue(!IT.item.activeSelf);
	}

	[UnityTest]
	public IEnumerator ItemDisplayed()
	{
		SceneManager.LoadScene("ItemMenu");
		yield return new WaitForFixedUpdate();
		GameObject ScrollMenu = GameObject.Find ("ScrollMenu");
		ItemControl IT = ScrollMenu.GetComponent<ItemControl>();
		IT.item = GameObject.Find ("ArmourUP");
		yield return new WaitForFixedUpdate();
		GameObject InfoPanelImage = GameObject.Find ("Image");
		DisplayItemInfo II; //Item Info
		II = InfoPanelImage.GetComponent<DisplayItemInfo>();
		Assert.AreEqual(II.item, GameObject.Find ("ArmourUP"));
	}
	[UnityTest]
	public IEnumerator RecharableItem_Recharged()
	{
		SceneManager.LoadScene("TestBattle");
		yield return new WaitForFixedUpdate();
		Assert.IsTrue (false);
	}
	[UnityTest]
	public IEnumerator ConsumableItem_Consumed()
	{
		SceneManager.LoadScene("TestBattle");
		yield return new WaitForFixedUpdate();
		Assert.IsTrue (false);
	}
	[UnityTest]
	public IEnumerator ExpendableItem_Refilled()
	{
		SceneManager.LoadScene("TestBattle");
		yield return new WaitForFixedUpdate();
		Assert.IsTrue (false);
	}
	[UnityTest]
	public IEnumerator PassiveItem_Active()
	{
		SceneManager.LoadScene("TestBattle");
		yield return new WaitForFixedUpdate();
		Assert.IsTrue (false);
	}
	[UnityTest]
	public IEnumerator InstantItem_Active()
	{
		SceneManager.LoadScene("TestBattle");
		yield return new WaitForFixedUpdate();
		Assert.IsTrue (false);
	}
	[UnityTest]
	public IEnumerator Item_Hidden()
	{
		SceneManager.LoadScene("ItemMenu");
		yield return new WaitForFixedUpdate();
		Assert.IsTrue (false);
	}
	[UnityTest]
	public IEnumerator StatChange_MouseHoverOverItem()
	{
		SceneManager.LoadScene("ItemMenu");
		yield return new WaitForFixedUpdate();
		Assert.IsTrue (false);
	}
}

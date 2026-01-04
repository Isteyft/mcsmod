using System;
using System.Collections.Generic;
using JSONClass;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200033C RID: 828
public class MainUISelectTianFu : MonoBehaviour
{
	// Token: 0x06001C85 RID: 7301 RVA: 0x000CBDA8 File Offset: 0x000C9FA8
	public void Init()
	{
		if (!this.isInit)
		{
			this.tianfuNum.text = "0";
			foreach (JSONObject json in jsonData.instance.CreateAvatarJsonData.list)
			{
				MainUITianFuCell tianfuCell = UnityEngine.Object.Instantiate<GameObject>(this.tianFuCell, this.tianFuList).GetComponent<MainUITianFuCell>();
				tianfuCell.Init(json);
				if (this.tianFuPageList.ContainsKey(tianfuCell.page))
				{
					this.tianFuPageList[tianfuCell.page].Add(tianfuCell);
				}
				else
				{
					this.tianFuPageList.Add(tianfuCell.page, new List<MainUITianFuCell>
					{
						tianfuCell
					});
				}
				tianfuCell.toggle.valueChange.AddListener(delegate()
				{
					if (tianfuCell.toggle.isOn)
					{
						if (this.hasSelectSeidList.ContainsKey(20))
						{
							int num = -1;
							foreach (int num2 in this.hasSelectSeidList[20])
							{
								if (jsonData.instance.CrateAvatarSeidJsonData[20][num2.ToString()]["value1"].ToList().Contains(tianfuCell.toggle.group))
								{
									num = num2;
									break;
								}
							}
							if (num > 0)
							{
								this.hasSelectList[num].toggle.isOn = false;
								this.hasSelectList[num].toggle.OnValueChange();
							}
						}
						foreach (int key in tianfuCell.seidList)
						{
							if (this.hasSelectSeidList.ContainsKey(key))
							{
								this.hasSelectSeidList[key].Add(tianfuCell.id);
							}
							else
							{
								this.hasSelectSeidList.Add(key, new List<int>
								{
									tianfuCell.id
								});
							}
						}
						this.hasSelectList.Add(tianfuCell.id, tianfuCell);
						this.AddTianFuDian(-tianfuCell.costNum);
						if (tianfuCell.seidList.Contains(19))
						{
							this.AddTianFuDian(jsonData.instance.CrateAvatarSeidJsonData[19][tianfuCell.id.ToString()]["value1"].I);
						}
						if (tianfuCell.seidList.Contains(20))
						{
							List<int> list = jsonData.instance.CrateAvatarSeidJsonData[20][tianfuCell.id.ToString()]["value1"].ToList();
							List<MainUITianFuCell> list2 = new List<MainUITianFuCell>();
							foreach (int key2 in this.hasSelectList.Keys)
							{
								if (list.Contains(this.hasSelectList[key2].toggle.group))
								{
									list2.Add(this.hasSelectList[key2]);
								}
							}
							for (int i = 0; i < list2.Count; i++)
							{
								list2[i].toggle.isOn = false;
								list2[i].toggle.OnValueChange();
							}
							return;
						}
					}
					else
					{
						bool flag = false;
						foreach (int key3 in tianfuCell.seidList)
						{
							if (this.hasSelectSeidList.ContainsKey(key3))
							{
								if (this.hasSelectSeidList[key3].Contains(tianfuCell.id))
								{
									this.hasSelectSeidList[key3].Remove(tianfuCell.id);
									if (!flag)
									{
										this.AddTianFuDian(tianfuCell.costNum);
										if (tianfuCell.seidList.Contains(19))
										{
											this.AddTianFuDian(-jsonData.instance.CrateAvatarSeidJsonData[19][tianfuCell.id.ToString()]["value1"].I);
										}
										this.hasSelectList.Remove(tianfuCell.id);
										flag = true;
									}
								}
								if (this.hasSelectSeidList[key3].Count == 0)
								{
									this.hasSelectSeidList.Remove(key3);
								}
							}
						}
					}
				});
				tianfuCell.toggle.clickEvent.AddListener(new UnityAction(MainUIPlayerInfo.inst.UpdataBase));
			}
			this.CostSort();
			this.isInit = true;
		}
		if (MainUIPlayerInfo.inst.sex == 1)
		{
			this.man.SetActive(true);
			this.woman.SetActive(false);
		}
		else if (MainUIPlayerInfo.inst.sex == 2)
		{
			this.man.SetActive(false);
			this.woman.SetActive(true);
		}
		this.ShowCurPageList();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x00004095 File Offset: 0x00002295
	public void ClickCell(bool isOn)
	{
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x000CBF68 File Offset: 0x000CA168
	private void CostSort()
	{
		foreach (int num in this.tianFuPageList.Keys)
		{
			if (num > 2)
			{
				this.tianFuPageList[num].Sort((MainUITianFuCell x, MainUITianFuCell y) => x.costNum.CompareTo(y.costNum));
			}
		}
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x000CBFF0 File Offset: 0x000CA1F0
	public void ShowCurPageList()
	{
		this.UpdateDesc();
		foreach (MainUITianFuCell mainUITianFuCell in this.tianFuPageList[this.curPage])
		{
			mainUITianFuCell.transform.SetAsLastSibling();
			mainUITianFuCell.gameObject.SetActive(true);
		}
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x000CC064 File Offset: 0x000CA264
	public void NextPage()
	{
		if (!this.CheckCurHasSelect())
		{
			UIPopTip.Inst.Pop("至少选择一个天赋", PopTipIconType.叹号);
			return;
		}
		if (this.setLingGen.CheckLingGen())
		{
			UIPopTip.Inst.Pop("请选择对应数目的灵根", PopTipIconType.叹号);
			return;
		}
		if (this.curPage == 8)
		{
			if (this.tianfuDian < 0)
			{
				UIPopTip.Inst.Pop("天赋点不能为负数", PopTipIconType.叹号);
				return;
			}
			this.HideCurPage();
			this.curPage++;
			this.ShowFinallyPage();
			return;
		}
		else
		{
			this.HideCurPage();
			this.curPage++;
			if (this.curPage == 5)
			{
				this.UpdateDesc();
				this.shenYuNum.SetActive(false);
				this.setLingGen.Init();
				return;
			}
			if (!this.shenYuNum.activeSelf)
			{
				this.shenYuNum.SetActive(true);
				this.setLingGen.gameObject.SetActive(false);
			}
			this.ShowCurPageList();
			return;
		}
	}

	// Token: 0x06001C8A RID: 7306 RVA: 0x000CC154 File Offset: 0x000CA354
	public void LastPage()
	{
		if (this.curPage == 9)
		{
			this.nextBtn.SetActive(true);
			this.finallyPage.SetActive(false);
		}
		this.HideCurPage();
		this.curPage--;
		if (this.curPage == 5)
		{
			this.UpdateDesc();
			this.shenYuNum.SetActive(false);
			this.setLingGen.Init();
			return;
		}
		if (this.curPage == 0)
		{
			this.curPage = 1;
			base.gameObject.SetActive(false);
			MainUIMag.inst.createAvatarPanel.setFacePanel.Init();
			return;
		}
		if (!this.shenYuNum.activeSelf)
		{
			this.shenYuNum.SetActive(true);
			this.setLingGen.gameObject.SetActive(false);
		}
		this.ShowCurPageList();
	}

	// Token: 0x06001C8B RID: 7307 RVA: 0x000CC220 File Offset: 0x000CA420
	public void HideCurPage()
	{
		if (this.tianFuPageList.ContainsKey(this.curPage))
		{
			foreach (MainUITianFuCell mainUITianFuCell in this.tianFuPageList[this.curPage])
			{
				mainUITianFuCell.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06001C8C RID: 7308 RVA: 0x000CC294 File Offset: 0x000CA494
	public bool CheckCurHasSelect()
	{
		if (this.tianFuPageList.ContainsKey(this.curPage))
		{
			using (List<MainUITianFuCell>.Enumerator enumerator = this.tianFuPageList[this.curPage].GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.toggle.isOn)
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}
		return true;
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x000CC314 File Offset: 0x000CA514
	public void UpdateDesc()
	{
		this.title.text = CreateAvatarMiaoShu.DataDict[this.curPage].title;
		int num = this.curPage - 1;
		this.desc.text = "";
		if (num > 1 && num != 5)
		{
			foreach (int key in this.hasSelectList.Keys)
			{
				if (this.hasSelectList[key].page == num)
				{
					Text text = this.desc;
					text.text = text.text + jsonData.instance.CreateAvatarJsonData[this.hasSelectList[key].id.ToString()]["Info"].Str + "\n";
				}
			}
		}
		Text text2 = this.desc;
		text2.text = text2.text + CreateAvatarMiaoShu.DataDict[this.curPage].Info + "\n";
	}

	// Token: 0x06001C8E RID: 7310 RVA: 0x000CC440 File Offset: 0x000CA640
	public void AddTianFuDian(int num)
	{
		this.tianfuDian += num;
		this.tianfuNum.text = this.tianfuDian.ToString();
	}

	// Token: 0x06001C8F RID: 7311 RVA: 0x000CC468 File Offset: 0x000CA668
	private void ShowFinallyPage()
	{
		this.title.text = "经历";
		this.desc.text = "";
		this.shenYuNum.SetActive(false);
		this.nextBtn.SetActive(false);
		this.finallyDesc.text = "\n";
		List<int> list = new List<int>();
		foreach (int item in this.hasSelectList.Keys)
		{
			list.Add(item);
		}
		list.Sort((int x, int y) => x.CompareTo(y));
		foreach (int key in list)
		{
			if (this.hasSelectList[key].page != 1)
			{
				Text text = this.finallyDesc;
				text.text = text.text + jsonData.instance.CreateAvatarJsonData[this.hasSelectList[key].id.ToString()]["Info"].Str + "\n\n";
			}
		}
		Text text2 = this.finallyDesc;
		text2.text += "十六岁那年，你意外捡到了一把满是锈迹的钝剑，无意间唤醒了其中沉睡的老者灵魂。在老者的指引下，长生之途的大门缓缓为你敞开——\n";
		this.finallyPage.SetActive(true);
	}

	// Token: 0x040016DE RID: 5854
	public Dictionary<int, List<int>> hasSelectSeidList = new Dictionary<int, List<int>>();

	// Token: 0x040016DF RID: 5855
	public Dictionary<int, MainUITianFuCell> hasSelectList = new Dictionary<int, MainUITianFuCell>();

	// Token: 0x040016E0 RID: 5856
	public Dictionary<int, List<MainUITianFuCell>> tianFuPageList = new Dictionary<int, List<MainUITianFuCell>>();

	// Token: 0x040016E1 RID: 5857
	[SerializeField]
	private GameObject tianFuCell;

	// Token: 0x040016E2 RID: 5858
	[SerializeField]
	private MainUISetLinGen setLingGen;

	// Token: 0x040016E3 RID: 5859
	[SerializeField]
	private GameObject shenYuNum;

	// Token: 0x040016E4 RID: 5860
	[SerializeField]
	private GameObject finallyPage;

	// Token: 0x040016E5 RID: 5861
	[SerializeField]
	private Transform tianFuList;

	// Token: 0x040016E6 RID: 5862
	[SerializeField]
	private GameObject man;

	// Token: 0x040016E7 RID: 5863
	[SerializeField]
	private GameObject woman;

	// Token: 0x040016E8 RID: 5864
	[SerializeField]
	private Text tianfuNum;

	// Token: 0x040016E9 RID: 5865
	[SerializeField]
	private Text title;

	// Token: 0x040016EA RID: 5866
	[SerializeField]
	private Text desc;

	// Token: 0x040016EB RID: 5867
	[SerializeField]
	private Text finallyDesc;

	// Token: 0x040016EC RID: 5868
	[SerializeField]
	private GameObject nextBtn;

	// Token: 0x040016ED RID: 5869
	private bool isInit;

	// Token: 0x040016EE RID: 5870
	public int tianfuDian;

	// Token: 0x040016EF RID: 5871
	public int curPage = 1;
}

//此文件由UIViewTemplate自动生成，任何手动修改将会被下一次生成覆盖，若需手动修改请避免自动生成
//Author : 原初z

using UnityEngine;
using UnityEngine.UI;
using UIFramework.Core;
using TMPro;

namespace UIFramework.ViewPath
{
	public class LobbyPlayerPanelView : BaseUIPanelView
	{
		[Header("可绑定组件")]
		public Button TeamButton;
		public Transform TeamBackground;
		public InputField RoomInput;
		public Transform RoomIdBackground;
		public Button CreateTeamButton;
		public Button JoinTeamButton;
		public Button LevelTeamButton;
		public Transform TeamMember;
		public Text RoomId;
		public GameObject MemberPrefab;

	}
}

using ProtoBuf;
using System;

using System.Collections.Generic;
using Fantasy;
using Fantasy.Network.Interface;
using Fantasy.Serialize;
#pragma warning disable CS8618

namespace Fantasy
{
	[ProtoContract]
	public partial class RegisterAccountRequest : AMessage, IRequest
	{
		public static RegisterAccountRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<RegisterAccountRequest>();
		}
		public override void Dispose()
		{
			account = default;
			pass = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<RegisterAccountRequest>(this);
#endif
		}
		[ProtoIgnore]
		public RegisterAccountResponse ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.RegisterAccountRequest; }
		[ProtoMember(1)]
		public string account { get; set; }
		[ProtoMember(2)]
		public string pass { get; set; }
	}
	[ProtoContract]
	public partial class RegisterAccountResponse : AMessage, IResponse
	{
		public static RegisterAccountResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<RegisterAccountResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			account = default;
			pass = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<RegisterAccountResponse>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.RegisterAccountResponse; }
		[ProtoMember(1)]
		public string account { get; set; }
		[ProtoMember(2)]
		public string pass { get; set; }
		[ProtoMember(3)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class RegisterNameRequest : AMessage, IRequest
	{
		public static RegisterNameRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<RegisterNameRequest>();
		}
		public override void Dispose()
		{
			accountName = default;
			name = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<RegisterNameRequest>(this);
#endif
		}
		[ProtoIgnore]
		public RegisterNameResponse ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.RegisterNameRequest; }
		[ProtoMember(1)]
		public string accountName { get; set; }
		[ProtoMember(2)]
		public string name { get; set; }
	}
	[ProtoContract]
	public partial class RegisterNameResponse : AMessage, IResponse
	{
		public static RegisterNameResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<RegisterNameResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			accountName = default;
			name = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<RegisterNameResponse>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.RegisterNameResponse; }
		[ProtoMember(1)]
		public string accountName { get; set; }
		[ProtoMember(2)]
		public string name { get; set; }
		[ProtoMember(3)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class EntryLobbyRequest : AMessage, IRequest
	{
		public static EntryLobbyRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<EntryLobbyRequest>();
		}
		public override void Dispose()
		{
			accountId = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<EntryLobbyRequest>(this);
#endif
		}
		[ProtoIgnore]
		public EntryLobbyResponse ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.EntryLobbyRequest; }
		[ProtoMember(1)]
		public long accountId { get; set; }
	}
	[ProtoContract]
	public partial class EntryLobbyResponse : AMessage, IResponse
	{
		public static EntryLobbyResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<EntryLobbyResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			selfData = default;
			otherPlayerData.Clear();
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<EntryLobbyResponse>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.EntryLobbyResponse; }
		[ProtoMember(1)]
		public stateSyncData selfData { get; set; }
		[ProtoMember(2)]
		public List<stateSyncData> otherPlayerData = new List<stateSyncData>();
		[ProtoMember(3)]
		public uint ErrorCode { get; set; }
	}
	/// <summary>
	/// 玩家登录请求
	/// </summary>
	[ProtoContract]
	public partial class LoginRequest : AMessage, IRequest
	{
		public static LoginRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<LoginRequest>();
		}
		public override void Dispose()
		{
			account = default;
			pass = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<LoginRequest>(this);
#endif
		}
		[ProtoIgnore]
		public LoginResponse ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.LoginRequest; }
		[ProtoMember(1)]
		public string account { get; set; }
		[ProtoMember(2)]
		public string pass { get; set; }
	}
	[ProtoContract]
	public partial class LoginResponse : AMessage, IResponse
	{
		public static LoginResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<LoginResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			accountId = default;
			accountName = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<LoginResponse>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.LoginResponse; }
		[ProtoMember(1)]
		public long accountId { get; set; }
		[ProtoMember(2)]
		public string accountName { get; set; }
		[ProtoMember(3)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class OtherPlayerLoginMessage : AMessage, IMessage
	{
		public static OtherPlayerLoginMessage Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<OtherPlayerLoginMessage>();
		}
		public override void Dispose()
		{
			playerData = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<OtherPlayerLoginMessage>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.OtherPlayerLoginMessage; }
		[ProtoMember(1)]
		public stateSyncData playerData { get; set; }
	}
	[ProtoContract]
	public partial class LogoutMessage : AMessage, IMessage
	{
		public static LogoutMessage Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<LogoutMessage>();
		}
		public override void Dispose()
		{
			playerId = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<LogoutMessage>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.LogoutMessage; }
		[ProtoMember(1)]
		public long playerId { get; set; }
	}
	[ProtoContract]
	public partial class OtherPlayerLogoutMessage : AMessage, IMessage
	{
		public static OtherPlayerLogoutMessage Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<OtherPlayerLogoutMessage>();
		}
		public override void Dispose()
		{
			playerId = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<OtherPlayerLogoutMessage>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.OtherPlayerLogoutMessage; }
		[ProtoMember(1)]
		public long playerId { get; set; }
	}
	[ProtoContract]
	public partial class StateSyncRequest : AMessage, IRequest
	{
		public static StateSyncRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<StateSyncRequest>();
		}
		public override void Dispose()
		{
			statePackageId = default;
			stateData = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<StateSyncRequest>(this);
#endif
		}
		[ProtoIgnore]
		public StateSyncResponse ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.StateSyncRequest; }
		[ProtoMember(1)]
		public long statePackageId { get; set; }
		[ProtoMember(2)]
		public stateSyncData stateData { get; set; }
	}
	[ProtoContract]
	public partial class StateSyncResponse : AMessage, IResponse
	{
		public static StateSyncResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<StateSyncResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			statePackageId = default;
			stateData = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<StateSyncResponse>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.StateSyncResponse; }
		[ProtoMember(1)]
		public long statePackageId { get; set; }
		[ProtoMember(2)]
		public stateSyncData stateData { get; set; }
		[ProtoMember(3)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class OtherPlayerStateSyncMessage : AMessage, IMessage
	{
		public static OtherPlayerStateSyncMessage Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<OtherPlayerStateSyncMessage>();
		}
		public override void Dispose()
		{
			roleData = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<OtherPlayerStateSyncMessage>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.OtherPlayerStateSyncMessage; }
		[ProtoMember(1)]
		public stateSyncData roleData { get; set; }
	}
	[ProtoContract]
	public partial class CreateTeamRequest : AMessage, IRequest
	{
		public static CreateTeamRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<CreateTeamRequest>();
		}
		public override void Dispose()
		{
			playerId = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<CreateTeamRequest>(this);
#endif
		}
		[ProtoIgnore]
		public CreateTeamResponse ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.CreateTeamRequest; }
		[ProtoMember(1)]
		public long playerId { get; set; }
	}
	[ProtoContract]
	public partial class CreateTeamResponse : AMessage, IResponse
	{
		public static CreateTeamResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<CreateTeamResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			teamId = default;
			playerId = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<CreateTeamResponse>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.CreateTeamResponse; }
		[ProtoMember(1)]
		public long teamId { get; set; }
		[ProtoMember(2)]
		public long playerId { get; set; }
		[ProtoMember(3)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class JoinTeamRequest : AMessage, IRequest
	{
		public static JoinTeamRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<JoinTeamRequest>();
		}
		public override void Dispose()
		{
			teamId = default;
			playerId = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<JoinTeamRequest>(this);
#endif
		}
		[ProtoIgnore]
		public JoinTeamResponse ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.JoinTeamRequest; }
		[ProtoMember(1)]
		public long teamId { get; set; }
		[ProtoMember(2)]
		public long playerId { get; set; }
	}
	[ProtoContract]
	public partial class JoinTeamResponse : AMessage, IResponse
	{
		public static JoinTeamResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<JoinTeamResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			teamId = default;
			teamOwnerId = default;
			teamMemberIds.Clear();
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<JoinTeamResponse>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.JoinTeamResponse; }
		[ProtoMember(1)]
		public long teamId { get; set; }
		[ProtoMember(2)]
		public long teamOwnerId { get; set; }
		[ProtoMember(3)]
		public List<long> teamMemberIds = new List<long>();
		[ProtoMember(4)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class TeamStateChangeMessage : AMessage, IMessage
	{
		public static TeamStateChangeMessage Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<TeamStateChangeMessage>();
		}
		public override void Dispose()
		{
			teamState = default;
			playerId = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<TeamStateChangeMessage>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.TeamStateChangeMessage; }
		[ProtoMember(1)]
		public int teamState { get; set; }
		[ProtoMember(2)]
		public long playerId { get; set; }
	}
	[ProtoContract]
	public partial class stateSyncData : AMessage
	{
		public static stateSyncData Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<stateSyncData>();
		}
		public override void Dispose()
		{
			playerId = default;
			position = default;
			inputDir = default;
			playerState = default;
			PlayerName = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<stateSyncData>(this);
#endif
		}
		[ProtoMember(1)]
		public long playerId { get; set; }
		[ProtoMember(2)]
		public CSVector3 position { get; set; }
		[ProtoMember(3)]
		public CSVector3 inputDir { get; set; }
		[ProtoMember(4)]
		public int playerState { get; set; }
		[ProtoMember(5)]
		public string PlayerName { get; set; }
	}
	[ProtoContract]
	public partial class CSVector3 : AMessage
	{
		public static CSVector3 Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<CSVector3>();
		}
		public override void Dispose()
		{
			x = default;
			y = default;
			z = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<CSVector3>(this);
#endif
		}
		[ProtoMember(1)]
		public float x { get; set; }
		[ProtoMember(2)]
		public float y { get; set; }
		[ProtoMember(3)]
		public float z { get; set; }
	}
}


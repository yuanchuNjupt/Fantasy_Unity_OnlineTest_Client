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
			selfData = default;
			otherPlayerData.Clear();
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<LoginResponse>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.LoginResponse; }
		[ProtoMember(1)]
		public PlayerData selfData { get; set; }
		[ProtoMember(2)]
		public List<PlayerData> otherPlayerData = new List<PlayerData>();
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
			playerId = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<OtherPlayerLoginMessage>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.OtherPlayerLoginMessage; }
		[ProtoMember(1)]
		public long playerId { get; set; }
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
	public partial class PlayerData : AMessage
	{
		public static PlayerData Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<PlayerData>();
		}
		public override void Dispose()
		{
			playerId = default;
			position = default;
			renderDir = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<PlayerData>(this);
#endif
		}
		[ProtoMember(1)]
		public long playerId { get; set; }
		[ProtoMember(2)]
		public CSVector3 position { get; set; }
		[ProtoMember(3)]
		public CSVector3 renderDir { get; set; }
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


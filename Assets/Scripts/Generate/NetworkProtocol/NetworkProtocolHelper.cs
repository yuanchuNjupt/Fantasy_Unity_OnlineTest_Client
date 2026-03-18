using System.Runtime.CompilerServices;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Fantasy
{
	public static class NetworkProtocolHelper
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<RegisterAccountResponse> RegisterAccountRequest(this Session session, RegisterAccountRequest request)
		{
			return (RegisterAccountResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<RegisterAccountResponse> RegisterAccountRequest(this Session session, string account, string pass)
		{
			using var request = Fantasy.RegisterAccountRequest.Create(session.Scene);
			request.account = account;
			request.pass = pass;
			return (RegisterAccountResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<RegisterNameResponse> RegisterNameRequest(this Session session, RegisterNameRequest request)
		{
			return (RegisterNameResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<RegisterNameResponse> RegisterNameRequest(this Session session, string accountName, string name)
		{
			using var request = Fantasy.RegisterNameRequest.Create(session.Scene);
			request.accountName = accountName;
			request.name = name;
			return (RegisterNameResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<EntryLobbyResponse> EntryLobbyRequest(this Session session, EntryLobbyRequest request)
		{
			return (EntryLobbyResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<EntryLobbyResponse> EntryLobbyRequest(this Session session, long accountId)
		{
			using var request = Fantasy.EntryLobbyRequest.Create(session.Scene);
			request.accountId = accountId;
			return (EntryLobbyResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<LoginResponse> LoginRequest(this Session session, LoginRequest request)
		{
			return (LoginResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<LoginResponse> LoginRequest(this Session session, string account, string pass)
		{
			using var request = Fantasy.LoginRequest.Create(session.Scene);
			request.account = account;
			request.pass = pass;
			return (LoginResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void OtherPlayerLoginMessage(this Session session, OtherPlayerLoginMessage message)
		{
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void OtherPlayerLoginMessage(this Session session, StateSyncData playerData)
		{
			using var message = Fantasy.OtherPlayerLoginMessage.Create(session.Scene);
			message.playerData = playerData;
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void LogoutMessage(this Session session, LogoutMessage message)
		{
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void LogoutMessage(this Session session, long playerId)
		{
			using var message = Fantasy.LogoutMessage.Create(session.Scene);
			message.playerId = playerId;
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void OtherPlayerLogoutMessage(this Session session, OtherPlayerLogoutMessage message)
		{
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void OtherPlayerLogoutMessage(this Session session, long playerId)
		{
			using var message = Fantasy.OtherPlayerLogoutMessage.Create(session.Scene);
			message.playerId = playerId;
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<StateSyncResponse> StateSyncRequest(this Session session, StateSyncRequest request)
		{
			return (StateSyncResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<StateSyncResponse> StateSyncRequest(this Session session, long tatePackageId, StateSyncData stateData)
		{
			using var request = Fantasy.StateSyncRequest.Create(session.Scene);
			request.tatePackageId = tatePackageId;
			request.stateData = stateData;
			return (StateSyncResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void OtherPlayerStateSyncMessage(this Session session, OtherPlayerStateSyncMessage message)
		{
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void OtherPlayerStateSyncMessage(this Session session, StateSyncData roleData)
		{
			using var message = Fantasy.OtherPlayerStateSyncMessage.Create(session.Scene);
			message.roleData = roleData;
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<CreateTeamResponse> CreateTeamRequest(this Session session, CreateTeamRequest request)
		{
			return (CreateTeamResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<CreateTeamResponse> CreateTeamRequest(this Session session, long playerId)
		{
			using var request = Fantasy.CreateTeamRequest.Create(session.Scene);
			request.playerId = playerId;
			return (CreateTeamResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<JoinTeamResponse> JoinTeamRequest(this Session session, JoinTeamRequest request)
		{
			return (JoinTeamResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async FTask<JoinTeamResponse> JoinTeamRequest(this Session session, long teamId, long playerId)
		{
			using var request = Fantasy.JoinTeamRequest.Create(session.Scene);
			request.teamId = teamId;
			request.playerId = playerId;
			return (JoinTeamResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TeamStateChangeMessage(this Session session, TeamStateChangeMessage message)
		{
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TeamStateChangeMessage(this Session session, int teamState, long playerId)
		{
			using var message = Fantasy.TeamStateChangeMessage.Create(session.Scene);
			message.teamState = teamState;
			message.playerId = playerId;
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EnterDungeonMessage(this Session session, EnterDungeonMessage message)
		{
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EnterDungeonMessage(this Session session, long teamId, List<long> teamMemberIds)
		{
			using var message = Fantasy.EnterDungeonMessage.Create(session.Scene);
			message.teamId = teamId;
			message.teamMemberIds = teamMemberIds;
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void LoadDungeonProgressMessage(this Session session, LoadDungeonProgressMessage message)
		{
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void LoadDungeonProgressMessage(this Session session, long teamId, long playerId, float progress)
		{
			using var message = Fantasy.LoadDungeonProgressMessage.Create(session.Scene);
			message.teamId = teamId;
			message.playerId = playerId;
			message.progress = progress;
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void StartDungeonBattleMessage(this Session session, StartDungeonBattleMessage message)
		{
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void StartDungeonBattleMessage(this Session session, List<BattlePlayerData> battlePlayers)
		{
			using var message = Fantasy.StartDungeonBattleMessage.Create(session.Scene);
			message.battlePlayers = battlePlayers;
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FrameOperateEventMessage_C2G(this Session session, FrameOperateEventMessage_C2G message)
		{
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FrameOperateEventMessage_C2G(this Session session, long battleId, FrameOperationData frameOperateDataList)
		{
			using var message = Fantasy.FrameOperateEventMessage_C2G.Create(session.Scene);
			message.battleId = battleId;
			message.frameOperateDataList = frameOperateDataList;
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FrameOperateEventMessage_G2C(this Session session, FrameOperateEventMessage_G2C message)
		{
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FrameOperateEventMessage_G2C(this Session session, long battleId, List<FrameOperationData> frameOperateDataList, long logicFrameId)
		{
			using var message = Fantasy.FrameOperateEventMessage_G2C.Create(session.Scene);
			message.battleId = battleId;
			message.frameOperateDataList = frameOperateDataList;
			message.logicFrameId = logicFrameId;
			session.Send(message);
		}

	}
}

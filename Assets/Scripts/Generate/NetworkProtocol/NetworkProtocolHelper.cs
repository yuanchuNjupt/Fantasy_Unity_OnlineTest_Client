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
		public static void OtherPlayerLoginMessage(this Session session, long playerId)
		{
			using var message = Fantasy.OtherPlayerLoginMessage.Create(session.Scene);
			message.playerId = playerId;
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
		public static async FTask<StateSyncResponse> StateSyncRequest(this Session session, long statePackageId, stateSyncData stateData)
		{
			using var request = Fantasy.StateSyncRequest.Create(session.Scene);
			request.statePackageId = statePackageId;
			request.stateData = stateData;
			return (StateSyncResponse)await session.Call(request);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void OtherPlayerStateSyncMessage(this Session session, OtherPlayerStateSyncMessage message)
		{
			session.Send(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void OtherPlayerStateSyncMessage(this Session session, stateSyncData roleData)
		{
			using var message = Fantasy.OtherPlayerStateSyncMessage.Create(session.Scene);
			message.roleData = roleData;
			session.Send(message);
		}

	}
}

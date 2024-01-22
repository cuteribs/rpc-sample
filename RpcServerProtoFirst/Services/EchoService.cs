using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RpcServer.Contracts;

namespace RpcServer.Services;

public class EchoService : Echo.EchoBase
{
	public override Task<Empty> Hello(Empty request, ServerCallContext context)
		=> Task.FromResult(new Empty());

	public override Task<PongResponse> Ping(PingRequest request, ServerCallContext context)
		=> Task.FromResult(new PongResponse
		{
			ClientInfo = new()
			{
				Ip = request.ClientInfo.Ip,
				Mac = request.ClientInfo.Mac
			},
			ServerInfo = new()
			{
				Ip = "1.1.1.1",
				Mac = "00:00:00:00:00:00"
			},
			Latency = Timestamp.FromDateTime(DateTime.UtcNow)
		});
}

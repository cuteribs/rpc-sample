using ProtoBuf.Grpc;
using RpcServer.Contracts;

namespace RpcServer.Services;

public class EchoService : IEchoService
{
	public ValueTask Hello(CallContext? callContext = null)
		=> new();

	public ValueTask<PongResponse> Ping(PingRequest request, CallContext? callContext = null)
		=> new(new PongResponse
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
			Latency = DateTime.Now
		});
}

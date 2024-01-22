using ProtoBuf.Grpc;
using RpcServer.Contracts;

namespace RpcServer.Services;

public class EchoService : IEchoService
{
	public Task Hello()
	{
		Console.WriteLine("hello");
		return Task.CompletedTask;
	}

	public Task Hello(CallContext callContext)
	{
		Console.WriteLine("hello call");
		return Task.CompletedTask;
	}

	public Task<PongResponse> Ping(PingRequest request)
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
			Latency = DateTime.UtcNow
		});
}

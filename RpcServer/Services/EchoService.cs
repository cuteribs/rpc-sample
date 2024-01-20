using RpcServer.Contracts;

namespace RpcServer.Services;

public class EchoService : IEchoService
{
	public ValueTask<PongResponse> Hello()
		=> ValueTask.FromResult(new PongResponse
		{
			ServerIp = "1.1.1.1",
			ServerMac = "00:00:00:00:00:00",
			Latency = 100
		});

	public ValueTask<PongResponse> Ping(PingRequest request)
		=> ValueTask.FromResult(new PongResponse
		{
			ClientId = request.ClientId,
			ClientMac = request.ClientMac,
			ServerIp = "1.1.1.1",
			ServerMac = "00:00:00:00:00:00",
			Latency = 100
		});
}

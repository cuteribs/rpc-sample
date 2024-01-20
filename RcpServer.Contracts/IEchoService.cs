using System.Runtime.Serialization;
using System.ServiceModel;

namespace RpcServer.Contracts;

[ServiceContract]
public interface IEchoService : IServiceContract
{
	ValueTask<PongResponse> Ping(PingRequest request);

	ValueTask<PongResponse> Hello();
}

[DataContract]
public class PingRequest
{
	[DataMember(Order = 1)]
	public string ClientId { get; set; } = default!;
	[DataMember(Order = 2)]
	public string ClientMac { get; set; } = default!;
}

[DataContract]
public class PongResponse
{
	[DataMember(Order = 1)]
	public string ClientId { get; set; } = default!;
	[DataMember(Order = 2)]
	public string ClientMac { get; set; } = default!;
	[DataMember(Order = 3)]
	public string ServerIp { get; set; } = default!;
	[DataMember(Order = 4)]
	public string ServerMac { get; set; } = default!;
	[DataMember(Order = 5)]
	public string SourceIp { get; set; } = default!;
	[DataMember(Order = 6)]
	public int Latency { get; set; }
}
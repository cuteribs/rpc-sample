using System.Runtime.Serialization;
using System.ServiceModel;

namespace RpcServer.Contracts;

[ServiceContract]
public interface IEchoService : IGrpcContract
{
	[OperationContract]
	Task<PongResponse> Ping(PingRequest request);

	[OperationContract]
	Task Hello();
}

[DataContract]
public class PingRequest
{
	[DataMember(Order = 1)]
	public NetworkInfo ClientInfo { get; set; } = default!;
}

[DataContract]
public class PongResponse
{
	[DataMember(Order = 1)]
	public NetworkInfo ClientInfo { get; set; } = default!;
	[DataMember(Order = 2)]
	public NetworkInfo ServerInfo { get; set; } = default!;
	[DataMember(Order = 3)]
	public DateTime Latency { get; set; }
}

[DataContract]
public class NetworkInfo
{
	[DataMember(Order = 1)]
	public string Ip { get; set; } = default!;
	[DataMember(Order = 2)]
	public string Mac { get; set; } = default!;
	[DataMember(Order = 3)]
	public IpType IpType { get; set; }
}

[DataContract]
public enum IpType
{
	IPv4 =3,
	IPv6
}
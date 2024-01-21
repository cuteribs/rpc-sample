using ProtoBuf;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;
using System.ServiceModel;

namespace RpcServer.Contracts;

[Service]
public interface IEchoService : IGrpcContract
{
	[OperationContract]
	ValueTask<PongResponse> Ping(PingRequest request, CallContext? callContext = null);

	[OperationContract]
	ValueTask Hello(CallContext? callContext = null);
}

[ProtoContract]
public class PingRequest
{
	[ProtoMember(1)]
	public NetworkInfo ClientInfo { get; set; } = default!;
}

[ProtoContract]
public class PongResponse
{
	[ProtoMember(1)]
	public NetworkInfo ClientInfo { get; set; } = default!;
	[ProtoMember(2)]
	public NetworkInfo ServerInfo { get; set; } = default!;
	[ProtoMember(3)]
	public DateTime Latency { get; set; }
}

[ProtoContract]
public class NetworkInfo
{
	[ProtoMember(1)]
	public string Ip { get; set; } = default!;
	[ProtoMember(2)]
	public string Mac { get; set; } = default!;
	[ProtoMember(3)]
	public IpType IpType { get; set; }
}

[ProtoContract]
public enum IpType
{
	IPv4 =3,
	IPv6
}
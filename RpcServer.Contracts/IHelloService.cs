using ProtoBuf;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace RpcServer.Contracts;

[Service]
public interface IHelloService : IGrpcContract
{
	ValueTask<HelloResponse> Hello(HelloRequest request, CallContext? callContext = null);
}

[ProtoContract]
public class HelloRequest
{
	[ProtoMember(1)]
	public string Name { get; set; } = default!;
}

[ProtoContract]
public class HelloResponse
{
	[ProtoMember(1)]
	public IEnumerable<string> Message { get; set; } = default!;
}
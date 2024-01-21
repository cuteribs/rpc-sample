using ProtoBuf.Grpc;
using RpcServer.Contracts;

namespace RpcServer.Services;

public class HelloService : IHelloService
{
	public ValueTask<HelloResponse> Hello(HelloRequest request, CallContext? callContext = null)
		=> default;
}

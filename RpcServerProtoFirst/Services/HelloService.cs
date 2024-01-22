using Grpc.Core;
using RpcServer.Contracts;

namespace RpcServer.Services;

public class HelloService : Hello.HelloBase
{
	public override Task<HelloResponse> Hello(HelloRequest request, ServerCallContext context)
		=> Task.FromResult(new HelloResponse());
}

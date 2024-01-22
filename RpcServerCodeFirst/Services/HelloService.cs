using RpcServer.Contracts;

namespace RpcServer.Services;

public class HelloService : IHelloService
{
	public Task<HelloResponse> Hello(HelloRequest request)
		=> Task.FromResult(new HelloResponse());
}

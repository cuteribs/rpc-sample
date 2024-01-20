using RpcServer.Contracts;

namespace RpcServer.Services;

public class HelloService : IHelloService
{
	public ValueTask<HelloResponse> Hello(HelloRequest request)
		=> ValueTask.FromResult(new HelloResponse { Message = $"Hello {request.Name}!" });
}

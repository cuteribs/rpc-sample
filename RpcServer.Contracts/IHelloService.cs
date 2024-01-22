using System.Runtime.Serialization;
using System.ServiceModel;

namespace RpcServer.Contracts;

[ServiceContract]
public interface IHelloService : IGrpcContract
{
	[OperationContract]
	Task<HelloResponse> Hello(HelloRequest request);
}

[DataContract]
public class HelloRequest
{
	[DataMember(Order = 1)]
	public string Name { get; set; } = default!;
}

[DataContract]
public class HelloResponse
{
	[DataMember(Order = 1)]
	public IEnumerable<string> Message { get; set; } = default!;
}
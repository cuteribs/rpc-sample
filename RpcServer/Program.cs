using ProtoBuf.Grpc.Reflection;
using ProtoBuf.Grpc.Server;
using RpcServer.Contracts;
using RpcServer.Services;

namespace RpcServer;

public class Program
{
	public static void Main(string[] args)
	{
		GenerateProtoFile();

		StartWebServer(args);
	}

	/// <summary>
	/// Generates the proto files for sharing to gRPC clients.
	/// </summary>
	static void GenerateProtoFile()
	{
		var generator = new SchemaGenerator();
		File.WriteAllText("echo.proto", generator.GetSchema<IEchoService>());
		File.WriteAllText("hello.proto", generator.GetSchema<IHelloService>());
	}

	static void StartWebServer(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		var services = builder.Services;

		// enable code first gRPC services
		services.AddCodeFirstGrpc(o => o.EnableDetailedErrors = true);

		// optional - enable service reflection
		services.AddCodeFirstGrpcReflection();

		services.AddScoped<IEchoService, EchoService>();
		services.AddScoped<IHelloService, HelloService>();

		var app = builder.Build();

		// map gRPC services
		app.MapGrpcService<IEchoService>();
		app.MapGrpcService<IHelloService>();

		// optional - map gRPC service reflection
		app.MapCodeFirstGrpcReflectionService();

		// map service contracts as Web API endpoints (auto-generated with T4 text template)
		app.MapPostServiceContracts();

		app.MapGet("/", () => "Hello World!");

		app.Run();
	}
}

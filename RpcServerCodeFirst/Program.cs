using Microsoft.AspNetCore.Http.Json;
using ProtoBuf.Grpc.Reflection;
using ProtoBuf.Grpc.Server;
using RpcServer.Contracts;
using RpcServer.Services;
using System.Text.Json;

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
		File.WriteAllText("Protos/echo.proto", generator.GetSchema<IEchoService>());
		File.WriteAllText("Protos/hello.proto", generator.GetSchema<IHelloService>());
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

		services.Configure<JsonOptions>(o => o.SerializerOptions.PropertyNamingPolicy = null);
		services.AddEndpointsApiExplorer()
			.AddSwaggerGen();

		var app = builder.Build();

		app.UseSwagger()
			.UseSwaggerUI();

		// map gRPC services
		app.MapGrpcService<EchoService>();
		app.MapGrpcService<HelloService>();


		// optional - map gRPC service reflection
		app.MapCodeFirstGrpcReflectionService();

		// map service contracts as Web API endpoints (auto-generated with T4 text template)
		app.MapPostServiceContracts();

		app.MapGet("/", () => "Hello World!");

		app.Run();
	}
}

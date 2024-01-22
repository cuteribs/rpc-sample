using Microsoft.AspNetCore.Mvc;
using RpcServer.Contracts;

namespace RpcServer;

public static class WebApplicationExtensions
{
	public static WebApplication MapPostServiceContracts(this WebApplication app)
	{
		app.MapPost(
			"/EchoService/Ping",
			(
				[FromBody] PingRequest request,
				[FromServices] IEchoService service,
				HttpContext context
			) => service.Ping(request)
		);

		app.MapPost(
			"/EchoService/Hello",
			(
				[FromServices] IEchoService service,
				HttpContext context
			) => service.Hello()
		);

		app.MapPost(
			"/HelloService/Hello",
			(
				[FromBody] HelloRequest request,
				[FromServices] IHelloService service,
				HttpContext context
			) => service.Hello(request)
		);

		return app;
	}
}
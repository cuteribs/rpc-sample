using Grpc.Core;

namespace RpcServer
{
	public class HttpServerCallContext : ServerCallContext
	{
		public HttpContext HttpContext { get; }

		public HttpServerCallContext(HttpContext httpContext)
		{
			this.HttpContext = httpContext;
		}

		protected override string MethodCore => this.HttpContext.Request.Path.Value!;

		protected override string HostCore => this.HttpContext.Request.Host.Value;

		protected override string PeerCore => throw new NotImplementedException();

		protected override DateTime DeadlineCore => throw new NotImplementedException();

		protected override Metadata RequestHeadersCore => ToMetadata(this.HttpContext.Request.Headers);

		protected override CancellationToken CancellationTokenCore => this.HttpContext.RequestAborted;

		protected override Metadata ResponseTrailersCore => throw new NotImplementedException();

		protected override Status StatusCore { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		protected override WriteOptions? WriteOptionsCore { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		protected override AuthContext AuthContextCore => throw new NotImplementedException();

		protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions? options)
		 => throw new NotImplementedException();

		protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
		 => throw new NotImplementedException();

		private static Metadata ToMetadata(IHeaderDictionary headers)
		{
			var metadata = new Metadata();

			foreach (var header in headers)
			{
				metadata.Add(header.Key, header.Value);
			}

			return metadata;
		}
	}
}

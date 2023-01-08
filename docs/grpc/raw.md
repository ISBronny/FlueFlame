# Low-level access to RPC Call

You may not have enough ready-made functions for testing in FlueFlame. In this case, you can directly access objects such as `IClientStreamWriter` and `IAsyncStreamReader`:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.Bidirectional
		.Call(c=>c.GetByIds())
		.RequestStream
			.WithStreamWriter(async writer =>
			{
				await writer.WriteAsync(new StringValue() { Value = Guid.NewGuid().ToString()});
				await writer.CompleteAsync();
			})
		.ResponseStream
			.WithStreamReader(async reader =>
			{
				await FluentActions.Awaiting(async () => await reader.MoveNext())
					.Should().ThrowAsync<RpcException>();
			});
```
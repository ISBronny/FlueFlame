namespace FlueFlame.Core.Builders;

public class FlueFlameHostBuilder<TBuilder> where TBuilder : FlueFlameHostBuilder<TBuilder>
{
	protected HttpClient HttpClient { get; set; } = new();
	
	public TBuilder UseCustomHttpClient(HttpClient httpClient)
	{
		HttpClient = httpClient;
		return (TBuilder)this;
	}
	
	public TBuilder ConfigureHttpClient(Action<HttpClient> configure)
	{
		configure(HttpClient);
		return (TBuilder)this;
	}
}
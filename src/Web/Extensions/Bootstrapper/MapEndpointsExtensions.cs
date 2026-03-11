using API.Endpoints;

namespace API.Extensions.Bootstrapper;

public static class MapEndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapMusicEndpoints();
        app.MapDownloadEndpoints();

        return app;
    }
}
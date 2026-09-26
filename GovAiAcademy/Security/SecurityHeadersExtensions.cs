namespace GovAiAcademy.Security;

public static class SecurityHeadersExtensions
{
    public static IApplicationBuilder UseAcademySecurityHeaders(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            var headers = context.Response.Headers;
            headers["X-Content-Type-Options"] = "nosniff";
            headers["X-Frame-Options"] = "DENY";
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
            headers["X-Permitted-Cross-Domain-Policies"] = "none";
            // Blazor Server needs inline boot styles and the circuit's script host.
            // Tighten script-src further only after a production circuit test.
            headers["Content-Security-Policy"] =
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
                "style-src 'self' 'unsafe-inline'; " +
                "img-src 'self' data:; " +
                "font-src 'self' data:; " +
                "connect-src 'self' ws: wss:; " +
                "frame-ancestors 'none'; " +
                "base-uri 'self'; " +
                "object-src 'none'; " +
                "form-action 'self'";
            await next();
        });
        return app;
    }
}

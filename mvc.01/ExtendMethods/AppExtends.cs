using System.Net;
using System.Runtime.CompilerServices;

namespace mvc._01.ExtendMethods
{
    public static class AppExtends
    {
        public static void AddStatusCodePage(this IApplicationBuilder app)
        {
            app.UseStatusCodePages(appError =>
            {
                appError.Run(async context =>
                {
                    var respone = context.Response;
                    var code = respone.StatusCode;

                    var content = $@"<html>
                        <head>
                        <meta charset = 'utf-8' />
                        <title>Loi {code}</title></head>
                        <body>
                            <p style='color:red;font-size:30px;'>
                                Co lo xay ra: {code} - {(HttpStatusCode)code}
                            </p>
                        </body>
                        </html>";

                    await respone.WriteAsync(content);
                });
            });
        }
    }
}

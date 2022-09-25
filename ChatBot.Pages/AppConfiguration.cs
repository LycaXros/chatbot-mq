using ChatBot.Pages.Hubs;

namespace ChatBot.Pages
{
    public static class AppConfiguration
    {
        public static WebApplication ConfigureApp(this WebApplication app)
        {


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapBlazorHub();
            app.MapHub<ChatHub>("/chatHub");
            app.MapFallbackToPage("/_Host");

            return app;
        }
    }
}

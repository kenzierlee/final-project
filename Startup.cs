using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using keepr.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace keepr
{
    public class Startup
    {
        private readonly string _connectionString = "";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _connectionString = configuration.GetSection("DB").GetValue<string>("mySQLConnectionString");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(o =>
            {
                //registers where (what path) you have the users login at.
                o.LoginPath = "/Account/Login/";
                //context is the node equivalent of "res" the request of the user.
                o.Events.OnRedirectToLogin = (context) =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
            //if you get a request that isnt a path on the server, CORS recognizes that 
            //and decides what do with the request/weither or not to accept/allow it.
            services.AddCors(o =>{
                //utilizes CORS when the current environment is in "Development" mode.
                o.AddPolicy("CORS_ENV_DEVELOPMENT", builder =>{
                    //the different options that CORS will allow
                    //"Origin" refers to the websites CORS will allow to use the server
                    //"Method" refers to our "get", "put", "post", "delete".
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });
            services.AddMvc();
            //transiate service of the connection, which can be passed from class to class
            services.AddTransient<IDbConnection>(x=> CreateDBContext());
            //allows for the burgerrepo to actually be used, only uses the repo when
            //you actually need it so you dont, run it constantly.
            services.AddTransient<UserRepository>();
            services.AddTransient<BoardRepository>();
            services.AddTransient<PinRepository>();
        }
        private IDbConnection CreateDBContext()
        {
            //creating the MySQL connection, opens up a new MySQL connection. 
            //MySqlClient takes care of the connection, sets it up and makes sure it works.
            var connection = new MySqlConnection(_connectionString);
            //opens the connection and then returns it
            connection.Open();
            return connection;
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("CORS_ENV_DEVELOPMENT");
            }
            //gives high priority to the index.html files and loads them prior to entire page loading.
            app.UseDefaultFiles();
            //Static Files are inside of the "wwwwroot" folder. 
            //nodejs equivalent to: server.use(express.static(__dirname + "/dist"));
            app.UseStaticFiles();
            //uses the authentication service that was built in ConfigureServices "AddAuthentication" method.
            app.UseAuthentication();
            //this while kick off built in code 
            app.UseMvc();
        }
    }
}

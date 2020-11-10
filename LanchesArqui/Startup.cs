using LanchesArqui.Areas.Admin.Servicos;
using LanchesArqui.Context;
using LanchesArqui.Models;
using LanchesArqui.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ReflectionIT.Mvc.Paging;

namespace LanchesArqui
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) // injeção de dependencia
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //options => options.SignIn.RequireConfirmedAccount = true
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;

            })
              .AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Home/AccessDenied");


            services.AddTransient<ICategoriaRepository, CategoriaRepository>(); //toda vez que é requisitado cria um objeto
            services.AddTransient<ILancheRepository, LancheRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();

            services.AddScoped<RelatorioVendasService>();

            //services.AddSingleton<IHttpContextAccessor, IHttpContextAccessor>(); //singleton: o objeto criado será utilziado para todas requisições
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //resolve erro do serviço acima

            services.AddScoped(cp => CarrinhoCompra.GetCarrinho(cp));// mesmo criando o objeto, caso crie mais de um, serão de instancias diferentes

            services.AddControllersWithViews();

            services.AddPaging(options =>
            {
                options.ViewName = "Bootstrap4";
                options.PageParameterName = "pageindex";
            });

            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles(); // ativa o uso de arquivos staticos
            app.UseSession();   // ativa midleware do uso da session 
            app.UseAuthentication(); //add a autenticação

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "AdminArea",
                    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "categoriaFiltro",
                    pattern: "Lanche/{action}/{categoria?}",
                    defaults: new { Controller = "Lanche", action = "List" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.DTO;
using ProjetBrima.Models;
using ProjetBrima.Services;

var builder = WebApplication.CreateBuilder(args);
// ?? Configuration du DbContext avec MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection"))
    ));


// Add services to the container.
builder.Services.AddControllersWithViews();
// Configurer la session
builder.Services.AddDistributedMemoryCache(); // Cache pour la session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Durée d'inactivité
    options.Cookie.HttpOnly = true;  // Cookie seulement accessible par le serveur
});
// Configurer l'authentification par cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/contact";  // Rediriger vers la page de connexion si non authentifié
        options.AccessDeniedPath = "/Home/AccessDenied"; // Rediriger vers la page d'accès refusé
        options.ReturnUrlParameter = "ReturnUrl"; // Paramètre pour stocker l'URL de retour
    });

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDirecteurService, DirecteurService>();
builder.Services.AddScoped<IProfesseurService, ProfesseurService>();
builder.Services.AddScoped<IComptesPersonnelService, ComptesPersonnelService>();
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddScoped<IEleveService, EleveService>();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IMatiereService, MatiereService>();
builder.Services.AddScoped<ISalleService, SalleService>();
builder.Services.AddScoped<IEmploiService, EmploiService>();
builder.Services.AddScoped<IAbsenceService, AbsenceService>();
builder.Services.AddScoped<IDemandeCongeService, DemandeCongeService>();
builder.Services.AddScoped<INoteService, NoteService>();





var app = builder.Build();


// ?? Créer un compte admin au démarrage
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate(); // Appliquer les migrations

    // Vérifier s’il existe déjà un admin
    if (!context.Users.Any(u => u.Role == "Admin"))
    {
        var admin = new User
        {
            FirstName = "Admin",
            LastName = "Principal",
            Email = "admin@ecole.com",
            Password = HashPassword("Admin123!"), // Tu peux aussi mettre le mot de passe brut si pas encore de hash
            Role = "Admin",
            IsEmailVerified = true,
            CreatedAt = DateTime.UtcNow,
            Country = "Tunisie",
            State = "Médenine",
            Address = "123 rue principale",
            PhoneNumber = "0600000000"
        };

        context.Users.Add(admin);
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
// ?? Méthode pour hasher le mot de passe
string HashPassword(string password)
{
    var hasher = new PasswordHasher<User>();
    return hasher.HashPassword(null, password);
}

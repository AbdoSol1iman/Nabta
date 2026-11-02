
using Infrastructure;
using OtpNet;
using QRCoder;

var builder = WebApplication.CreateBuilder(args);

// Register services BEFORE Build
builder.Services.AddControllers();

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(
        typeof(Nabta.Application.Featuers.Authentication.LogIn.LogInCommand).Assembly));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Temporary OTP implementation for testing only
byte[] secretKey = KeyGeneration.GenerateRandomKey();
string base32SecretKey = Base32Encoding.ToString(secretKey);

const string issuer = "Nabta";
const string user = "NabtaUser@gmail.com";

app.MapGet("/generate-otp", () =>
{
    string escapedIssuer = Uri.EscapeDataString(issuer);
    string escapedUser = Uri.EscapeDataString(user);

    var otpUri =
        $"otpauth://totp/{escapedIssuer}:{escapedUser}" +
        $"?secret={base32SecretKey}&issuer={escapedIssuer}" +
        "&algorithm=SHA1&digits=6&period=35";

    using var qrGenerator = new QRCodeGenerator();

    using var qrCodeData = qrGenerator.CreateQrCode(
        otpUri,
        QRCodeGenerator.ECCLevel.Q);

    using var qrCode = new PngByteQRCode(qrCodeData);

    byte[] qrCodeBytes = qrCode.GetGraphic(10);

    return Results.File(qrCodeBytes, "image/png");
});

app.MapPost("/otp/validate", (ValidateOtpRequest request) =>
{
    var totp = new Totp(secretKey, step: 35);

    bool isValid = totp.VerifyTotp(
        request.Code,
        out var timeStepMatched,
        VerificationWindow.RfcSpecifiedNetworkDelay);

    return Results.Ok(new { isValid });
});

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nabta API V1");
    c.RoutePrefix = "";
    c.ConfigObject.AdditionalItems["syntaxHighlight"] = false;
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Urls.Add("http://0.0.0.0:5000");

app.Run();

public record ValidateOtpRequest(string Code);

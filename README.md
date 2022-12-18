# ViThor.Auth

A small package that adds Authentication and Authorization layer, based on Json Web Token (JWT), in your dotnet application.

Ready-made endpoints are available for:
- User Registration
- Login (get token)
- Token Validation
- Refresh Token

An email submission and validation service is also now available in this library.


## Installation

Install the package from NuGet:

```bash
dotnet add package ViThor.Auth
```


## Usage

### Authentication

It is necessary to create a class that inherits from UserBase:

```csharp
public class User : UserBase
{    
}
```

Next, you need to create a class that implements the IUserService interface. Add to dependency injection from the `Program.cs` class:
    
```csharp
builder.Services.AddScoped<IUserService<User>, UserService>();
```

The remaining settings and services are built by running:

```csharp
builder.BuildViThorSettings();
```

The parameterization of these services is done in appsettings:

```json
{
    "JwtConfig": {
        "Secret": "This is my custom Secret key for authnetication",
        "Issuer": "http://localhost:5000",
        "Audience": "http://localhost:5000",
        "ExpirationType": "Minutes",
        "Expiration": 1,
        "ValidateIssuerSigningKey": true,
        "ValidateLifetime": true,
        "ValidateIssuer": false,
        "ValidateAudience": false,
        "RequireHttpsMetadata": false,
        "SaveToken": false
    }
}
```

### Email

The email service configuration must also be done in appsettings:

```json
{
    "BaseAddress": "http://localhost:5000",
    "SmtpConfig": {
        "Enabled": false,
        "Host": "smtp.gmail.com",
        "Port": 587,
        "UserName": "user@gmail.com",
        "Password": "password-here",
        "From": "user@gmail.com"
    }
}
```

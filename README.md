# ViThor Auth

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

Next, you need to create a class that implements the IUserService interface.
You can get a sample implementation in `Samples/Vithor.Auth.Sample/Serives/UserService.cs`.

Add the class that implements the IUserService to the `Program.cs` dependency injection service:
    
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

Add AuthController to the API:

```csharp
public class AuthController : AuthControllerBase<User>
{
    public AuthController(IJwtService jwtServices, IEmailService emailService, IUserService<User> userService, IOptions<ViThorAuthSettings> appSettings) : base(jwtServices, emailService, userService, appSettings)
    {
    }
}
```

### E-mail

E-mail configuration is not required. That is, only if you are going to use the email sending service to validate the user registration, add the parameters below in appsettings:

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


## Sample

The sample project is available in the repository. It is a simple dotnet web api project that uses the ViThor.Auth package.


### Running the sample

Run the following command to run the sample API:

```bash
dotnet run --project ./Samples/Vithor.Auth.Sample/ViThor.Auth.Sample.csproj
```

### Testing the sample

#### Swagger

The sample project has Swagger configured. To access it, run the API and access the following URL:

```bash
http://localhost:5000/swagger/index.html
```

#### Postman

The example project has a Postman collection that can be used to test endpoints. To use it, import the collections from the `Postman` folder into Postman and run the requests.



## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.


## License

[MIT](https://choosealicense.com/licenses/mit/)
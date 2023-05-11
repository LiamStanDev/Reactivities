# Objective

- 能使使用者有登入功能，並且在登入之後才能取得 ActivityController 裡面的方法

## 01 - Identity.EntityFrameworeCore

- 我們可以使用這個框架來建立使用者，可以很方便建立管於使用者相關的資料庫與使用者操作，並且因為經過數千次 Hashing
  對於密碼的保護比較完善。

### 實際操作 Part 1

- 使用框架：Microsoft.AspNetCore.Identity.EntityFrameworeCore，需要在 Domain 引入

1. 建立 User class 在本例中是使用 AppUser，必須繼承 IdentityUser，裡面已經包含很多與使用者有關的屬性 e.g.
   UserName, Email, PhoneNumber，也可以在 User class 中添業務需求額外的屬性。
2. 將 DataContext 類改為繼承 IdentityDbContext，不需要另外寫 DbSet<User> 因為要使用 User 可以使用 UserManager
3. 在 API 層中添加新的 Extensions 為了在 DI 容器添加 Identity 服務

```C#
builser.Services.AddIdentityCore<User>(opt =>
    {
        // 添加一些User建立的限制
    }).AddEntityFrameworkStores<DataContext>(); // 將資料上下文添加到裡面
```

```bash
# 建立新的Migrations
dotnet ef migrations add IdentityAdded -p Persistence -s API
```

- Note: 關於 User 的屬性可以看 migrations 裡面的內容，這邊主要會建立一個權限驗證的資料庫，但與 JWT 還沒有關係。
- UserManager 的功能

  1. 使用者的建立和註冊：UserManager 提供了創建新使用者帳戶的方法，可以設定使用者名稱、密碼、電子郵件地址等屬性，並將新使用者儲存到資料庫中。

  2. 使用者的驗證和登入：UserManager 可以進行使用者的驗證，驗證使用者名稱和密碼是否正確。它還提供了登入和登出使用者的方法，以及生成和驗證身份驗證的令牌。

  3. 使用者的角色管理：UserManager 允許將使用者分配到不同的角色中。它提供了添加使用者到角色、從角色中刪除使用者以及檢查使用者是否屬於特定角色的方法。

  4. 使用者密碼的管理：UserManager 提供了修改使用者密碼、重置密碼、確認密碼重置令牌等方法，用於管理使用者的密碼。

  5. 使用者的屬性管理：UserManager 允許管理使用者的其他屬性，例如電子郵件地址、手機號碼、安全問題等。

  6. 使用者的尋找和查詢：UserManager 提供了根據不同條件（例如使用者 ID、使用者名稱、電子郵件地址等）查詢和尋找使用者的方法。

## 02 - 什麼是 Dto ?

- DTO(Data Transmit Object): 與 Domain 不同，Domain 是整個 App 的核心，也是映射到數據庫的模型，裡面包含最多資訊，但在前端
  或者程式與程式之間進行溝通時，傳遞的數據可能需要關聯多的 Domain 或者不需要這麼多資訊，所以我們會建立 DTO 用來在程式間進行溝通，
  它只用來進行溝通，不涉及與資料庫的關係。
- 權限驗證為一個特殊的部份，我們不希望交給 Application(Use Case) 進行，我們希望他是在 API 進行權限驗證所以，有關權限驗證，
  都會在 API 裡面完成。

### 實際操作 Part 2

1. 建立一個 DTOs 資料夾，裡面放入 LoginDto(前端登入時會傳入的驗證信息), RegisterDto(前端註冊時會傳入的建立信息)和 UserDto
   (前端請求使用者時要求傳出的信息，包含 Token)
2. 建立一個 AccountController 統一管理 Identity，這邊為了不要混雜到其他 Controller，我們並不使用 BaseController 作為基底，
   反而直接繼承 ControllerBase，我們需要給這個 Controller 添加一些基本的方法 e.g. Login, Register, GetCurrentUser.

- 重要方法

  1. 取得使用者（使用 UserManager），並驗證密碼

  ```C#
  var user = await _userManager.FindByEmailAsync(loginDto.Email);
  bool result = await _userManager.CheckPasswordAsync(user, loginDto.Password) ;
  ```

  2. 建立新的 User

  ```C#
  // 記得先對特定的屬性進行過濾 e.g. 重複名稱與email等等
    var user = new AppUser
    {
        DisplayName = registerDto.DisplayName,
        Email = registerDto.Email,
        UserName = registerDto.UserName
    };

    var result = await _userManager.CreateAsync(user, registerDto.Password);

    if (!result.Succeeded)
    {
        return BadRequest(result.Errors);
    }
  ```

## 03 - JWT

- JWT 可以"保持使用者登入"的權限驗證方式，首先使用者先登入之後由服務端發送 JWT（JWT 為一個字符串），然後前端收到之後，會
  利用 javascript 操作網頁，將 JWT 以鍵值對的方式保存在 local storage。
- JWT 由三個部份組成 Header + Payload + Signature，Signature 會將 Header 與 Payload 加密之後得到，用來保證內容沒有經過修改，
  另外 JWT 被第三方拿到確實導被其他人登入，但是若通訊建立在 HTTPS 下就不用擔心。

### 實際操作 Part 3

1. 引用第三方包:
   1. System.IdentityModel.Tokens.Jwt : 用來生成 Jwt 的工具
   2. Microsoft.AspNetCore.Authentication.JwtBearer : 使用 JwtBearer 服務，可以配置應用程式以接受和驗證傳入的 JWT，並從中
      提取和驗證用戶身份和權限。它提供了設置驗證參數、驗證 JWT 簽名、提供用戶主體（Principal）和角色等功能。
2. 建立 Services 資料夾，並在裡面建立一個 TokenService，作為建立 token 的服務

   ```C#
   public class TokenService
   {
       private readonly IConfiguration _config;

       public TokenService(IConfiguration config)
       {
           _config = config; // 因為要使用appsettings.json
       }

       public string CreateToken(AppUser user)
       {
           // Claim is used to claim what you are.
           var claims = new List<Claim>
           {
               // Claim用來說明是這個人，也就是需要提供這些資訊作為驗證
               new Claim(ClaimTypes.Name, user.UserName), // Name Claim使用user中的UserName
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim(ClaimTypes.Email, user.Email),
           };

           // security key作為建立signature的重要值 (防止竄改)
           var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])); // it need be very longer

           // 建立signature: Credential是金鑰或者憑證的意思
           var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

           // 建立token的配置
           var tokenDescriptor = new SecurityTokenDescriptor
           {
               Subject = new ClaimsIdentity(claims),
               Expires = DateTime.UtcNow.AddDays(7),
               SigningCredentials = creds
           };

           var tokenHandler = new JwtSecurityTokenHandler();

           var token = tokenHandler.CreateToken(tokenDescriptor);

           return tokenHandler.WriteToken(token); // 返回字符串
       }
   }
   ```

3. 添加 TokenService 到 DI 容器中
   ```C#
   services.AddScoped<TokenService>();
   ```
4. 添加 JwtBearer 服務

   ```C#
    services.AddAuthenticaltion(JwtBearerDefault.AuthenticationScheme) // 這個方法要傳入一個Scheme
    .AddJwtBearer(opt => // 配置JwtBearer
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = key, // 給secret key
                        ValidateIssuerSigningKey = true, // 確認是否被竄改
                        ValidateIssuer = false, // 驗證Server
                        ValidateAudience = false, // 驗證請求者
                    };
                });

   ```

5. 啟用 Jwt 驗證與授權服務
   ```C#
   app.UseAuthentication(); // Add for JWT identification. Note: need before Authorization.
   app.UseAuthorization();
   ```
6. 對所有 Controller 啟用 Authorization，與配置 Authorization Policy，這邊我們使用全局的
   AuthorizationFilter，表示有授權者才能使用
   ```C#
   builder.AddControllers(opt =>
   {
       var policy = new AuthorizationPolicyBuilder()
                           .RequireAuthenticationUser().Builder();
        opt.Filters.Add(new AuthorizeFilter(policy)); // 添加AuthorizationFilter給所有的Controller
   })
   ```
7. 在 AccountController 裡面的 Login 與 Register "方法"中，添加[AllowAnonymous]這樣才能在沒有權限下使用。
   - Note: 若在 Controller 上添加[AllowAnonymous]，對裡面個別方法添加[Authorize]，還是 AllowAnonymous，並不會被 Authorize 覆蓋
     所以最好是在 Login 與 Register 添加[AllowAnonymous]
8. 修改 Login, Register 與 GetCurrentUser 要提供 jwt 給 UserDto

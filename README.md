# OAUTH Client Assertions

[![.NET](https://github.com/damienbod/OAuthClientAssertions/actions/workflows/dotnet.yml/badge.svg)](https://github.com/damienbod/OAuthClientAssertions/actions/workflows/dotnet.yml)

Three applications are using in this setup, an API which uses the access token, an OAuth client application implemented as a console app and a OAuth server, implemented using ASP.NET Core and Duende IdentityServer. OAuth client credentials is used to acquire the access token and the signed JWT is used to authenticate the client request.

> NOTE: The code in this repository was created from the IdentityServer samples and Duende IdentityServer.

## Blogs

### [Implement client assertions for OAuth client credential flows in ASP.NET Core](https://damienbod.com/2025/04/21/implement-client-assertions-for-oauth-client-credential-flows-in-asp-net-core/)

![flow](https://github.com/damienbod/OAuthClientAssertions/blob/main/images/OAuthCCSignedJWTAssertion.png)

### [Implement client assertions with client credentials flow using OAuth DPoP](https://damienbod.com/2025/05/12/implement-client-assertions-with-client-credentials-flow-using-oauth-dpop/)

![flow](https://github.com/damienbod/OAuthClientAssertions/blob/main/images/OAuthCCSignedJWTAssertionDPoP.png)


## Migrations

```
Add-Migration "InitializeApp" -Context ApplicationDbContext
```

```
Update-Database -Context ApplicationDbContext
```

## History

- 2026-01-07 Updated packages
- 2025-12-01 .NET 10
- 2025-09-27 Updated packages
- 2025-08-01 Updated packages
- 2025-05-12 Add DPoP and client credentials flow
- 2025-05-09 Updated packages
- 2025-04-21 Updated packages

## Links

https://docs.duendesoftware.com/identityserver/v7/tokens/authentication/jwt/

https://docs.duendesoftware.com/identityserver/v7/reference/validators/custom_token_request_validator/

https://docs.duendesoftware.com/identityserver/v7/tokens/authentication/jwt/

https://docs.duendesoftware.com/foss/accesstokenmanagement/advanced/client_assertions/

https://www.scottbrady.io/oauth/removing-shared-secrets-for-oauth-client-authentication

https://damienbod.com/2025/02/24/use-client-assertions-in-openid-connect-and-asp-net-core/

https://github.com/DuendeSoftware/products/tree/main/aspnetcore-authentication-jwtbearer

# NET-CQRS-IdentityServer4-Docker
in `terminal`
```sh
dotnet new -l

dotnet new sln -n NET-CQRS-IdentityServer4-Docker

dotnet new mvc -o source/app/identity
dotnet sln add source/app/identity

dotnet watch -p source/app/identity run

docker-compose -f source/app/docker-compose.yml up -d
```

## References

## Ports
* identity => https://localhost:5001/

# TODO:
    * dockerization
    * https docker
    * unit tests
    * settings
    * adding logger to customers
    * graphql?
# Video

https://github.com/user-attachments/assets/fb20a567-5bfa-4b99-9aaa-8a6926cfcf88

# Screenshots
![Transactions Page](https://github.com/user-attachments/assets/71d2e578-b36a-4a3d-ad9a-94b15cb96588)
![Cash Flow Page](https://github.com/user-attachments/assets/a4d56d39-4245-4e5d-a9b2-d06ce8ef9de1)


# Setting Up the API

1. Create your PostgreSQL instance however you want, and run
```sql
CREATE DATABASE sst;
```
2. Change the connection string in `api/Sst.Api/appsettings.json` appropriately
3. Change directories to `api/Sst.Database` and apply the migrations
```bash
dotnet ef database update --startup-project ../Sst.Api
```
4. Change directories back to `api`, supply your Plaid credentials via environment variables, and run the application
```bash
export PlaidClientOptions__ClientId="YOUR_CLIENT_ID"
export PlaidClientOptions__Secret="YOUR_CLIENT_SECRET"
dotnet run --project Sst.Api
```

# Setting Up the Client

1. Change directories to `client` and install dependencies
```bash
npm i
```
2. Run the development server
```bash
npm run dev
```

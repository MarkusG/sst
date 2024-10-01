# Screenshots
![Transactions page](https://i.imgur.com/ZVG3QPo.png)
![Cash flow page](https://i.imgur.com/YksOzmq.png)

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

# Setup
- get the latest Wide World Importers sample database in bacpac format from https://github.com/Microsoft/sql-server-samples/releases/tag/wide-world-importers-v1.0
- create a new Anzure SQL database from the bacpac file following this tutorial: https://docs.microsoft.com/en-us/azure/sql-database/sql-database-import
- set databse connection string in ProtobufDemo.Api\appsettings.json
- publish ProtobufDemo as Azure WebApp
- set ApiBaseUrl appSetting in ProtobufDemo.Ui\app.config
- Run ...
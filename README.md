# GSMS.API
API Project for GSMS (SWD)

## Some notes for calling API
API url: 
- Swagger: [https://gsms-api.azurewebsites.net/swagger/index.html](https://gsms-api.azurewebsites.net/swagger/index.html)
- API: https://gsms-api.azurewebsites.net/api/v1.0/

For all the endpoints, the current version is **1.0** and a Firebase JWT must be included in header as a Bearer Token.

_For Example_: **.../api/v1.0/brands...**

For the **GET** method to get list of entities:
- Sort parameter value: 1 to sort ASCENDINGLY and -1 to sort DESCENDINGLY.
- Page parameter value: 0 to get all if you don't want to paging.

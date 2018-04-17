# API Spec

The swagger spec can be found [here](specs/crif-api.yml)

# Example of Request

```js
{
	"customer":{
		"first_name":"Falk",
		"last_name":"Quintuss",
		"sex":"Male",
		"phone": {
			"country_code":"+49",
			"number":"123 4 567"
		},
		"email":"support@checkout.com"
	},
	"billing_address":{
		"street":"Rathausstrasse",
		"house":"2",
		"city":"Glücksburg",
		"zip":"24960",
		"country":"DEU"
	}
}
```

# Example of Response
```js
{
    "payment_methods": [
        "CREDIT_CARD",
        "PREPAYMENT",
        "PAYPAL"
    ]
}
```

# How to set up the service

1. Install Docker CE for Windows or Mac [here](https://www.docker.com/community-edition)
2. To run the service execute the following command in command line (_Shell_ or _Powershell_)

```shell
docker run --rm -e ASPNETCORE_URLS=http://*:5000 -e ASPNETCORE_ENVIRONMENT=Development -it -p 5000:5000 vladimiraleksandrovcko/crif-rest:16-Apr-2018
```

3. Do not close the command line window while the service is running.

4. Send requests against http://localhost:5000/creditcheck

5. Error log messages can be viewed in command line window from step 2.

6. To terminate the service press Ctrl+C in the command line (the Docker container will be automatically stopped and removed).

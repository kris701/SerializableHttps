# SerializableHttps

This is a project to make it easier to work with the `HttpClient` in C#.

The idea is that instead of manually serialising and deserialising content, you can just let this library do it for you!

In its most simple form, a HTTP call can be made as follows:
```csharp
var input = new InputModel(){
	Value = 34512,
	Name = "something"
};
var client = new SerializableHttpsClient();
var result = client.Post<InputModel,OutputModel>("https://localhost/test");
```

The library will then automatically serialise the input model correctly, and deserialise the response into the `OutputModel`;

Usually, all the body serialisation is in JSON format, however you can also give it a `XElement` as an input or output and it will serialise/deserialise as XML instead of JSON.

You can also use it to transfer file streams. There is a base model for file streaming, called `FileDataModel`, where a stream can be set to it.
The library will then serialise and deserialise it as a stream instead of a JSON document.

The serialisation used depends on the call, i.e. if its a POST, PATCH, GET or DELETE:
* DELETE and GET: Serialises the input model as a query string in the URL.
* POST and PATCH: Serialises the input model as JSON.

For this system to work, all the input and output models must be serialisable (i.e. no `dynamic` or `object`!)

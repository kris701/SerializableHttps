
<p align="center">
    <img src="https://github.com/kris701/SerializableHttps/assets/22596587/d3f387a6-e0b5-4118-9801-c125a4e64100" width="200" height="200" />
</p>

[![Build and Publish](https://github.com/kris701/SerializableHttps/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/kris701/SerializableHttps/actions/workflows/dotnet-desktop.yml)
![Nuget](https://img.shields.io/nuget/v/SerializableHttps)
![Nuget](https://img.shields.io/nuget/dt/SerializableHttps)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/kris701/SerializableHttps/main)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/kris701/SerializableHttps)
![Static Badge](https://img.shields.io/badge/Platform-Windows-blue)
![Static Badge](https://img.shields.io/badge/Platform-Linux-blue)
![Static Badge](https://img.shields.io/badge/Framework-dotnet--8.0-green)

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

You can find the project as a package on the [NuGet Package Maznager](www.link.com).
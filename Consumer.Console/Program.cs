

using Grpc.Core;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("http://localhost:5259");

var customerClient = new GrpcCon.Customer.CustomerClient(channel);

var createCustomerRequest = new GrpcCon.CreateCustomerRequest
{
    Name = "Ahmad",
    Age = 30
};

var asyncServerStreamingCall = customerClient.GetCustomer(createCustomerRequest);

await foreach (var item in asyncServerStreamingCall.ResponseStream.ReadAllAsync())
{
    Console.WriteLine(item.Message);
}
//Console.WriteLine(createCustomerReply.Message);

Console.ReadKey();

// ================================

//var services = new ServiceCollection();


//services.AddGrpcClient<GrpcCon.Customer.CustomerClient>(o =>
//{
//    o.Address = new Uri("http://localhost:5259");
//});

//var serviceProvider = services.BuildServiceProvider();

//var customerClientFromDI = serviceProvider.GetRequiredService<GrpcCon.Customer.CustomerClient>();

//var createCustomerRequestFromDI = new GrpcCon.CreateCustomerRequest
//{
//    Name = "Ahmad",
//    Age = 30
//};

//var createCustomerReplyFromDI = await customerClientFromDI.CreateAsync(createCustomerRequestFromDI);

//async static void CreateCustomer(GrpcCon.Customer.CustomerClient client)
//{
//    var createCustomerRequestFromDI = new GrpcCon.CreateCustomerRequest
//    {
//        Name = "Ahmad",
//        Age = 30
//    };

//    var createCustomerReplyFromDI = await client.CreateAsync(createCustomerRequestFromDI);
//}

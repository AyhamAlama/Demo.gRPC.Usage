

using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("http://localhost:5259");

var customerClient = new GrpcCon.Customer.CustomerClient(channel);

var createCustomerRequest = new GrpcCon.CreateCustomerRequest
{
    Name = "Ahmad",
    Age = 30
};

var createCustomerReply = await customerClient.CreateAsync(createCustomerRequest);

Console.WriteLine(createCustomerReply.Message);

Console.ReadKey();




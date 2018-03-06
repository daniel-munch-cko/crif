using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace CRIF
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<OrderCheckPortType> factory = null;  
            OrderCheckPortType serviceProxy = null;  
            Binding binding = null;

            binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);  
            factory = new ChannelFactory<OrderCheckPortType>(binding, new EndpointAddress("https://demo-ordercheck.deltavista.de/soap"));  
            serviceProxy = factory.CreateChannel();

            var input = new input(
                new MessageContext()
                {
                   credentials = new Credentials()
                   {
                       user = "",
                       password = ""
                   } 
                },
                new OrderCheckRequest() 
                {
                    product = new Product() 
                    {
                        name = "CreditCheckConsumer",
                        country = "DEU",
                        proofOfInterest = "ABK"
                    },
                    searchedAddress = new SearchedAddress()
                    {
                        legalForm = LegalForm.PERSON,
                        address = new Address() {
                            name = "Falk",
                            firstName = "Quintus",
                            gender = Gender.MALE,
                            dateOfBirth = 19680414,
                            location = new Location()
                            {
                                street = "Rathausstrasse",
                                house = "2",
                                city = "Glücksburg",
                                zip = "24960",
                                country = "DEU"
                            }
                        },
                        contact = new []{
                            new Contact() 
                            {
                                item = "email",
                                value = "f.quintus@crif.com"
                            }
                        }
                    },
                    clientData = new ClientData() 
                    {
                        reference = "Test_RCO_01",
                        order = new Order() 
                        {
                            orderValue = 78.55F
                        }
                    }
                }
            );
            
            OrderCheck(serviceProxy, input).Wait();
        }


        public static async Task OrderCheck(OrderCheckPortType serviceProxy, input input) 
        {
            try
            {
                var result = await serviceProxy.orderCheckAsync(input);
                Console.WriteLine($"Result: {result.orderCheckResponse.myDecision}");
            }
            catch(FaultException<www.deltavista.com.dspone.ordercheckif.V001.Error> ex) 
            {
                Console.WriteLine($"Error: {ex.Detail.messageText}");
            }
        }
    }
}

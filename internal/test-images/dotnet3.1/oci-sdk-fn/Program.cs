using System;
using System.Threading.Tasks;
using Fnproject.Fn.Fdk;
using Fnproject.Fn.Fdk.Context;
using Oci.IdentityService;
using Oci.Common;
using Oci.Common.Auth;

// TODO: modify this test to use RPST when its available


namespace Function
{
    public class OCISDKTestInput
    {
        public string compartmentId { get; set; }
    }
    class OCISDK
    {
        public static async Task<string> GetCompartmentID(IRuntimeContext ctx, OCISDKTestInput input)
        {
            string result = "";
            try
            {
                var provider = new ConfigFileAuthenticationDetailsProvider("DEFAULT");

                var getCompartmentRequest = new Oci.IdentityService.Requests.GetCompartmentRequest
                { CompartmentId = input.compartmentId };

                using (var client = new IdentityClient(provider, new ClientConfiguration()))
                {
                    var response = await client.GetCompartment(getCompartmentRequest);
                    result = "{\"compartmentId\":\"" + response.Compartment.Id + "\"}";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"GetCompartment Failed with {e.Message}");
            }
            return result;
        }

        static void Main(string[] args)
        {
            Fdk.Handle(args[0]);
        }
    }
}
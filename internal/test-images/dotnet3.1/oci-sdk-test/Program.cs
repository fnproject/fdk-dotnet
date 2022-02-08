using Fnproject.Fn.Fdk;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Oci.Common;
using Oci.Common.Auth;
using Oci.IdentityService;
using Oci.IdentityService.Models;
using Oci.IdentityService.Requests;
using Oci.IdentityService.Responses;


namespace example
{
    class Input
    {
        public String message { get; set; }

        public Input(){}
    }

    class Output
    {
        public String message { get; set; }

        public Output() { }

        public Output(String msg)
        {
            logger.Info("Inside oci sdk test function");

            string compartmentId = msg;
            
            // Creates an Instance Principal provider that holds authentication details of the OCI Instance
            // This helps in making API requests by the Instance without user involvement
            var instanceProvider = new InstancePrincipalsAuthenticationDetailsProvider();

            // Create a client for the service to enable using its APIs
            var identity_client = new IdentityClient(instanceProvider, new ClientConfiguration());

            try
            {
                compartment_id_from_identity_client = identity_client.get_compartment(compartment_id).Id;
                logger.Info($"compartmentId: {compartment_id_from_identity_client}");

            }
            catch (Exception e)
            {
                logger.Info($"Exception in sending request to identity client due to {e.Message}");
            }
            finally
            {
                identity_client.Dispose();
            }
        }
    }

    class Program
    {
        public static Output userFunc(IContext ctx, Input input)
        {
            return new Output(input);
        }

        static void Main(string[] args)
        {
            Fdk.Handle<Input, Output>(Program.userFunc);
        }
    }
}
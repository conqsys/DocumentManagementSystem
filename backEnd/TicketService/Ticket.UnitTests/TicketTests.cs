using Ticket.API.Common;
using Ticket.DataAccess.Common;
using Ticket.DataAccess.TicketService;
using RestSharp;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;


namespace Ticket.UnitTests
{
    public class TicketTests
    {
        BaseClient baseClient = null;
        AsyncRestClient client = null;

        private string apiUrl = "/api/WorkOrder";
        private string resource = "";
        private long id = 31;

        public TicketTests()
        {
            baseClient = new BaseClient();
            client = baseClient.GetClient(ServiceType.Security);
        }

        /// <summary>
        /// this should always pass if work order  with clientid,workorderstatusid         /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostWorkOrder()
        {
            var postObj = new
            {
                id = 0,
                clientId = 43,
                assignedUserRoleId = 1,
                assignedUserId = 1,
                batchName = "string",
                scanDate = "2017 - 03 - 10T07:36:25.146Z",
                pageNo = 0,
                referenceNo = "string",
                mrNo = "string",
                patientName = "Vikas",
                dosDate = "2017-03-10T07:36:25.146Z",
                workOrderStatusId = 1,
                amount = 100,
                clientDoctorName = "string",
                referingDoctorName = "string",
                facilityId = 0,
                processId = 0,
                requestTypeId = 0,
                comments = "string",
                createdBy = 1,
                createdDate = "2017-03-10T07:36:25.146Z",
                modifiedBy = 0,
                modifiedDate = "2017-03-10T07:36:25.146Z",
                clarificationVerified = true,
                clarificationRequired = true,
                assigneUserName = "string",
                ticketCreatedBy = "string",
                ticketStatusName = "string",
                requestTypeName = "string",
                processName = "string",
                facilityName = "string",
                coordinatorId = 0
            };

            resource = apiUrl;

            RestRequest request = baseClient.GetRequest(resource, Method.POST);
            request.AddJsonBody(postObj);

            var result = await client.ExecuteAsync<WorkOrder>(request);

            Assert.NotNull(result);
            Assert.Matches(result.Response.StatusCode.ToString(), HttpStatusCode.OK.ToString());
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Id > 0);
            Assert.True(postObj.clientId == result.Data.ClientId);

            id = result.Data.Id;
        }


        ///// <summary>
        ///// this should atleast return validation error if workorder with clientid , workorderstatusid is null or zero  
        ///// </summary>
        ///// <returns></returns>
        [Fact]
        public async Task PostWorkOrderWithRequiredValidationError()
        {
            var postObj = new
            {
                id = 0,
                clientId = 0,
                assignedUserRoleId = 1,
                assignedUserId = 1,
                batchName = "string",
                scanDate = "2017 - 03 - 10T07:36:25.146Z",
                pageNo = 0,
                referenceNo = "string",
                mrNo = "string",
                patientName = "string",
                dosDate = "2017-03-10T07:36:25.146Z",
                workOrderStatusId = 0,
                amount = 100,
                clientDoctorName = "string",
                referingDoctorName = "string",
                facilityId = 0,
                processId = 0,
                requestTypeId = 0,
                comments = "string",
                createdBy = 1,
                createdDate = "2017-03-10T07:36:25.146Z",
                modifiedBy = 0,
                modifiedDate = "2017-03-10T07:36:25.146Z",
                clarificationVerified = true,
                clarificationRequired = true,
                assigneUserName = "string",
                ticketCreatedBy = "string",
                ticketStatusName = "string",
                requestTypeName = "string",
                processName = "string",
                facilityName = "string",
                coordinatorId = 0
            };

            resource = apiUrl;

            RestRequest request = baseClient.GetRequest(resource, Method.POST);
            request.AddJsonBody(postObj);

            var result = await client.ExecuteAsync<SerializableEntityValidationCodeResult>(request);

            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.ValidationErrors);
            Assert.Contains(result.Data.ValidationErrors, (error) =>
            {
                return error.MemberNames.Contains("ClientId");
            });
            Assert.Contains(result.Data.ValidationErrors, (error) =>
            {
                return error.MemberNames.Contains("WorkOrderStatusId");
            });
        }

        /// <summary>
        /// this should always update the record
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutWorkOrder()
        {
            var patient = await GetWorkOrderById();
            patient.ReferingDoctorName = "Sona";
            patient.ClientDoctorName = "babu";

            resource = apiUrl;

            RestRequest request = baseClient.GetRequest(resource, Method.PUT);

            request.AddJsonBody(patient);

            var result = await client.ExecuteAsync<bool>(request);

            Assert.NotNull(result);
            Assert.Matches(result.Response.StatusCode.ToString(), HttpStatusCode.OK.ToString());
        }

        /// <summary>
        /// /this should return the single patient
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task<WorkOrder> GetWorkOrderById()
        {
            resource = apiUrl + "/getWorkOrderById/" + id;

            RestRequest request = baseClient.GetRequest(resource, Method.GET);

            var result = await client.ExecuteAsync<WorkOrder>(request);

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Id == id);

            return result.Data;
        }

        ///// <summary>
        ///// / this should return the list of all work orders
        ///// </summary>
        ///// <returns></returns>
        [Fact]
        public async Task FindAllWorkOrders()
        {
            resource = apiUrl + "/list";

            RestRequest request = baseClient.GetRequest(resource, Method.GET);

            var result = await client.ExecuteAsync<WorkOrder>(request);

            Assert.NotNull(result);
            Assert.Matches(result.Response.StatusCode.ToString(), HttpStatusCode.OK.ToString());

        }


        [Fact]
        public async Task DeleteWorkOrder()
        {
            resource = apiUrl + "/" + id;

            RestRequest request = baseClient.GetRequest(resource, Method.DELETE);

            var result = await client.ExecuteAsync<bool>(request);

            Assert.NotNull(result);
            Assert.Matches(result.Response.StatusCode.ToString(), HttpStatusCode.OK.ToString());
        }

    }
}

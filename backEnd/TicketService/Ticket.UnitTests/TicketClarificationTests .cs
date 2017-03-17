using Ticket.API.Common;
using Ticket.DataAccess.Common;
using Ticket.DataAccess.TicketService;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;


namespace Ticket.UnitTests
{
    public class TicketClarificationTests
    {
        BaseClient baseClient = null;
        AsyncRestClient client = null;

        private string apiUrl = "/api/WorkOrderClarification";
        private string resource = "";
        private long id = 31;
        private long workOrderId = 30;

        public TicketClarificationTests()
        {
            baseClient = new BaseClient();
            client = baseClient.GetClient(ServiceType.Security);
        }

        /// <summary>
        /// this should always pass if work order clarification  with workOrderID         /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostWorkOrderClarification()
        {
            var postObj = new
            {
                id = 0,
                workOrderId = 30,
                clarificationQuestionComment = "string",
                clarificationQuestionBy = 0,
                QuestionAssignToUserId = 0,
                clarificationAnswerComment = "string",
                clarificationAnswerBy = 0,
                clarificationAnswerDate = "2017-03-10T07:36:25.150Z",
                clarificationQuestionDate = "2017-03-10T07:36:25.150Z",
                clarificationVerifiedComment = "string",
                clarificationVerifiedDate = "2017-03-10T07:36:25.150Z",
                clarificationRequired = true,
                clarificationVerified = true,
                clarificationVerifiedBy = 0,
                clarificationQuestionUserName = "string",
                clarificationAnswerUserName = "string",
                clarificationVerifyUserName = "string"
            };

            resource = apiUrl;

            RestRequest request = baseClient.GetRequest(resource, Method.POST);
            request.AddJsonBody(postObj);

            var result = await client.ExecuteAsync<WorkOrderClarification>(request);

            Assert.NotNull(result);
            Assert.Matches(result.Response.StatusCode.ToString(), HttpStatusCode.OK.ToString());
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Id > 0);
            Assert.True(postObj.workOrderId == result.Data.WorkOrderId);

            id = result.Data.Id;
        }


        ///// <summary>
        ///// this should atleast return validation error if workorder clarification  with not  workOrderID   
        ///// </summary>
        ///// <returns></returns>
        [Fact]
        public async Task PostWorkOrderClarificationWithRequiredValidationError()
        {
            var postObj = new
            {
                id = 0,
                workOrderId = 0,
                clarificationQuestionComment = "string",
                clarificationQuestionBy = 0,
                QuestionAssignToUserId = 0,
                clarificationAnswerComment = "string",
                clarificationAnswerBy = 0,
                clarificationAnswerDate = "2017-03-10T07:36:25.150Z",
                clarificationQuestionDate = "2017-03-10T07:36:25.150Z",
                clarificationVerifiedComment = "string",
                clarificationVerifiedDate = "2017-03-10T07:36:25.150Z",
                clarificationRequired = false,
                clarificationVerified = false,
                clarificationVerifiedBy = 0,
                clarificationQuestionUserName = "string",
                clarificationAnswerUserName = "string",
                clarificationVerifyUserName = "string"
            };

            resource = apiUrl;

            RestRequest request = baseClient.GetRequest(resource, Method.POST);
            request.AddJsonBody(postObj);

            var result = await client.ExecuteAsync<SerializableEntityValidationCodeResult>(request);

            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.ValidationErrors);
        }

        /// <summary>
        /// this should always update the record
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutWorkOrderClarification()
        {
            var patient = await GetWorkOrderClarificationById();
            patient.clarificationAnswerComment = "HelloQuestion";
            patient.ClarificationAnswerDate = DateTime.Now;
            patient.QuestionAssignToUserId = 1;
            resource = apiUrl;

            RestRequest request = baseClient.GetRequest(resource, Method.PUT);

            request.AddJsonBody(patient);

            var result = await client.ExecuteAsync<bool>(request);

            Assert.NotNull(result);
            Assert.Matches(result.Response.StatusCode.ToString(), HttpStatusCode.OK.ToString());

        }



        /// <summary>
        /// this should always update the record with verification
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutWorkOrderVerification()
        {
            var patient = await GetWorkOrderClarificationById();
            patient.ClarificationVerifiedBy = 1;
            patient.ClarificationVerifiedComment = "HelloVerification";
            patient.ClarificationVerifiedDate = DateTime.Now;

            resource = apiUrl + "/verifyclarification";

            RestRequest request = baseClient.GetRequest(resource, Method.PUT);

            request.AddJsonBody(patient);

            var result = await client.ExecuteAsync<bool>(request);

            Assert.NotNull(result);
            Assert.Matches(result.Response.StatusCode.ToString(), HttpStatusCode.OK.ToString());
        }


        /// <summary>
        /// /this should return the  single clarification by id 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task<WorkOrderClarification> GetWorkOrderClarificationById()
        {
            resource = apiUrl + "/" + id;

            RestRequest request = baseClient.GetRequest(resource, Method.GET);

            var result = await client.ExecuteAsync<WorkOrderClarification>(request);

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Id == id);

            return result.Data;
        }


        /// <summary>
        /// /this should return the  list of  clarification by work order id 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task<WorkOrderClarification> GetWorkOrderClarificationByWorkOrderId()
        {
            resource = apiUrl + "/getworkorderclarificationbyworkorderid/" + workOrderId;

            RestRequest request = baseClient.GetRequest(resource, Method.GET);

            var result = await client.ExecuteAsync<WorkOrderClarification>(request);

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Id == id);

            return result.Data;
        }


    }
}

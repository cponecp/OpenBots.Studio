﻿using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace OpenBots.Core.Server.API_Methods
{
    public class AutomationMethods
    {
        public static Guid CreateAutomation(RestClient client, string name)
        {
            string automationEngine = "OpenBots";
            var request = new RestRequest("api/v1/Automations", Method.POST);
            request.RequestFormat = DataFormat.Json;
            
            request.AddJsonBody(new { name, automationEngine });

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");

            var deserializer = new JsonDeserializer();
            var output = deserializer.Deserialize<Dictionary<string, string>>(response);

            Guid automationId = Guid.Parse(output["id"]);

            return automationId;
        }

        public static void UploadAutomation(RestClient client, string name, string filePath)
        {
            Guid processId = CreateAutomation(client, name);
            var request = new RestRequest("api/v1/Automations/{id}/upload", Method.POST);
            request.AddUrlSegment("id", processId.ToString());
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "multipart/form-data"); 
            request.AddFile("File", filePath);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");
        }
    }
}

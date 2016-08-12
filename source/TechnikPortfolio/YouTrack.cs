namespace TechnikPortfolio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Newtonsoft.Json.Linq;

    using RestSharp;

    public class YouTrack
    {
        public const string NoLevel = "no level";
        public const string NoClassification = "no classification";

        public IReadOnlyList<Issue> GetIssues(string username, string password)
        {
            var client = new RestClient("https://youtrack.bbv.ch");

            var cookie = Login(client, username, password);
            var issues = GetIssues(cookie, client);

            var parsedIssues = Parse(issues);

            return parsedIssues;
        }

        private static RestResponseCookie Login(RestClient client, string username, string password)
        {
            var request = new RestRequest("rest/user/login", Method.POST);
            request.AddParameter("login", username);
            request.AddParameter("password", password);
            var response = client.Execute(request);

            ThrowIfNotOk(response, "could not login");

            return response.Cookies.Single(c => c.Name == "jetbrains.charisma.main.security.PRINCIPAL");
        }

        private static string GetIssues(RestResponseCookie cookie, RestClient client)
        {
            var request = new RestRequest("rest/issue/byproject/TP?max=1000", Method.GET);
            request.AddCookie(cookie.Name, cookie.Value);
            var response = client.Execute(request);

            ThrowIfNotOk(response, "could not get issues");

            return response.Content;
        }

        private static List<Issue> Parse(string issues)
        {
            issues = "{ \"issues\": " + issues + "}";

            JObject o = JObject.Parse(issues);

            var i = (JArray)o["issues"];

            var nodes = i.Select(ParseIssue);


            return nodes.ToList();
        }

        private static Issue ParseIssue(JToken t)
        {
            var id = ((string)t["id"]).Replace("TP-", string.Empty);

            var summary = GetTextFieldValue(t, "summary", "no summary");
            var level = GetEnumFieldValue(t, "TP Level", NoLevel).ParseValueAs<Level>();
            var linksTo = GetLinks(t);
            var classification = GetEnumFieldValue(t, "TP Klassifizierung", NoClassification).ParseValueAs<Classification>();
            var priority = GetEnumFieldValue(t, "TP Priority", "no priority");
            var types = GetMultivalueEnumFieldValues(t, "TP Type") ?? Enumerable.Empty<string>();
            var kompetenzen = GetMultivalueEnumFieldValues(t, "Kompetenz");

            return new Issue
            {
                Id = id,
                Name = summary,
                Level = level,
                LinksTo = linksTo,
                Classification = classification,
                Priority = priority,
                Types = types,
                Kompetenzen = kompetenzen
            };
        }

        private static string GetTextFieldValue(JToken t, string fieldname, string defaultValue)
        {
            string value = defaultValue;

            var levelField = (JObject)((JArray)t["field"]).SingleOrDefault(f => (string)f["name"] == fieldname);
            if (levelField != null)
            {
                value = (string)levelField["value"];
            }

            return System.Security.SecurityElement.Escape(value);
        }

        private static string GetEnumFieldValue(JToken t, string fieldname, string defaultValue)
        {
            string value = defaultValue;

            var levelField = (JObject)((JArray)t["field"]).SingleOrDefault(f => (string)f["name"] == fieldname);
            if (levelField != null)
            {
                value = (string)((JArray)levelField["value"]).Single();
            }

            return value;
        }

        private static IEnumerable<string> GetMultivalueEnumFieldValues(JToken t, string fieldname)
        {
            IEnumerable<string> values = Enumerable.Empty<string>();
            var enumsField = ((JArray)t["field"]).SingleOrDefault(f => (string)f["name"] == fieldname);
            if (enumsField != null)
            {
                values = enumsField["value"].Select(x => x.ToString());
            }

            return values;
        }

        private static IEnumerable<string> GetLinks(JToken t)
        {
            IEnumerable<string> linksTo = Enumerable.Empty<string>();
            var fieldname = "links";
            var linksField = ((JArray)t["field"]).SingleOrDefault(f => (string)f["name"] == fieldname);
            if (linksField != null)
            {
                linksTo = linksField["value"].Where(l => (string)l["role"] == "is required for").Select(l => ((string)l["value"]).Replace("TP-", string.Empty));
            }
            return linksTo;
        }

        private static void ThrowIfNotOk(IRestResponse response, string message)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"{message}: {response.StatusCode} - {response.StatusDescription}");
            }
        }
    }
}
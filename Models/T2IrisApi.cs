using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Xml;

namespace IrisProxy.Models
{
    public class T2IrisApi
    {
        private readonly CustomCredentials _credentials;

        public T2IrisApi(NetworkCredential creds)
            : this(creds.UserName, creds.Password)
        { }

        public T2IrisApi(string userName, string password)
        {
            _credentials = new CustomCredentials();
            _credentials.UserName.UserName = userName;
            _credentials.UserName.Password = password;
        }

        public string HostName { get; set; }

        public PlateInfo.PlateInfoServiceClient GetPlateInfoServiceClient()
            => GetClient<PlateInfo.PlateInfoServiceClient, PlateInfo.PlateInfoService>();

        public TransactionData.TransactionDataServiceClient GetTransactionDataServiceClient()
            => GetClient<TransactionData.TransactionDataServiceClient, TransactionData.TransactionDataService>();


        private T GetClient<T, S>()
            where T : ClientBase<S>, new()
            where S : class
        {
            var client = new T();
            if (!string.IsNullOrEmpty(HostName))
            {
                if (HostName.Contains("http://"))
                {
                    HostName = HostName.Replace("http://", "");
                }

                if (HostName.Contains("https://"))
                {
                    HostName = HostName.Replace("https://", "");
                }

                string sUri = client.Endpoint.Address.ToString();
                sUri = sUri.Replace("www.intellapay.com", HostName);
                sUri = sUri.Replace("developer.digitalpaytech.com", HostName);

                client.Endpoint.Address = new EndpointAddress(sUri);
            }

            client.Endpoint.EndpointBehaviors.Remove(typeof(ClientCredentials));
            client.Endpoint.EndpointBehaviors.Add(_credentials);

            var elements = client.Endpoint.Binding.CreateBindingElements();
            elements.Find<SecurityBindingElement>().IncludeTimestamp = false;
            client.Endpoint.Binding = new CustomBinding(elements);

            return client;
        }

        private class CustomTokenSerializer : WSSecurityTokenSerializer
        {
            public CustomTokenSerializer(SecurityVersion securityVersion)
                : base(securityVersion)
            { }

            protected override void WriteTokenCore(XmlWriter writer, SecurityToken token)
            {
                var unToken = (UserNameSecurityToken)token;
                writer.WriteRaw(
                    @"<o:UsernameToken u:Id=""" + token.Id + @""">
                    <o:Username>" + unToken.UserName + @"</o:Username>
                    <o:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">" + unToken.Password + @"</o:Password>
                    </o:UsernameToken>
                    ");
            }
        }

        private class CustomSecurityTokenManager : ClientCredentialsSecurityTokenManager
        {
            public CustomSecurityTokenManager(CustomCredentials credentials)
                : base(credentials)
            { }

            public override SecurityTokenSerializer CreateSecurityTokenSerializer(SecurityTokenVersion version)
            {
                return new CustomTokenSerializer(SecurityVersion.WSSecurity11);
            }
        }

        private class CustomCredentials : ClientCredentials
        {
            public CustomCredentials()
            { }

            protected CustomCredentials(CustomCredentials customCredentials)
                : base(customCredentials)
            { }

            public override SecurityTokenManager CreateSecurityTokenManager()
            {
                return new CustomSecurityTokenManager(this);
            }

            protected override ClientCredentials CloneCore()
            {
                return new CustomCredentials(this);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;

namespace VersaITFTP
{
    public class SecureFTP : FTPClient.FTPClient.FTPclient
    {
        private Boolean _ignoreServerMismatch = false;
        private String _securityChainElement = "";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Hostname">in either ftp://ftp.host.com or ftp.host.com form</param>
        /// <param name="Username">FTP Username</param>
        /// <param name="Password">FTP Password</param>
        /// <param name="ValidationCallback">The current ServicePointManager.ServerCertificateValidationCallback</param>
        /// <param name="KeepAlive">keep connections alive between requests (v1.1)</param>
        /// <remarks></remarks>
        public SecureFTP(String Hostname, String Username, String Password, RemoteCertificateValidationCallback ValidationCallback, Boolean KeepAlive) : base(Hostname, Username, Password, KeepAlive)
        {
            //FTPclient(Hostname, Username, Password, KeepAlive);
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateCertificate);
            //ValidationCallback = new RemoteCertificateValidationCallback(ValidateCertificate);
            this.EnableSSL = true;
            this.UsePassive = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Hostname">in either ftp://ftp.host.com or ftp.host.com form</param>
        /// <param name="Username">FTP Username</param>
        /// <param name="Password">FTP Password</param>
        /// <param name="ValidationCallback">The current ServicePointManager.ServerCertificateValidationCallback</param>
        /// <param name="SecurityChainElement">The Security Chain element that will be accepted if the certificate is not signed by a certificate authority</param>
        /// <param name="IgnoreServerMismatch">Sets whether or not the validation will allow or deny mismatches in the hostname and the certificate</param>
        /// <param name="KeepAlive">keep connections alive between requests (v1.1)</param>
        /// <remarks></remarks>
        public SecureFTP(String Hostname, String Username, String Password, RemoteCertificateValidationCallback ValidationCallback, String SecurityChainElement, Boolean IgnoreServerMismatch, Boolean KeepAlive) : base(Hostname, Username, Password, KeepAlive)
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateCertificate);
            //ValidationCallback = new RemoteCertificateValidationCallback(ValidateCertificate);
            _ignoreServerMismatch = IgnoreServerMismatch;
            _securityChainElement = SecurityChainElement;
            this.EnableSSL = true;
            this.UsePassive = true;
        }

        /// <summary>
        /// Public Boolean IgnoreServerMismatch
        /// </summary>
        /// <remarks>Gets or Sets whether or not the validation will allow or deny mismatches in the hostname and the certificate</remarks>
        public Boolean IgnoreServerMismatch
        {
            get { return _ignoreServerMismatch; }
            set { _ignoreServerMismatch = value; }
        }

        /// <summary>
        /// Public String SecurityChainElement
        /// </summary>
        /// <remarks>Gets or Sets the security chain element that will be accepted for a certificate that is not signed by a certificate authority</remarks>
        public String SecurityChainElement
        {
            get { return _securityChainElement; }
            set { _securityChainElement = value; }
        }

        private Boolean ValidateCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Boolean certOK = false;

            switch ((Int32)(sslPolicyErrors))
            {
                case 0:
                    {
                        certOK = true;
                        break;
                    }
                case 2:
                    {
                        certOK = _ignoreServerMismatch;
                        break;
                    }
                case 4:
                    {
                        certOK = ValidateChainElements(certificate, chain);
                        break;
                    }
                case 6:
                    {
                        if (_ignoreServerMismatch && ValidateChainElements(certificate, chain)) { certOK = true; }
                        else { certOK = false; }
                        break;
                    }
            }
            return certOK;
        }

        private Boolean ValidateChainElements(X509Certificate certificate, X509Chain chain)
        {
            Boolean certOK = false;

            foreach (X509ChainElement myElement in chain.ChainElements)
            {
                if (ValidateChainElementStatus(certificate, myElement)) { certOK = true; }
                else
                {
                    certOK = false;
                    break;
                }
            }

            return certOK;
        }

        private Boolean ValidateChainElementStatus(X509Certificate certificate, X509ChainElement chainElement)
        {
            Boolean certOK = false;

            foreach (X509ChainStatus mystatus in chainElement.ChainElementStatus)
            {
                if (mystatus.Status == X509ChainStatusFlags.NoError || (mystatus.Status == X509ChainStatusFlags.UntrustedRoot && certificate.Issuer == _securityChainElement)) { certOK = true; }
                else
                {
                    certOK = false;
                    break;
                }
            }

            return certOK;
        }
    }
}

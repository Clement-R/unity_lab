using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.Video;
using System.Text.RegularExpressions;

namespace YoutubeVideoStreamPlayer {
    public class YoutubeLinkExtractor : MonoBehaviour {

        public string youtubeLink = "http://www.youtube.com/watch?v=NHCgbs3TcYg";

        void Start() {

            Regex myRegex = new Regex(@"href=""(.+)"" class=""downloadButtons btn btn-lg btn-block btn-success noBorder"">Download");

            string encodedVideoUrl = Uri.EscapeDataString(youtubeLink);
            string baseUrl = "https://youtubeinmp4.com/youtube.php?url=";
            string urlRoot = "https://youtubeinmp4.com/";
            string videoUrl = "";

            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            WebClient wc = new WebClient();
            using (Stream st = wc.OpenRead(baseUrl + encodedVideoUrl)) {
                using (StreamReader sr = new StreamReader(st, Encoding.UTF8)) {
                    string html = sr.ReadToEnd();
                    Match m = myRegex.Match(html);
                    if (m.Success) {
                        videoUrl = urlRoot + m.Groups[1].Value;
                    }
                }
            }

            GetComponent<VideoPlayer>().url = videoUrl;
            GetComponent<VideoPlayer>().Play();
        }

        public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            bool isOk = true;
            // If there are errors in the certificate chain, look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None) {
                for (int i = 0; i < chain.ChainStatus.Length; i++) {
                    if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
                        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                        bool chainIsValid = chain.Build((X509Certificate2)certificate);
                        if (!chainIsValid) {
                            isOk = false;
                        }
                    }
                }
            }
            return isOk;
        }
    }

}

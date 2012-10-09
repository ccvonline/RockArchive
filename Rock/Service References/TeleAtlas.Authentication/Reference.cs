﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rock.TeleAtlas.Authentication
{

#pragma warning disable 1591
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ezlocate.na.teleatlas.com/Authentication.wsdl", ConfigurationName="TeleAtlas.Authentication.AuthenticationPortType")]
    public interface AuthenticationPortType {
        
        // CODEGEN: Generating message contract since the wrapper namespace (http://ezlocate.na.teleatlas.com/Authentication.xsd1) of message requestChallengeRequest does not match the default value (http://ezlocate.na.teleatlas.com/Authentication.wsdl)
        [System.ServiceModel.OperationContractAttribute(Action="Authentication:AuthenticationPortType#requestChallenge", ReplyAction="*")]
        Rock.TeleAtlas.Authentication.requestChallengeResponse requestChallenge(Rock.TeleAtlas.Authentication.requestChallengeRequest request);
        
        // CODEGEN: Generating message contract since the wrapper namespace (http://ezlocate.na.teleatlas.com/Authentication.xsd1) of message answerChallengeRequest does not match the default value (http://ezlocate.na.teleatlas.com/Authentication.wsdl)
        [System.ServiceModel.OperationContractAttribute(Action="Authentication:AuthenticationPortType#answerChallenge", ReplyAction="*")]
        Rock.TeleAtlas.Authentication.answerChallengeResponse answerChallenge(Rock.TeleAtlas.Authentication.answerChallengeRequest request);
        
        // CODEGEN: Generating message contract since the wrapper namespace (http://ezlocate.na.teleatlas.com/Authentication.xsd1) of message invalidateCredentialRequest does not match the default value (http://ezlocate.na.teleatlas.com/Authentication.wsdl)
        [System.ServiceModel.OperationContractAttribute(Action="Authentication:AuthenticationPortType#invalidateCredential", ReplyAction="*")]
        Rock.TeleAtlas.Authentication.invalidateCredentialResponse invalidateCredential(Rock.TeleAtlas.Authentication.invalidateCredentialRequest request);
        
        // CODEGEN: Generating message contract since the wrapper namespace (http://ezlocate.na.teleatlas.com/Authentication.xsd1) of message testCredentialRequest does not match the default value (http://ezlocate.na.teleatlas.com/Authentication.wsdl)
        [System.ServiceModel.OperationContractAttribute(Action="Authentication:AuthenticationPortType#testCredential", ReplyAction="*")]
        Rock.TeleAtlas.Authentication.testCredentialResponse testCredential(Rock.TeleAtlas.Authentication.testCredentialRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="requestChallenge", WrapperNamespace="http://ezlocate.na.teleatlas.com/Authentication.xsd1", IsWrapped=true)]
    public partial class requestChallengeRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string userName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=1)]
        public int minutesValid;
        
        public requestChallengeRequest() {
        }
        
        public requestChallengeRequest(string userName, int minutesValid) {
            this.userName = userName;
            this.minutesValid = minutesValid;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="requestChallengeResponse", WrapperNamespace="http://ezlocate.na.teleatlas.com/Authentication.xsd1", IsWrapped=true)]
    public partial class requestChallengeResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public int resultCode;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=1)]
        public int encryptedID;
        
        public requestChallengeResponse() {
        }
        
        public requestChallengeResponse(int resultCode, int encryptedID) {
            this.resultCode = resultCode;
            this.encryptedID = encryptedID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="answerChallenge", WrapperNamespace="http://ezlocate.na.teleatlas.com/Authentication.xsd1", IsWrapped=true)]
    public partial class answerChallengeRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public int encryptedResponse;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=1)]
        public int originalChallenge;
        
        public answerChallengeRequest() {
        }
        
        public answerChallengeRequest(int encryptedResponse, int originalChallenge) {
            this.encryptedResponse = encryptedResponse;
            this.originalChallenge = originalChallenge;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="answerChallengeResponse", WrapperNamespace="http://ezlocate.na.teleatlas.com/Authentication.xsd1", IsWrapped=true)]
    public partial class answerChallengeResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public int resultCode;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=1)]
        public int credential;
        
        public answerChallengeResponse() {
        }
        
        public answerChallengeResponse(int resultCode, int credential) {
            this.resultCode = resultCode;
            this.credential = credential;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="invalidateCredential", WrapperNamespace="http://ezlocate.na.teleatlas.com/Authentication.xsd1", IsWrapped=true)]
    public partial class invalidateCredentialRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public int credential;
        
        public invalidateCredentialRequest() {
        }
        
        public invalidateCredentialRequest(int credential) {
            this.credential = credential;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="invalidateCredentialResponse", WrapperNamespace="http://ezlocate.na.teleatlas.com/Authentication.xsd1", IsWrapped=true)]
    public partial class invalidateCredentialResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public int resultCode;
        
        public invalidateCredentialResponse() {
        }
        
        public invalidateCredentialResponse(int resultCode) {
            this.resultCode = resultCode;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="testCredential", WrapperNamespace="http://ezlocate.na.teleatlas.com/Authentication.xsd1", IsWrapped=true)]
    public partial class testCredentialRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string ipAddress;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=1)]
        public int credential;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=2)]
        public int serverCred;
        
        public testCredentialRequest() {
        }
        
        public testCredentialRequest(string ipAddress, int credential, int serverCred) {
            this.ipAddress = ipAddress;
            this.credential = credential;
            this.serverCred = serverCred;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="testCredentialResponse", WrapperNamespace="http://ezlocate.na.teleatlas.com/Authentication.xsd1", IsWrapped=true)]
    public partial class testCredentialResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public int resultCode;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=1)]
        public string user;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=2)]
        public string password;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=3)]
        public long expiration;
        
        public testCredentialResponse() {
        }
        
        public testCredentialResponse(int resultCode, string user, string password, long expiration) {
            this.resultCode = resultCode;
            this.user = user;
            this.password = password;
            this.expiration = expiration;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface AuthenticationPortTypeChannel : Rock.TeleAtlas.Authentication.AuthenticationPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AuthenticationPortTypeClient : System.ServiceModel.ClientBase<Rock.TeleAtlas.Authentication.AuthenticationPortType>, Rock.TeleAtlas.Authentication.AuthenticationPortType {
        
        public AuthenticationPortTypeClient() {
        }
        
        public AuthenticationPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AuthenticationPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AuthenticationPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AuthenticationPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Rock.TeleAtlas.Authentication.requestChallengeResponse Rock.TeleAtlas.Authentication.AuthenticationPortType.requestChallenge(Rock.TeleAtlas.Authentication.requestChallengeRequest request) {
            return base.Channel.requestChallenge(request);
        }
        
        public int requestChallenge(string userName, int minutesValid, out int encryptedID) {
            Rock.TeleAtlas.Authentication.requestChallengeRequest inValue = new Rock.TeleAtlas.Authentication.requestChallengeRequest();
            inValue.userName = userName;
            inValue.minutesValid = minutesValid;
            Rock.TeleAtlas.Authentication.requestChallengeResponse retVal = ((Rock.TeleAtlas.Authentication.AuthenticationPortType)(this)).requestChallenge(inValue);
            encryptedID = retVal.encryptedID;
            return retVal.resultCode;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Rock.TeleAtlas.Authentication.answerChallengeResponse Rock.TeleAtlas.Authentication.AuthenticationPortType.answerChallenge(Rock.TeleAtlas.Authentication.answerChallengeRequest request) {
            return base.Channel.answerChallenge(request);
        }
        
        public int answerChallenge(int encryptedResponse, int originalChallenge, out int credential) {
            Rock.TeleAtlas.Authentication.answerChallengeRequest inValue = new Rock.TeleAtlas.Authentication.answerChallengeRequest();
            inValue.encryptedResponse = encryptedResponse;
            inValue.originalChallenge = originalChallenge;
            Rock.TeleAtlas.Authentication.answerChallengeResponse retVal = ((Rock.TeleAtlas.Authentication.AuthenticationPortType)(this)).answerChallenge(inValue);
            credential = retVal.credential;
            return retVal.resultCode;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Rock.TeleAtlas.Authentication.invalidateCredentialResponse Rock.TeleAtlas.Authentication.AuthenticationPortType.invalidateCredential(Rock.TeleAtlas.Authentication.invalidateCredentialRequest request) {
            return base.Channel.invalidateCredential(request);
        }
        
        public int invalidateCredential(int credential) {
            Rock.TeleAtlas.Authentication.invalidateCredentialRequest inValue = new Rock.TeleAtlas.Authentication.invalidateCredentialRequest();
            inValue.credential = credential;
            Rock.TeleAtlas.Authentication.invalidateCredentialResponse retVal = ((Rock.TeleAtlas.Authentication.AuthenticationPortType)(this)).invalidateCredential(inValue);
            return retVal.resultCode;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Rock.TeleAtlas.Authentication.testCredentialResponse Rock.TeleAtlas.Authentication.AuthenticationPortType.testCredential(Rock.TeleAtlas.Authentication.testCredentialRequest request) {
            return base.Channel.testCredential(request);
        }
        
        public int testCredential(string ipAddress, int credential, int serverCred, out string user, out string password, out long expiration) {
            Rock.TeleAtlas.Authentication.testCredentialRequest inValue = new Rock.TeleAtlas.Authentication.testCredentialRequest();
            inValue.ipAddress = ipAddress;
            inValue.credential = credential;
            inValue.serverCred = serverCred;
            Rock.TeleAtlas.Authentication.testCredentialResponse retVal = ((Rock.TeleAtlas.Authentication.AuthenticationPortType)(this)).testCredential(inValue);
            user = retVal.user;
            password = retVal.password;
            expiration = retVal.expiration;
            return retVal.resultCode;
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SSTTestClient.SSTMonitorService {
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Status", Namespace="http://schemas.datacontract.org/2004/07/SSTMonitorService")]
    public enum Status : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Online = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Warning = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Offline = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SSTMonitorService.ISSTMonitorService")]
    public interface ISSTMonitorService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISSTMonitorService/GetStatus", ReplyAction="http://tempuri.org/ISSTMonitorService/GetStatusResponse")]
        SSTTestClient.SSTMonitorService.Status GetStatus();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISSTMonitorService/GetStatus", ReplyAction="http://tempuri.org/ISSTMonitorService/GetStatusResponse")]
        System.Threading.Tasks.Task<SSTTestClient.SSTMonitorService.Status> GetStatusAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISSTMonitorServiceChannel : SSTTestClient.SSTMonitorService.ISSTMonitorService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SSTMonitorServiceClient : System.ServiceModel.ClientBase<SSTTestClient.SSTMonitorService.ISSTMonitorService>, SSTTestClient.SSTMonitorService.ISSTMonitorService {
        
        public SSTMonitorServiceClient() {
        }
        
        public SSTMonitorServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SSTMonitorServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SSTMonitorServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SSTMonitorServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SSTTestClient.SSTMonitorService.Status GetStatus() {
            return base.Channel.GetStatus();
        }
        
        public System.Threading.Tasks.Task<SSTTestClient.SSTMonitorService.Status> GetStatusAsync() {
            return base.Channel.GetStatusAsync();
        }
    }
}
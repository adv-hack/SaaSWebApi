﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

//
// This source code was auto-generated by xsd, Version=4.0.30319.33440.
//

namespace SaaSDAL
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRoot("SaaSResponse")]
    public partial class SaaSResponse
    {
        [XmlElement("SaaSData")]
        private string SaaSData;

        [XmlElement("Fault")]
        private string Fault;

        [XmlElement("MessageInfo")]
        private SaaSResponseMessageInfo MessageInfo;

        /// <remarks/>
        public string _saasData
        {
            get
            {
                return this.SaaSData;
            }
            set
            {
                this.SaaSData = value;
            }
        }

        /// <remarks/>
        public string _fault
        {
            get
            {
                return this.Fault;
            }
            set
            {
                this.Fault = value;
            }
        }

        /// <remarks/>
        public SaaSResponseMessageInfo _messageInfo
        {
            get
            {
                return this.MessageInfo;
            }
            set
            {
                this.MessageInfo = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/SaaSAPI")]
    public partial class SaaSResponseMessageInfo
    {
        [XmlElement("ApplicationId")]
        private string ApplicationId;

        [XmlElement("ApplicationName")]
        private string ApplicationName;

        [XmlElement("OkToStore")]
        private bool OkToStore;

        [XmlElement("generated")]
        private System.DateTime generated;

        [XmlElement("originatingHost")]
        private string originatingHost;

        /// <remarks/>
        public string _applicationId
        {
            get
            {
                return this.ApplicationId;
            }
            set
            {
                this.ApplicationId = value;
            }
        }

        /// <remarks/>
        public string _applicationName
        {
            get
            {
                return this.ApplicationName;
            }
            set
            {
                this.ApplicationName = value;
            }
        }

        /// <remarks/>
        public bool _okToStore
        {
            get
            {
                return this.OkToStore;
            }
            set
            {
                this.OkToStore = value;
            }
        }

        /// <remarks/>
        public System.DateTime _generated
        {
            get
            {
                return this.generated;
            }
            set
            {
                this.generated = value;
            }
        }

        /// <remarks/>
        public string _originatingHost
        {
            get
            {
                return this.originatingHost;
            }
            set
            {
                this.originatingHost = value;
            }
        }
    }
}
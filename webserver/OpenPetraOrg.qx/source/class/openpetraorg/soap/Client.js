
/*
 * Copyright (c) 2008, Burak Arslan (burak.arslan-qx@arskom.com.tr).
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the Arskom Consultancy Ltd. nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED ''AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, 
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
 * AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
 * AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
 * OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
 * POSSIBILITY OF SUCH DAMAGE.
 */

qx.Class.define("openpetraorg.soap.Client", { extend : qx.core.Object
    ,construct : function(url) {
        this.base(arguments);
        // timo: somehow in my Chrome browser, the port comes twice???
        url = url.replace(/8081:8081/g, "8081");
        this.setUrl(url)
    }

    ,events : {
        "failed" : "qx.event.type.Event",
        "wsdl_failed" : "qx.event.type.Event"
    }

    ,properties : {
         timeout :          { check : "Integer", nullable : true }
        ,crossDomain :      { check : "Boolean", init : false }
        ,url :              { check : "String",  nullable : true }
    }

    ,statics : {
        NAMESPACES: {
             "a"    : "http://schemas.xmlsoap.org/wsdl/"
            ,"x"    : "http://www.w3.org/2001/XMLSchema"
            ,"plnk" : "http://schemas.xmlsoap.org/ws/2003/05/partner-link/"
            ,"soap" : "http://schemas.xmlsoap.org/wsdl/soap/"
        }
    }

    ,members : {
         __wsdl : null

        ,__extract_fault : function(method_name, async, callback, req) {
            var retval = null;
            if(req.responseXML.getElementsByTagName("faultcode").length > 0) {
                var fault_string  = req.responseXML.getElementsByTagName("faultstring")[0].childNodes[0].nodeValue;
                fault_string += "\nDetail:\n\n" + req.responseXML.getElementsByTagName("detail")[0].childNodes[0].nodeValue;
                retval = new Error(500, fault_string);
                if (!(async || callback)) {
                    throw retval;
                }
            }
            else {
                retval = new Error("No fautstring was found!");
                if (!(async || callback)) {
                    throw retval;
                }
            }
            return retval;
        }

        ,__on_send_soap_request : function(method_name, async, callback, req) {
            var retval = null;
            var nsmap = this.self(arguments).NAMESPACES;

            if (req.responseXML == null) {
                this.dispatchEvent(new qx.io.remote.Response("failed"));
            }
            else {
// timop: at the moment, the rest of this function does not go anywhere; the alerts are not shown
// therefore taking a shortcut
if(callback) {
    callback(1, req.responseXML);
}
return 1;
                
                var tag_name = qx.xml.Element.selectSingleNode(this.__wsdl,"/a:definitions/a:portType/a:operation[@name='"+method_name+"']/a:output/@name",nsmap);

                if(tag_name == null) {
alert("tag null");
                    retval = this.__extract_fault(method_name, async, callback, req);
                }
                else {
alert("tag null else");
                    var nd = req.responseXML.getElementsByTagName(tag_name.nodeValue);
                    if(nd == null || nd.length == 0) {
                        var tns = this.__wsdl.documentElement.getAttribute("targetNamespace");
                        nd = qx.xml.Element.getElementsByTagNameNS(req.responseXML, tns, tag_name.nodeValue);
                    }

                    if(nd == null || nd.length == 0) {
                        retval = this.__extract_fault(method_name, async, callback, req);
                    }
                    else {
                        retval = this.__to_object(nd[0]);
                    }
                }

                if(callback) {
                    callback(retval, req.responseXML);
                }
            }

            return retval;
        }

        ,__to_object : function(node) {
            var ssn = qx.xml.Element.selectSingleNode;
            var sn = qx.xml.Element.selectNodes;
            var nsmap = this.self(arguments).NAMESPACES;

            var retval = null;

            if(ssn(this.__wsdl,"/a:definitions/a:types", nsmap) != null) { // get return type from wsdl
                var return_type_node = ssn(this.__wsdl,"/a:definitions/a:types/x:schema/x:element[@name='"+node.nodeName+"']", nsmap);
                var return_type_name = return_type_node.getAttribute("type");
                return_type_name = return_type_name.split(":")[1];

                var return_type_definition = ssn(this.__wsdl,"/a:definitions/a:types/x:schema/x:complexType[@name='"+return_type_name+"']", nsmap);
                var retval_type_node = ssn(return_type_definition, ".//x:element",nsmap);

                retval = this.__extract(node.childNodes[0],retval_type_node);
            }
            else { // no types section so get the type directly from the message part node
                var part_node = ssn(this.__wsdl,"/a:definitions/a:message[@name='"+(node.localName || node.baseName)+"']/a:part[1]", nsmap);
                retval = this.__extract(node.childNodes[0],part_node);
            }

            return retval;
        }

        ,__extract : function(node, type_node) {
            var ssn = qx.xml.Element.selectSingleNode;
            var sn = qx.xml.Element.selectNodes;
            var nsmap = this.self(arguments).NAMESPACES;

            var retval = null;

            // get type name
            var type_name = type_node.getAttribute("type");
            if (type_name == null) {
                type_name = type_node.getAttribute("xsi:type");
            }
            type_name = type_name.split(":")[1];
            var type_name_lower = type_name.toLowerCase();

            // in case this is a primitive value, (we don't know yet)
            // get the value as well. do it here instead of doing it
            // inside every primitive case.
            var value_ = null;
            if (node.hasChildNodes()) {
                value_ = node.childNodes[0].nodeValue;
            }
            else {
                value_ = node.nodeValue;
            }

            // let's see...
            if (type_name_lower == "boolean") {
                retval = value_ + "" == "true";
            }
            else if (type_name_lower == "int" || type_name == "long") {
                retval = (value_ != null) ? parseInt(value_ + "", 10) : 0;
            }
            else if (type_name_lower == "double") {
                retval = (value_ != null) ? parseFloat(value_ + "") : 0;
            }
            else if (type_name_lower == "datetime") {
                if(value_ != null) {
                    value_ = value_ + "";
                    value_ = value_.substring(0, (value_.lastIndexOf(".") == -1 ? value_.length : value_.lastIndexOf(".")));
                    value_ = value_.replace(/T/gi," ");
                    value_ = value_.replace(/-/gi,"/");
                    retval = new Date();
                    retval.setTime(Date.parse(value_));
                }
            }
            else if (type_name_lower == "string") {
                retval = (value_ != null) ? value_ + "" : "";
            }
            else { // it's a complex type
                var type_node = ssn(this.__wsdl,"/a:definitions/a:types/x:schema/x:complexType[@name='"+type_name+"']", nsmap);
                var elts = sn(type_node, ".//x:element",nsmap);
                if (elts.length == 1 && elts[0].getAttribute("minOccurs") != null
                                     && elts[0].getAttribute("maxOccurs") != null) { // it's an array

                    retval = new Array();
                    for (var i=0; i < node.childNodes.length; i++) {
                        retval[retval.length] = this.__extract(node.childNodes[i],elts[0]);
                    }
                }
                else {
                    if (node.hasChildNodes()){
                        retval = new Object();

                        for (var i=0; i<elts.length; ++i) {
                            var nn = node.childNodes[i].nodeName;
                            if (nn == null) {
                                throw new Error("'"+nn+"' not found in the type node. This wsdl is invalid.");
                            }
                            var elt = ssn(type_node, ".//x:element[@name='"+nn+"']",nsmap);
                            retval[node.childNodes[i].nodeName] = this.__extract(node.childNodes[i],elt);
                        }
                    }
                }
            }

            return retval;
        }

        ,callSync : function(methodName, args) {
            return this.__invoke(methodName, args, false, null);
        }

        ,callAsync : function(methodName, args, handler) {
            return this.__invoke(methodName, args, true, handler);
        }

        ,__invoke : function(method, parameters, async, callback) {
            if(async) {
                this.__load_wsdl(method, parameters, async, callback);
            }
            else {
                return this.__load_wsdl(method, parameters, async, callback);
            }
        }

        // private: invoke async
        ,__load_wsdl : function(method_, parameters, async, callback) {
            if(this.__wsdl == null) {
                var xmlHttp = qx.io.remote.transport.XmlHttp.createRequestObject();
                xmlHttp.open("GET", this.getUrl() + "?wsdl=0", async);
                if(async) {
                    var self=this;
                    xmlHttp.onreadystatechange = function() {
                        if(xmlHttp.readyState == 4) {
                            self.__on_load_wsdl(method_, parameters, async, callback, xmlHttp);
                        }
                    }
                }

                xmlHttp.send(null);
                if (!async) {
                    return this.__on_load_wsdl(method_, parameters, async, callback, xmlHttp);
                }
            }
            else {
                return this.__send_soap_request(method_, parameters, async, callback);
            }
        }

        ,__on_load_wsdl : function(method_, parameters, async, callback, req) {
            this.__wsdl = req.responseXML;
            if (this.__wsdl == null) {
                this.dispatchEvent(new qx.io.remote.Response("wsdl_failed"));
                return null;
            }
            else {

                return this.__send_soap_request(method_, parameters, async, callback);
            }
        }

        ,__send_soap_request : function(method_, parameters, async, callback) {
            var ns; // namespace
            if (this.__wsdl.documentElement.attributes["targetNamespace"] + "" == "undefined") {
                ns = this.__wsdl.documentElement.attributes.getNamedItem("targetNamespace").nodeValue;
            }
            else {
                ns = this.__wsdl.documentElement.attributes["targetNamespace"].value;
            }

            // build SOAP request
            var sr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<soap:Envelope " +
                        "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                        "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                        "xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                    "<soap:Body>" +
                    "<" + method_ + " xmlns=\"" + ns + "\">" +
                    parameters.toXml() +
                    "</" + method_ + "></soap:Body></soap:Envelope>";

            // send request
            var xmlHttp = qx.io.remote.transport.XmlHttp.createRequestObject();
            xmlHttp.open("POST", this.getUrl(), async);

            var soapaction = ((ns.lastIndexOf("/") != ns.length - 1) ? ns + "/" : ns) + method_;
            xmlHttp.setRequestHeader("SOAPAction", soapaction);
            xmlHttp.setRequestHeader("Content-Type", "text/xml; charset=utf-8");

            if(async) {
                var self = this;
                xmlHttp.onreadystatechange = function() {
                    if(xmlHttp.readyState == 4) { /* FIXME: No magic numbers in the code */
                        self.__on_send_soap_request(method_, async, callback, xmlHttp);
                    }
                }
            }

            xmlHttp.send(sr);
            if (!async) {
                return this.__on_send_soap_request(method_, async, callback, xmlHttp);
            }
        }
    }
});


